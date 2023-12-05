using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Cardinals.Board;
using System;
using System.Linq;
using Cardinals.Enums;
using Cardinals.Game;
using Cardinals.Buff;
using Sirenix.Utilities;

namespace Cardinals
{
    public class Player : BaseEntity, IBoardPiece
    {
        [Header("초기 값")] [SerializeField] private int _initHp; 
        
        [Header("Other")]
        
        [SerializeField] private Board.Tile _onTile;
        private bool _isDamagedThisTurn;
        [SerializeField] private PlayerInfo _playerInfo;

        
        private Animator _animator;
        protected override Animator Animator => (_animator ??= GetComponentInChildren<Animator>());
        public PlayerInfo PlayerInfo => _playerInfo;
        public Tile OnTile => _onTile;

        private int _boardRoundCount = 0;

        public Action<PlayerActionType> UpdateActionEvent { get; set; }
        public PlayerActionType CurActionType { get; private set; }

        public override void Init(int _ = default) {
            base.Init(_initHp);
            _playerInfo = new PlayerInfo();
            
            Debug.Log("플레이어 초기화 완료");
        }

        public override int Hp {
            get => base.Hp;
            set
            {
                var calculHp = Math.Min(Math.Max(0, value), MaxHp);

                if (calculHp != _hp)
                {
                    _hp = calculHp;
                    _isDamagedThisTurn = true;
                    UpdateHpEvent?.Invoke(_hp, MaxHp);
                    
                    if (_hp == 0)
                    {
                        Animator.SetTrigger("Die");
                        Bubble.SetBubble(BubbleText.die);
                        DieEvent?.Invoke();
                    }
                }
            }
        }

        public override void OnTurn()
        {
            _isDamagedThisTurn = false;
        }

        public override void EndTurn()
        {
            // 버프/디버프 소모
            for(int i = Buffs.Count -1; i >= 0 ; i--)
            {
                var buff = Buffs[i];
                buff.Execute(this);
                buff.EndTurn();
            }

            //GameManager.I.Next();
            GameManager.I.Stage.CardManager.EndTurn();
            GameManager.I.Player.UpdateAction(PlayerActionType.None);
            
            
            if (PlayerInfo.CheckBlessExist(BlessType.BlessEarth1))
            {
                BlessEarth1();
            }
            
            if (!PlayerInfo.CheckBlessExist(BlessType.BlessEarth2))
            {
                DefenseCount = 0;
            }
            else GameManager.I.Player.PlayerInfo.BlessEventDict[BlessType.BlessEarth2]?.Invoke();
        }

        public void EndBattle()
        {
            DefenseCount = 0;
        }

        public void BlessWater1()
        {
            if (_isDamagedThisTurn)
            {
                return;
            }

            //TODO : 힐 수치 Constants에서 끌어쓰기
            GameManager.I.Player.PlayerInfo.BlessEventDict[BlessType.BlessWater1]?.Invoke();
            Heal(3);
        }

        void BlessEarth1()
        {
            if (DefenseCount > 0)
            {
                GameManager.I.Player.PlayerInfo.BlessEventDict[BlessType.BlessEarth1]?.Invoke();
                
                var enemies = (GameManager.I.Stage.CurEvent as BattleEvent).Enemies;
                for (int i = enemies.Count - 1; i >= 0; i--)
                {
                    if (enemies[i].PrevPattern.Type == EnemyActionType.Attack)
                    {
                        enemies[i].Hit(DefenseCount);
                    }
                }
            }
        }

        public void SetMaxHP(int value)
        {
            MaxHp = value;
        }

        public void SetTile(Board.Tile tile)
        {
            _onTile = tile;
            transform.position = tile.transform.position + new Vector3(0, 1.3f, 0);
        }
      
        public IEnumerator MoveTo(int count,float time)
        {
            GameManager.I.Stage.CardManager.SetCardSelectable(false);
            _onTile?.Leave(this);
            GameManager.I.UI.UINewPlayerInfo.TileInfo.Hide();

            for(int i = 0; i < count; i++)
            {
                if (_onTile.Next == GameManager.I.Stage.Board.GetStartTile()) {
                    _boardRoundCount++;

                    GameManager.I.Stage.GenerateBoardEvent();

                    if (GameManager.I.Player.PlayerInfo.CheckArtifactExist(Enums.ArtifactType.Rigloo))
                    {
                        Heal(2);
                    }

                    if (GameManager.I.Player.PlayerInfo.CheckArtifactExist(Enums.ArtifactType.Verdant))
                    {
                        GameManager.I.Stage.Board.GetRandomTile(false).TileMagic.GainExpToNextLevel();
                    }
                }

                SetFlipOnMoveByPosition(_onTile.Next);

                Vector3 nextPos = _onTile.Next.transform.position;
                nextPos.y += 1.3f;
                transform.DOJump(nextPos, 2, 1, time);
                Animator.Play("Jump");
                yield return new WaitForSeconds(time);

                GameManager.I.Sound.PlayerMove();

                _onTile = _onTile.Next;
                CheckSummonOnTile();
                if (i != count - 1) {
                    _onTile.StepOn(this);
                }
            }

            _onTile.Arrive(this);
            GameManager.I.UI.UINewPlayerInfo.TileInfo.Show(_onTile);
            GameManager.I.Stage.CardManager.UpdateCardState(count, true);
            GameManager.I.Stage.CardManager.SetCardSelectable(true);

            SetFlipTowardEnemy();
        }
        
