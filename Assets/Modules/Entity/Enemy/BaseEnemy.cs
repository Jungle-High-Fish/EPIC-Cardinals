using System;
using System.Collections.Generic;
using Cardinals.Board;
using Cardinals.Enemy;
using Cardinals.Enums;
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
        
        private Pattern CurPattern => FixPattern ?? Patterns[Turn % Patterns.Length];
        
        protected BaseEnemy(string name,
                            int maxHp   ) : base(maxHp)
        {
            Name = name;
            UpdatePatternEvent += ExecutePreActionByPattern;
        }

        public virtual void Init(string name, int maxHp) {
            base.Init(maxHp);

            Name = name;
            UpdatePatternEvent += ExecutePreActionByPattern;
        }

        public virtual void Init() {
            
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
            Pattern curPat = CurPattern;
            int value = curPat.Value ?? 0;
            
            Debug.Log($"적({Name}): {curPat.Type.ToString()} 수행");
            switch (curPat.Type)
            {
                case EnemyActionType.Attack :
                    GameManager.I.Player.Hit(value);
                    break;
                case EnemyActionType.Defense :
                    GameManager.I.Player.DefenseCount += value;
                    break;
                case EnemyActionType.AreaAttack :
                case EnemyActionType.Magic :
                case EnemyActionType.Buff :
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
    }
}