using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Cardinals.Entity.UI;
using Cardinals.Enums;
using Cardinals.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Cardinals
{
    public abstract class BaseEntity : MonoBehaviour
    {
        protected int _hp;

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

        public int MaxHp { get; protected set; }
        public Action<int, int> UpdateHpEvent { get; set; }

        public Action<BaseBuff> AddBuffEvent { get; set; } 

        #region Buff Event Related
        protected ObservableCollection<BaseBuff> Buffs { get; set; }

        public Action<BaseBuff> AddNewBuffEvent { get; set; }
        public Action<BaseBuff> ExecuteBuffEvent { get; set; }

        private void OnBuffCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                BaseBuff buff = (sender as ObservableCollection<BaseBuff>).Last();
                buff.ExecuteEvent += ExecuteBuffEvent;
                AddNewBuffEvent?.Invoke(buff);
            }
        }
        #endregion
       

        private Action _turnStartedEvent;
        
        public Action DieEvent { get; set; }

        [Header("Bubble")]
        private BubbleText _bubbleText;
        public BubbleText BubbleText => _bubbleText;
        public Bubble Bubble { protected get; set; }

        [Header("Render")]
        [SerializeField] private SpriteRenderer[] _renderers;

        public SpriteRenderer[] Renderers
        {
            get => _renderers;
            set => _renderers = value;
        }
        
        [ShowInInspector] private int _defenseCount;
        public Action<int> UpdateDefenseEvent { get; set; }
        public int DefenseCount
        {
            get => _defenseCount;
            set
            {
                _defenseCount = Math.Max(0, value);
                UpdateDefenseEvent?.Invoke(_defenseCount);
            }
        }

        protected virtual Animator Animator { get; } // 추후 Abstract로..
        
        public virtual void Init(int maxHp) {
            MaxHp = maxHp;
            Hp = MaxHp;
            
            // 초기 세팅
            _bubbleText = GetComponent<BubbleText>();
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
            for (int i = Buffs.Count - 1; i >= 0; i--)
            {
                Buffs[i].Execute(this);
                Buffs[i].EndTurn();
            }

            // 방어도 초기화
            
        }

        public bool CheckBuffExist(BuffType buffType)
        {
            return (Buffs.Any(x => x.Type == buffType));
        }
        #region 기본 액션 및 버프 관련 함수
        [Button]
        /// <summary> 
        /// 맞을 때 호출
        /// </summary>
        /// <param name="damage">입힐 데미지</param>
        public virtual void Hit(int damage) 
        {
            if (DefenseCount > 0)
            {
                var calculDamage = Math.Max(0, damage - DefenseCount);
                DefenseCount -= damage;
                damage = calculDamage; 
            }
            
            if (damage > 0)
            {
                Hp -= damage;
                Animator?.Play("Hit");
                Bubble?.SetBubble(BubbleText.hit);
                transform.DOShakeScale(0.5f, .1f, 2, 45f);
                
                if (this is Player) {
                    GameManager.I.CameraController.ShakeCamera(0.3f, 2, 1);
                }
            }
        }

        
        /// <summary>
        /// 대상을 피격 시, 호출
        /// </summary>
        /// <param name="target">피격 대상</param>
        /// <param name="damage">데미지</param>
        public void Attack(BaseEntity target, int damage)
        {
            target.Hit(CalculDamage(damage));
            Bubble?.SetBubble(BubbleText.attack);
            Animator?.Play("Attack");
        }

        /// <summary>
        /// 현재 버프 혹은 값에 맞게 데미지를 계산 계산
        /// </summary>
        /// <param name="originDamage"></param>
        /// <returns></returns>
        int CalculDamage(int originDamage)
        {
            float damage = originDamage;

            if (CheckBuffExist(BuffType.Weak))
            {
                damage *= .5f;
            }
            
            return (int) Math.Floor(damage);
        }

        public int Heal(int value)
        {
            int _mathHeal = _hp + value;
            Hp += value;

            return _mathHeal - _hp;
        }
        /// <summary>
        /// 버프 추가 시, 호출
        /// 기존 버프가 존재할 경우, Count를 증가시킴
        /// </summary>
        /// <param name="buff">버프</param>
        public virtual void AddBuff(BaseBuff buff)
        {
            var existBuff = Buffs.FirstOrDefault(b => b.Type == buff.Type);
            if (existBuff == null)
            {
                buff.RemoveEvent += () => { Buffs.Remove(buff); };
                Buffs.Add(buff);
            }
            else
            {
                existBuff.Count += buff.Count;
            }
            
            AddBuffEvent?.Invoke(buff);
        }
        #endregion
    }
}