using Cardinals.Buff;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Cardinals.Entity.UI;
using Cardinals.Enums;
using Cardinals.Game;
using Cardinals.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Steamworks.ServerList;
using UnityEngine;
using Util;

namespace Cardinals
{
    public abstract class BaseEntity : MonoBehaviour
    {
        protected int _hp;

        public virtual int Hp
        {
            get => _hp;
            protected set
            {
                var calculHp = Mathf.Clamp(value, 0, MaxHp);

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

        private int _maxHP;
        public int MaxHp {
            get => _maxHP;
            protected set
            {
                _maxHP = value;
                _hp = Mathf.Clamp(_hp, 0, _maxHP);

                UpdateHpEvent?.Invoke(_hp, _maxHP);
            } 
        }
        public Action<int, int> UpdateHpEvent { get; set; }

        public Action<BaseBuff> AddBuffEvent { get; set; } 

        #region Buff Event Related
        protected ObservableCollection<BaseBuff> Buffs { get; set; }

        public Action<BaseBuff> AddNewBuffEvent { get; set; }
        public Action<BaseBuff> ExecuteBuffEvent { get; set; }
        public Action SuccessDefenseEvent { get; set; }
        public Action BrokenDefenseEvent { get; set; }
        public Action<int, Color> ValidChangedHPEvent { get; set; }

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
        public Bubble Bubble { get; set; }

        [Header("Render")]
        [SerializeField] private SpriteRenderer[] _renderers;
        public SpriteRenderer[] Renderers
        {
            get => _renderers;
            set
            {
                _renderers = value;
                _defaultRotate = Renderers.First().transform.rotation;
            }
        }
        protected Quaternion _defaultRotate { get; private set; }
        
        [ShowInInspector] private int _defenseCount;
        public Action<int> UpdateDefenseEvent { get; set; }
        protected int DefenseCount
        {
            get => _defenseCount;
            private set
            {
                _defenseCount = Math.Max(0, value);
                UpdateDefenseEvent?.Invoke(_defenseCount);
            }
        }

        protected virtual Animator Animator { get; set; } // 추후 Abstract로..
        protected Action HitEvent;
        
        public virtual void Init(int maxHp) {
            MaxHp = maxHp;
            Hp = MaxHp;
            
            // 초기 세팅
            _bubbleText = GetComponent<BubbleText>();
            Buffs = new();
            Buffs.CollectionChanged += OnBuffCollectionChanged;
            DieEvent += () => { Buffs.CollectionChanged -= OnBuffCollectionChanged; };
            
            // Effects
            HitEvent += () =>
            {
                Animator?.Play("Hit");
                Bubble?.SetBubble(TMPUtils.LocalizedText(BubbleText.hit));
                transform.DOShakeScale(0.5f, .1f, 2, 45f);
            };
        }

        public virtual void PostInit()
        {
            
        }

        /// <summary>
        /// 새로운 턴 시작 시, 수행할 액션
        /// </summary>
        public virtual IEnumerator StartTurn()
        {
            _turnStartedEvent?.Invoke();

            yield return null;
        }

        public virtual IEnumerator OnPreTurn()
        {
            yield return null;
        }

        /// <summary>
        /// 본인 턴 때, 수행할 액션
        /// </summary>
        public abstract IEnumerator OnTurn();
        public virtual IEnumerator OnPostTurn()
        {
            yield return null;
        }

        public virtual float OnBuff()
        {   
            float execTime = 0;
            foreach (var buff in Buffs)
            {
                if (buff.GetType().GetMethod("Execute").DeclaringType == typeof(BaseBuff)) {
                    execTime = 0.5f;
                }
            }
            //Buffs.ForEach(b => b.Execute(this));
            for (int i = Buffs.Count - 1; i >= 0; i--)
            {
                Buffs[i].Execute(this);
            }
            return execTime;
        }

        public virtual IEnumerator EndTurn()
        {
            // 버프/디버프 소모
            for (int i = Buffs.Count - 1; i >= 0; i--)
            {
                Buffs[i].EndTurn();
            }

            yield return null;
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
        public virtual void Hit(int damage, TileMagicType type = TileMagicType.Attack) 
        {
            if (DefenseCount > 0)
            {
                var calculDamage = Math.Max(0, damage - DefenseCount);
                DefenseCount -= damage;
                damage = calculDamage;

                if (damage > 0)
                {
                    GameManager.I.Sound.PlayerDefenseHit();
                    BrokenDefenseEvent?.Invoke();
                }
                else
                {
                    GameManager.I.Sound.PlayerDefenseHit();
                    SuccessDefenseEvent?.Invoke();
                }
            }
            
            if (damage > 0)
            {
                // 데미지 표시기 
                var color = type == TileMagicType.Attack ? Color.white :  EnumHelper.GetColorByMagic(type);
                ValidChangedHPEvent?.Invoke(-damage, color);
                
                HitEvent?.Invoke();
                Hp -= damage;
                
                GameManager.I.Sound.PlayerHit();
                var gain = Math.Clamp(damage / 2, 1, 5);
                var duration = Math.Clamp(damage / 5f, .3f, .6f);
                GameManager.I.CameraController.ShakeCamera(duration, gain, 1);
                
                // if (this is Player) {
                //     GameManager.I.Sound.PlayerHit();
                //     GameManager.I.CameraController.ShakeCamera(0.3f, 2, 1);
                // } else if (this is BaseEnemy) {
                //     GameManager.I.CameraController.ShakeCamera(0.3f, 1f, 1);
                // }
            }
        }

        
        /// <summary>
        /// 대상을 피격 시, 호출
        /// </summary>
        /// <param name="target">피격 대상</param>
        /// <param name="damage">데미지</param>
        public virtual void Attack(BaseEntity target, int damage)
        {
            target.Hit(CalculDamage(damage));
            Bubble?.SetBubble(TMPUtils.LocalizedText(BubbleText.attack));
            Animator?.Play("Attack");
        }

        /// <summary>
        /// 현재 버프 혹은 값에 맞게 데미지를 계산 계산
        /// </summary>
        /// <param name="originDamage"></param>
        /// <returns></returns>
        protected int CalculDamage(int originDamage)
        {
            float damage = originDamage;

            if (CheckBuffExist(BuffType.Weak))
            {
                damage *= .5f;
            }
            
            return (int) Math.Floor(damage);
        }

        protected void ResetDefenseCount(int resetValue = 0) => DefenseCount = resetValue;
        public void AddDefenseCount(int value)
        {
            GameManager.I.Sound.GetDefense();
            if (CheckBuffExist(BuffType.Powerless))
                value = (int)Math.Floor((float) value / 2);

            DefenseCount += value;
        }

        public virtual int Heal(int value)
        {
            GameManager.I.Sound.PlayerHeal();
            var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Particle_HealParticle);
            var obj = Instantiate(prefab, Renderers.First().transform.parent);
            //obj.transform.position = new Vector3(0, -1, -1);

            int realRecoveryHeal = Math.Min(MaxHp - Hp, value);
            if (realRecoveryHeal > 0)
            {
                var color = EnumHelper.GetColorByMagic(TileMagicType.Water);
                ValidChangedHPEvent(realRecoveryHeal, color);
            }
            
            int _mathHeal = _hp + value;
            Hp += value;

            int overHeal = _mathHeal - _hp;
            
            return overHeal;
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
        
        

        /// <summary>
        /// 두 transform의 위치를 비교하여, tr2가 tr1보다 우측에 있다면 false를 반환
        /// </summary>
        /// <returns></returns>
        protected bool ComparePivot(Transform tr1, Transform tr2)
        {
            var pivot1 = (tr1.position.x + tr1.position.z) / 2;
            var pivot2 = (tr2.position.x + tr2.position.z) / 2;

            return pivot1 < pivot2;
        }
        
        public virtual void Flip(bool filpX,  Quaternion quat = default)
        {
            foreach (SpriteRenderer renderer in Renderers)
            {
                if (renderer.flipX != filpX)
                {
                    renderer.transform.DORotate(new Vector3(0, 180, 0), 0.25f, RotateMode.LocalAxisAdd)
                        .SetEase(Ease.InCirc)
                        .OnComplete(() =>
                        {
                            renderer.flipX = filpX;
                            renderer.transform.rotation = quat == default ? _defaultRotate : quat;
                        });
                }
            }
        }

        public Growth GetGrowthBuff()
        {
            if (CheckBuffExist(BuffType.Growth))
            {
                return Buffs.First(b => b.Type == BuffType.Growth) as Growth;
            }
            else return null;
        }
    }
}