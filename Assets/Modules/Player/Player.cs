using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Cardinals.Board;
using System;

namespace Cardinals
{
    public class Player : BaseEntity, IBoardPiece
    {
        [SerializeField] private Board.Tile _onTile;
        private bool _isDamagedThisTurn;
        [SerializeField] private PlayerInfo _playerInfo;

        public PlayerInfo PlayerInfo => _playerInfo;
        public Player() : base(12){

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
            FindAnyObjectByType<CardManager>().OnTurn();
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
            FindAnyObjectByType<CardManager>().EndTurn();
            if (!PlayerInfo.IsBless6)
            {
                DefenseCount = 0;
            }
        }

        public void Bless3()
        {
            if (_isDamagedThisTurn)
            {
                return;
            }

            //TODO : 힐 수치 Constants에서 끌어쓰기
            Heal(3);
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

        public IEnumerator CardAction(int num) {
            _onTile.CardAction(num);
            yield return null;
        }

        public void Heal(int value)
        {
            Hp += value;
        }
    }
}

