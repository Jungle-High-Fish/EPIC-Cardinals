using System;
using Cardinals.Enums;

namespace Cardinals
{
    public abstract class BaseBuff
    {
        protected BaseBuff(BuffType type,
                           BuffCountDecreaseType countDecreaseType = BuffCountDecreaseType.Turn,
                           int count = 0)
        {
            Type = type;
            CountDecreaseType = countDecreaseType;
            Count = count;
        }
        
        public string Name { get; }
        public string Description { get; }
        
        public BuffType Type { get; }
        public BuffCountDecreaseType CountDecreaseType { get; }

        private int _count;

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                if ( CountDecreaseType == BuffCountDecreaseType.Turn 
                     && _count == 0)
                {
                    Remove();
                }
                else UpdateBuffCountEvent?.Invoke(_count);
            }
        }

        public Action<int> UpdateBuffCountEvent { get; set; }
        
        protected int Value { get; set; }

        public Action RemoveEvent { get; set; }

        public virtual void Execute(BaseEntity entity)
        {
            
        }
        
        public void EndTurn()
        {
            if (CountDecreaseType <= BuffCountDecreaseType.Turn)
            {
                Count--;
            }
        }
        
        public void EndEvent()
        {
            if (CountDecreaseType <= BuffCountDecreaseType.Event)
            {
                Remove();
            }
        }

        public void EndStage()
        {
            if (CountDecreaseType <= BuffCountDecreaseType.Stage)
            {
                Remove();
            }
        }

        /// <summary>
        /// 해당 버프가 제거될 때 호출됨
        /// </summary>
        public void Remove()
        {
            RemoveEvent?.Invoke();
        }
    }
}