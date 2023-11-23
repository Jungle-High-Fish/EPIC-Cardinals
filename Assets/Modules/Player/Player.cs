using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Cardinals.Board;
using System;
using Cardinals.Enums;
using Cardinals.Game;
using Cardinals.Buff;

namespace Cardinals
{
    public class Player : BaseEntity, IBoardPiece
    {
        [SerializeField] private Board.Tile _onTile;
        private bool _isDamagedThisTurn;
        [SerializeField] private PlayerInfo _playerInfo;

        public PlayerInfo PlayerInfo => _playerInfo;
        public Tile OnTile => _onTile;

        public override void Init(int hp) {
            base.Init(hp);
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
                        DieEvent?.Invoke();
                    }
                }
            }
        }
        public override void OnTurn()
        {
            GameManager.I.Stage.CardManager.OnTurn();
            _isDamagedThisTurn = false;
        }

        public override void EndTurn()
        {
            // 버프/디버프 소모
            foreach (var buff in Buffs)
            {
                buff.Execute(this);
                buff.EndTurn();
            }

            GameManager.I.Next();
            GameManager.I.Stage.CardManager.EndTurn();
            
            
            if (PlayerInfo.CheckBlessExist(BlessType.BlessEarth1))
            {
                BlessEarth1();
            }
            
            if (!PlayerInfo.CheckBlessExist(BlessType.BlessEarth2))
            {
                DefenseCount = 0;
            }

        }

        public void BlessWater1()
        {
            if (_isDamagedThisTurn)
            {
                return;
            }

            //TODO : 힐 수치 Constants에서 끌어쓰기
            Heal(3);
        }

        void BlessEarth1()
        {
            if (DefenseCount > 0)
            {
                foreach (var enemy in (GameManager.I.Stage.CurEvent as BattleEvent).Enemies)
                {
                    if (enemy.CurPattern.Type == EnemyActionType.Attack)
                    {
                        enemy.Hit(DefenseCount);
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
            _onTile?.Leave(this);
            for(int i = 0; i < count; i++)
            {
                Vector3 nextPos = _onTile.Next.transform.position;
                nextPos.y += 1.3f;
                transform.DOJump(nextPos, 2, 1, time);
                yield return new WaitForSeconds(time);
                _onTile = _onTile.Next;
                if (i != count - 1) {
                    _onTile.StepOn(this);
                }
            }
            _onTile.Arrive(this);
        }
        
        public IEnumerator PrevMoveTo(int count, float time)
        {
            _onTile?.Leave(this);
            for (int i = 0; i < count; i++)
            {
                Vector3 prevPos = _onTile.Prev.transform.position;
                prevPos.y += 1.3f;
                transform.DOJump(prevPos, 2, 1, time);
                yield return new WaitForSeconds(time);
                _onTile = _onTile.Prev;
                if (i != count - 1)
                {
                    _onTile.StepOn(this);
                }
            }
            _onTile.Arrive(this);
        }

        public IEnumerator CardAction(int num, BaseEntity target) {
            _onTile.CardAction(num, target);
            yield return null;
        }


        [Button]
        public void TestAddBuff(BuffType buffType)
        {
            AddBuff(new ElectricShock());
        }
      

        
    }
}

