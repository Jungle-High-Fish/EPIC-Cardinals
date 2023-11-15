using System;
using Cardinals.Enemy;
using Cardinals.Enums;
using UnityEngine;

namespace Cardinals
{
    public abstract class BaseEnemy : BaseEntity
    {
        public string Name { get; }
        
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

        public Action<Sprite> UpdatedSpriteEvent { get; set; }
        
        protected Pattern[] Patterns { get; set; }

        private Pattern _fixPattern;
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
        
        protected bool BerserkMode { get; set; }
        
        private Pattern CurPattern => FixPattern ?? Patterns[Turn % Patterns.Length];
        
        protected BaseEnemy(string name,
                            int hp,
                            int maxHp   ) : base(hp, maxHp)
        {
            Name = name;
        }

        
        public override void StartTurn()
        {
            base.StartTurn();
            UpdatePatternEvent?.Invoke(CurPattern);
        }
        
        public override void OnTurn()
        {
            Pattern curPat = CurPattern;
            Debug.Log($"적({Name}): {curPat.Type.ToString()} 수행");
            switch (curPat.Type)
            {
                case EnemyActionType.Attack :
                    
                    break;
                case EnemyActionType.Defense :
                    
                    break;
                case EnemyActionType.AreaAttack :
                    
                    break;
                default:
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
                FixPattern = null;
            }
        }
    }
}