        public IEnumerator PrevMoveTo(int count, float time)
        {
            GameManager.I.Stage.CardManager.SetCardSelectable(false);
            _onTile?.Leave(this);
            GameManager.I.UI.UINewPlayerInfo.TileInfo.Hide();

            for (int i = 0; i < count; i++)
            {
                SetFlipOnMoveByPosition(_onTile.Prev);
                Vector3 prevPos = _onTile.Prev.transform.position;
                prevPos.y += 1.3f;
                transform.DOJump(prevPos, 2, 1, time);
                Animator.Play("Jump");
                yield return new WaitForSeconds(time);
                _onTile = _onTile.Prev;
                CheckSummonOnTile();
                if (i != count - 1)
                {
                    _onTile.StepOn(this);
                }
            }
            _onTile.Arrive(this);
            GameManager.I.UI.UINewPlayerInfo.TileInfo.Show(_onTile);
            GameManager.I.Stage.CardManager.UpdateCardState(count, true);
            GameManager.I.Stage.CardManager.SetCardSelectable(true);
            
            SetFlipTowardEnemy();
        }

        private void CheckSummonOnTile()
        {
            var summons = GameManager.I.Stage.Summons;
            for (int i = summons.Count - 1; i >= 0; i--)
            {
                if (_onTile == summons[i].OnTile)
                {
                    summons[i].Execute();
                }
            }

        }
        
        public IEnumerator CardAction(int num, BaseEntity target) {
            GameManager.I.Stage.CardManager.SetCardSelectable(false);
            
            yield return _onTile.CardAction(num, target);
            yield return null;

            // [유물] 워프 부적
            if (GameManager.I.Player.PlayerInfo.CheckArtifactExist(Enums.ArtifactType.Warp)
                && num == 4)
            {
                GameManager.I.Stage.CardManager.WarpArtifact();
            }

            GameManager.I.Stage.CardManager.SetCardSelectable(true);
        }
        
        [Button]
        public void TestAddBuff(BuffType buffType)
        {
           // AddBuff(new Slow());
            AddBuff(new ElectricShock());
        }


        public void Defense(int value)
        {
            DefenseCount += value;
            Animator.Play("Shield");
        }

        public void Win()
        {   
            Animator.Play("Win");
            Bubble.SetBubble(BubbleText.win);
            for (int i = Buffs.Count - 1; i >= 0; i--)
            {
                Buffs[i].EndEvent();
            }
        }

        public void UpdateAction(PlayerActionType type)
        {
            if (type != CurActionType)
            {
                CurActionType = type;
                UpdateActionEvent?.Invoke(type);
            }
        }


        void SetFlipOnMoveByPosition(Tile moveTile)
        {
            bool filpX = true; // false일 때, 좌측을 바라봄

            var list = GameManager.I.Stage.Board.TileSequence;
            var curIdx = list.IndexOf(_onTile);
            var nextIdx = list.IndexOf(moveTile);

            var value = list.Count / 4;
            if (nextIdx <= value || nextIdx > value * 3)
            {
                filpX = false; 
            }
            
            if (nextIdx != 0 && curIdx > nextIdx)
            {
                filpX = !filpX;
            }
            
            Renderers.ForEach(r => r.flipX = filpX);
            bool enemyFlipX = nextIdx > list.Count / 2;
            GameManager.I.Stage.Enemies.ForEach(e => e.Renderer.FlipX(enemyFlipX));
        }

        void SetFlipTowardEnemy()
        {
            bool filpX = true;  // false일 때, 좌측을 바라봄
            
            var list = GameManager.I.Stage.Board.TileSequence;
            var curIdx = (list.IndexOf(_onTile)) % list.Count;
            if (curIdx > list.Count / 2)
            {
                filpX = false;
            }
            
            Renderers.ForEach(r => r.flipX = filpX);
        }
    }
}

