using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cardinals.Board;
using Cardinals.Enemy;
using Cardinals.Enums;
using Cardinals.Game;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Util;

namespace Cardinals
{
    public abstract class BaseEnemy : BaseEntity
    {
        public string Name { get; private set; }
        
        protected int Turn { get; set; }

        private ComponentGetter<Animator> _animator
            = new ComponentGetter<Animator>(TypeOfGetter.Child);
        protected override Animator Animator
        {
            get
            {
                return _animator.Get(gameObject);
            }
        }

        private Reward[] _rewards;
        public Reward[] Rewards
        {
            get => _rewards;
            protected set => _rewards = value;
        }

        private Pattern[] _patterns; 
        protected Pattern[] Patterns 
        { 
            get => _patterns;
            set
            {
                _patterns = value;
                UpdatePatternEvent(_patterns.First());
            }
        }

        private Pattern  _fixPattern;
        protected Pattern FixPattern
        {
            get => _fixPattern;
            set
            {
                _fixPattern = value;
                UpdatePatternEvent?.Invoke(_fixPattern);
            }
        }
        
        public Action<Pattern> UpdatePatternEvent { get; set; }

        private bool _berserkMode;
        public bool BerserkMode 
        { 
            get => _berserkMode;
            set
            {
                _berserkMode = value;
                if (_berserkMode)
                {
                    BerserkModeEvent?.Invoke();
                    _animator.Clear();
                }
            }
        }
        public Action BerserkModeEvent { get; set; }
        
        public Pattern CurPattern => FixPattern ?? Patterns[Turn % Patterns.Length];
        public Pattern PrevPattern;
        public Action OnTurnEvent;

        private EnemyDataSO _enemyData;
        public EnemyDataSO EnemyData => _enemyData;
        
        private EnemyRenderer _renderer;

        public EnemyRenderer Renderer
        {
            get => _renderer;
            set => _renderer = value;
        }
        
        public virtual void Init(EnemyDataSO enemyData) {
            Init(enemyData.maxHP);
    
            Name = enemyData.enemyName;
            UpdatePatternEvent += ExecutePreActionByPattern;

            _enemyData = enemyData;
            Renderer = GetComponent<EnemyRenderer>();
            DieEvent += () => { GameManager.I.ExecuteEnemyCount++; };
            HitEvent += () => _renderer.HitMMF.PlayFeedbacks();
        }

        public override void Init(int maxHp) {
            base.Init(maxHp);
        }

        /// <summary>
        /// 범위 공격 시, 공격할 타일들을 저장하는 리스트 
        /// </summary>
        protected List<Tile> AreaAttackTiles { get; } = new();

        public override IEnumerator StartTurn()
        {
            base.StartTurn();
            UpdatePatternEvent?.Invoke(CurPattern);

            yield return null;
        }

        private void ExecutePreActionByPattern(Pattern pattern)
        {
            switch (pattern?.Type)
            {
                case EnemyActionType.AreaAttack :
                    AreaAttackTiles.Clear();
                    pattern.PreAction(); // [TODO] Action을 통해서 공격 범위를 설정함
                    // [TODO] Board에서 범위 공격 범위 표시 필요
                    break;
            }
        }

        public override IEnumerator EndTurn()
        {
            yield return base.EndTurn();
        }

        public override IEnumerator OnPreTurn()
        {
            ResetDefenseCount();
            yield return new WaitForSeconds(.5f);
        }

        public override IEnumerator OnTurn()
        {
            if (CheckBuffExist(BuffType.Stun))
            {
                PrevPattern = null;
                // 행동 하지 않음... 스턴 효과 애니메이션 출력?
            }
            else
            {
                PrevPattern = CurPattern;
                Pattern curPat = CurPattern;
                int value = curPat.Value ?? 0;
            
                switch (curPat.Type)
                {
                    case EnemyActionType.Attack :
                        Attack(GameManager.I.Player, value);
                        break;
                    case EnemyActionType.Defense :
                        AddDefenseCount(value);
                        break;
                    case EnemyActionType.AreaAttack :
                    case EnemyActionType.TileDebuff :
                    case EnemyActionType.UserDebuff :
                    case EnemyActionType.TileCurse :
                    case EnemyActionType.Spawn :
                        curPat?.Action();
                        break;
                    case EnemyActionType.NoAction :
                    case EnemyActionType.Confusion :
                    case EnemyActionType.Sleep :
                        break;
                    default: break;
                }
                OnTurnEvent?.Invoke();
            }

            // 초기화
            if (FixPattern == null)
            { 
                Turn++;
            }
            else
            {
                _fixPattern = null;
            }
            
            yield return new WaitForSeconds(.5f);
        }

        public override void AddBuff(BaseBuff buff)
        {
            base.AddBuff(buff);
            
            if (buff.Type == BuffType.Burn)
            {
                // [축복] 메테오: 적의 화상 중첩이 10이 되면, 중첩을 0으로 만들고 강력한 메테오를 소환합니다. (20 데미지)
                if (GameManager.I.Player.PlayerInfo.CheckBlessExist(BlessType.BlessFire2))
                {
                    var burnBuff = Buffs.FirstOrDefault(x => x.Type == BuffType.Burn);
                    if (burnBuff != null)
                    {
                        if (burnBuff.Count >= 5)
                        {
                            GameManager.I.Player.PlayerInfo.BlessEventDict[BlessType.BlessFire2]?.Invoke();
                            burnBuff.Count -= 5;
                            GameManager.I.Stage.Meteor();
                        }
                    }
                }
            }
            else if (buff.Type == BuffType.Weak)
            {
                UpdatePatternEvent?.Invoke(CurPattern);
            }
        }
    }
}