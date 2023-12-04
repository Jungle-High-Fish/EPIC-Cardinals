using System;
using Cardinals.Enums;
using Util;

namespace Cardinals
{
    public abstract class BaseBuff
    {
        public BuffDataSO Data { get; private set; }
        protected BaseBuff(BuffType type,
                           BuffCountDecreaseType countDecreaseType = BuffCountDecreaseType.Turn,
                           int count = 0)
        {
            Type = type;
            CountDecreaseType = countDecreaseType;
            Count = count;

            Data = ResourceLoader.LoadSO<BuffDataSO>(Constants.FilePath.Resources.SO_BuffData + Type);
        }
        
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
        
        public Action<BaseBuff> ExecuteEvent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Execute(BaseEntity entity)
        {
            ExecuteEvent?.Invoke(this);
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