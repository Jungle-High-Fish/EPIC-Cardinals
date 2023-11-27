using System;
using System.Collections.Generic;
using System.Linq;
using Cardinals.Board;
using Cardinals.Enemy;
using Cardinals.Enums;
using Cardinals.Game;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Cardinals
{
    public abstract class BaseEnemy : BaseEntity
    {
        public string Name { get; private set; }
        
        protected int Turn { get; set; }

        private Sprite _sprite; 
        public Sprite Sprite
        {
            get => _sprite;
            set
            {
                _sprite = value;
                UpdatedSpriteEvent?.Invoke(_sprite);
            }
        }

        private Reward[] _rewards;
        public Reward[] Rewards
        {
            get => _rewards;
            protected set => _rewards = value;
        }

        public Action<Sprite> UpdatedSpriteEvent { get; set; }
        
        protected Pattern[] Patterns { get; set; }

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
                if (_berserkMode) BerserkModeEvent?.Invoke();
            } 
        }
        protected Action BerserkModeEvent { get; set; }
        
        public Pattern CurPattern => FixPattern ?? Patterns[Turn % Patterns.Length];

        public virtual void Init(EnemyDataSO enemyData) {
            Init(enemyData.maxHP);
    
            Name = enemyData.enemyName;
            Sprite = enemyData.sprite;
            UpdatePatternEvent += ExecutePreActionByPattern;
        }

        public override void Init(int maxHp) {
            base.Init(maxHp);
        }

        /// <summary>
        /// 범위 공격 시, 공격할 타일들을 저장하는 리스트 
        /// </summary>
        protected List<Tile> AreaAttackTiles { get; } = new();

        public override void StartTurn()
        {
            base.StartTurn();

            UpdatePatternEvent?.Invoke(CurPattern);

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
        
        public override void OnTurn()
        {
            DefenseCount = 0;

            Pattern curPat = CurPattern;
            int value = curPat.Value ?? 0;
            
            Debug.Log($"적({Name}): {curPat.Type.ToString()} 수행");
            switch (curPat.Type)
            {
                case EnemyActionType.Attack :
                    GameManager.I.Player.Hit(value);
                    break;
                case EnemyActionType.Defense :
                    DefenseCount += value;
                    break;
                case EnemyActionType.AreaAttack :
                case EnemyActionType.TileDebuff :
                case EnemyActionType.UserDebuff :
                case EnemyActionType.TileCurse :
                    curPat.Action();
                    break;
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
            
        }

        public override void AddBuff(BaseBuff buff)
        {
            // [축복] 그을린 상처: 적을 공격 할 때, 화상이 걸린 상태라면 1의 데미지를 추가로 입힘
            if (GameManager.I.Player.PlayerInfo.CheckBlessExist(BlessType.BlessFire1))
            {
                if (buff.Type == BuffType.Burn)
                {
                    if (CheckBuffExist(BuffType.Burn))
                    {
                        Hit(1);
                    }
                }
            }
            
            base.AddBuff(buff);
            
            // [축복] 메테오: 적의 화상 중첩이 10이 되면, 중첩을 0으로 만들고 강력한 메테오를 소환합니다. (20 데미지)
            if (GameManager.I.Player.PlayerInfo.CheckBlessExist(BlessType.BlessFire2))
            {
                if (buff.Type == BuffType.Burn)
                {
                    var burnBuff = Buffs.FirstOrDefault(x => x.Type == BuffType.Burn);
                    if (burnBuff != null)
                    {
                        if (burnBuff.Count >= 10)
                        {
                            burnBuff.Count -= 10;
                            (GameManager.I.Stage.CurEvent as BattleEvent)?.Meteor();
                        }
                    }
                }
            }
        }
    }
}