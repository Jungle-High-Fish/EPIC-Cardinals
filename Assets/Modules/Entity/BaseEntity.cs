using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Cardinals.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Cardinals
{
    public abstract class BaseEntity : MonoBehaviour
    {
        private int _hp;

        public virtual int Hp
        {
            get => _hp;
            set
            {
                var calculHp = Math.Min(Math.Max(0, value), MaxHp);

                if (calculHp != _hp)
                {
                    _hp = calculHp;
                    UpdateHpEvent?.Invoke(_hp, MaxHp);
                    if (_hp == 0)
                    {
                        DieEvent?.Invoke();
                    }
                }
            }
        }

        public int MaxHp { get; private set; }
        public Action<int, int> UpdateHpEvent { get; set; }

        #region Buff Event Related
        private ObservableCollection<BaseBuff> Buffs { get; set; }

        public Action<BaseBuff> AddBuffEvent { get; set; }

        private void OnBuffCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                BaseBuff buff = (sender as ObservableCollection<BaseBuff>).First();
                AddBuffEvent?.Invoke(buff);
            }
        }
        #endregion
        
       

        private Action _turnStartedEvent;
        public Action DieEvent { get; set; }


        private int _defenseCount;

        public int DefenseCount
        {
            get => _defenseCount;
            set
            {
                _defenseCount = Math.Max(0, value);   
            }
        }
        
        public BaseEntity(int maxHp)
        {
            MaxHp = maxHp;
            Hp = MaxHp;
            
            // 초기 세팅
            Buffs = new();
            Buffs.CollectionChanged += OnBuffCollectionChanged;
            DieEvent += () => { Buffs.CollectionChanged -= OnBuffCollectionChanged; };
        }

        public void Init(int maxHp) {
            MaxHp = maxHp;
            Hp = MaxHp;
            
            // 초기 세팅
            Buffs = new();
            Buffs.CollectionChanged += OnBuffCollectionChanged;
            DieEvent += () => { Buffs.CollectionChanged -= OnBuffCollectionChanged; };
        }

        /// <summary>
        /// 새로운 턴 시작 시, 수행할 액션
        /// </summary>
        public virtual void StartTurn()
        {
            _turnStartedEvent?.Invoke();
        }
        
        /// <summary>
        /// 본인 턴 때, 수행할 액션
        /// </summary>
        public abstract void OnTurn();

        public virtual void EndTurn()
        {
            // 버프/디버프 소모
            foreach (var buff in Buffs)
            {
                buff.Execute(this);
                buff.EndTurn();
            }
        }

        #region 기본 액션 및 버프 관련 함수
        [Button]
        /// <summary>
        /// 맞을 때 호출
        /// </summary>
        /// <param name="damage">입힐 데미지</param>
        public void Hit(int damage)
        {
            int calculDamage = damage - DefenseCount ;

            if (calculDamage > 0)
            {
                DefenseCount -= damage;
            }
            else
            {
                DefenseCount = 0 ;
            }
            
            Hp -= calculDamage;
        }

        
        /// <summary>
        /// 대상을 피격 시, 호출
        /// </summary>
        /// <param name="target">피격 대상</param>
        /// <param name="damage">데미지</param>
        public void Attack(BaseEntity target, int damage)
        {
            int calculDamage = Buffs.Any(x => x.Type == BuffType.Weak) ? damage / 2 : damage;
            
            target.Hit(calculDamage);
        }

        
        /// <summary>
        /// 버프 추가 시, 호출
        /// 기존 버프가 존재할 경우, Count를 증가시킴
        /// </summary>
        /// <param name="buff">버프</param>
        public void AddBuff(BaseBuff buff)
        {
            var existBuff = Buffs.FirstOrDefault(b => b.Type == buff.Type);
            if (existBuff == null)
            {
                Buffs.Add(buff);
            }
            else
            {
                existBuff.Count += buff.Count;
            }
        }
        #endregion
    }
}