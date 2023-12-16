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
using Unity.Mathematics;
using Modules.Entity.Buff;
using Util;

namespace Cardinals
{
    public class Player : BaseEntity, IBoardPiece
    {
        [Header("초기 값")] [SerializeField] private int _initHp; 
        
        [Header("Other")]
        
        [SerializeField] private Board.Tile _onTile;
        private bool _isDamagedThisTurn;
        [InlineEditor]
        [SerializeField] private PlayerInfo _playerInfo;

        
        
        private Animator _animator;
        protected override Animator Animator => (_animator ??= GetComponentInChildren<Animator>());
        
        public PlayerInfo PlayerInfo => _playerInfo;
        public Tile OnTile => _onTile;

        private int _boardRoundCount = 0;

        public Action<PlayerActionType> UpdateActionEvent { get; set; }
        public PlayerActionType CurActionType { get; private set; }

        private Quaternion _defaultRotate;

        public override void Init(int _ = default) {
            base.Init(_initHp);
            _playerInfo = new PlayerInfo();

            _defaultRotate = Renderers.First().transform.rotation;
        }

        public void SetData(PotionType[] potionList, BlessType[] blessList, int coin, int maxHp, int hp)
        {
            potionList.ForEach(p => _playerInfo.AddPotion(p));
            blessList.ForEach(b => _playerInfo.GetBless(b));
            _playerInfo.SetGold(coin);
            Hp = hp;
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

        public override IEnumerator OnTurn()
        {
            _isDamagedThisTurn = false;
            
            if (CheckBuffExist(BuffType.Stun))
            {
                // 행동 하지 않음... 스턴 효과 애니메이션 출력?
            }
            else
            {
                GameManager.I.UI.UIEndTurnButton.Activate();

                yield return GameManager.I.WaitNext(); // 플레이어의 [턴 종료] 버튼 선택 대기

                GameManager.I.UI.UIEndTurnButton.Deactivate();
                GameManager.I.Stage.DiceManager.SetDiceSelectable(false);
            }
        }

        public IEnumerator PreEndTurn()
        {
            if (PlayerInfo.CheckBlessExist(BlessType.BlessEarth2) && DefenseCount == 0)
            {
                AddDefenseCount(4);
                GameManager.I.Player.PlayerInfo.BlessEventDict[BlessType.BlessEarth2]?.Invoke();
            }

            yield break;
        }
        public override IEnumerator EndTurn()
        {
            yield return base.EndTurn(); // 버프/디버프 소모
            
            //yield return GameManager.I.Stage.CardManager.EndTurn();
            yield return GameManager.I.Stage.DiceManager.EndTurn();
            GameManager.I.Player.UpdateAction(PlayerActionType.None);
            
            if (PlayerInfo.CheckBlessExist(BlessType.BlessEarth1))
            {
                BlessEarth1();
            }

            ResetDefenseCount();
        }

        public void EndBattle()
        {
            ResetDefenseCount();
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
                
                var enemies = GameManager.I.CurrentEnemies.ToList();
                for (int i = enemies.Count - 1; i >= 0; i--)
                {
                    if (enemies[i].PrevPattern?.Type == EnemyActionType.Attack)
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

        public Action HomeReturnEvent { get; set; }
      
        public IEnumerator MoveTo(int count,float time)
        {
            _onTile?.Leave(this);
            GameManager.I.UI.UINewPlayerInfo.TileInfo.Hide();

            for(int i = 0; i < count; i++)
            {
                if (_onTile.Next == GameManager.I.Stage.Board.GetStartTile()) {
                    _boardRoundCount++;

                    HomeReturnEvent?.Invoke();

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

                yield return new WaitForSeconds(time / 2);
                GameManager.I.Sound.PlayerMove();
                yield return new WaitForSeconds(time / 2);

                _onTile = _onTile.Next;
                yield return CheckCollisionBoardObject();
                if (i != count - 1) {
                    _onTile.StepOn(this);
                }
            }

            yield return CheckArriveBoardObject();

            _onTile.Arrive(this);
            GameManager.I.UI.UINewPlayerInfo.TileInfo.Show(_onTile);
            GameManager.I.Stage.CardManager.UpdateCardState(count, true);

            SetFlipTowardEnemy();
        }
        
        public IEnumerator PrevMoveTo(int count, float time)
        {
            GameManager.I.Stage.DiceManager.SetDiceSelectable(false);
            _onTile?.Leave(this);
            GameManager.I.UI.UINewPlayerInfo.TileInfo.Hide();

            for (int i = 0; i < count; i++)
            {
                SetFlipOnMoveByPosition(_onTile.Prev);
                Vector3 prevPos = _onTile.Prev.transform.position;
                prevPos.y += 1.3f;
                transform.DOJump(prevPos, 2, 1, time);
                Animator.Play("Jump");

                yield return new WaitForSeconds(time / 2);
                GameManager.I.Sound.PlayerMove();
                yield return new WaitForSeconds(time / 2);

                _onTile = _onTile.Prev;
                yield return CheckCollisionBoardObject();
                if (i != count - 1)
                {
                    _onTile.StepOn(this);
                }
            }

            yield return CheckArriveBoardObject();
            
            _onTile.Arrive(this);
            GameManager.I.UI.UINewPlayerInfo.TileInfo.Show(_onTile);
            GameManager.I.Stage.DiceManager.UpdateDiceState(count, true);
            GameManager.I.Stage.DiceManager.SetDiceSelectable(true);
            
            SetFlipTowardEnemy();
        }

        private IEnumerator CheckCollisionBoardObject()
        {
            var summons = GameManager.I.Stage.BoardObjects;
            for (int i = summons.Count - 1; i >= 0; i--)
            {
                if (_onTile == summons[i].OnTile)
                {
                    yield return summons[i].OnCollisionPlayer();
                }
            }
        }
        
        private IEnumerator CheckArriveBoardObject()
        {
            var summons = GameManager.I.Stage.BoardObjects;
            for (int i = summons.Count - 1; i >= 0; i--)
            {
                if (_onTile == summons[i].OnTile)
                {
                    yield return summons[i].ArrivePlayer();
                }
            }
        }
        
        public IEnumerator CardAction(int num, BaseEntity target) {
            
            yield return _onTile.CardAction(num, target);
            yield return null;

            // [유물] 워프 부적
            /*if (GameManager.I.Player.PlayerInfo.CheckArtifactExist(Enums.ArtifactType.Warp)
                && num == 4)
            {
                GameManager.I.Stage.CardManager.WarpArtifact();
            }*/

        }
        
        [Button]
        public void TestAddBuff(BuffType buffType)
        {
           // AddBuff(new Slow());
            AddBuff(new Confusion());
        }

        public override void Attack(BaseEntity target, int damage)
        {
            Bubble?.SetBubble(BubbleText.attack);
            Animator?.Play("Attack");

            var targetPos = target.transform.position + new Vector3(0, -1, 0);
            var obj = Instantiate(ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Particle_Attack));
            obj.transform.position = transform.position;
            
            obj.transform.DOJump(targetPos, 3, 1, .8f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    target.Hit(CalculDamage(damage));

                    var explosion = Instantiate(ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Particle_Explosion));
                    explosion.transform.position = targetPos;
                    
                    Destroy(obj);
                });
        }

        public void Defense(int value)
        {
            AddDefenseCount(value);
            Animator.Play("Shield");
        }

        [Button]
        public override int Heal(int value)
        {

            if (PlayerInfo.CheckBlessExist(BlessType.BlessWater2))
            {
                GameManager.I.Player.PlayerInfo.BlessEventDict[BlessType.BlessWater2]?.Invoke();
                int damage = (MaxHp -Hp) < value ? (MaxHp - _hp) : value;
                GameManager.I.Stage.Enemies[UnityEngine.Random.Range(0, GameManager.I.Stage.Enemies.Count)].Hit(damage);
            }

            int _mathHeal = _hp + value;
            Hp += value;

            

            return _mathHeal - _hp;
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
            

            Renderers.ForEach(r => Flip(r, filpX));
            //Renderers.ForEach(r => r.flipX = filpX);
            bool enemyFlipX = nextIdx > list.Count / 2;

            GameManager.I.Stage.Enemies.ForEach(e => Flip(e.Renderers.First(), enemyFlipX));
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

            Renderers.ForEach(r => Flip(r, filpX));
        }

        void Flip(SpriteRenderer renderer,  bool filpX)
        {
            if (renderer.flipX != filpX)
            {
                renderer.transform.DORotate(new Vector3(0, 180, 0), 0.25f, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.InCirc)
                    .OnComplete(() =>
                    {
                        renderer.flipX = filpX;
                        renderer.transform.rotation = _defaultRotate;
                    });
            }
        }

        public void MotionThinking()
        {
            Animator.Play("Thinking");
        }

        public void MotionIdle()
        {
            Animator.Play("Idle");
        }
        
        public void MotionWorry()
        {
            Animator.Play("Worry");
        }
    }
}

