using System;

namespace Cardinals
{
    public abstract class BaseEntity
    {
        private int _hp;

        protected int Hp
        {
            get => _hp;
            set
            {
                _hp = Math.Min(Math.Max(0, value), _maxHp);
                if (_hp == 0)
                {
                    Die?.Invoke();
                }
            }
        }

        private int _maxHp;

        public Action Die { get; set; }
        
        
        public BaseEntity()
        {
            
        }

        public abstract void StartTurn();
        
        public abstract void OnTurn();

        public virtual void EndTurn()
        {
            // 버프/디버프 소모
            // 버프/디버프 존재 시 감소
        }
        
    }
}