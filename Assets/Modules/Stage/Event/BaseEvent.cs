using System;

namespace Cardinals.Game
{
    public abstract class BaseEvent : IDisposable
    {
        protected BaseEvent(EventType type)
        {
            Type = type;
            IsClear = false;
        }
        
        public UIEvent UIEvent { protected get; set; }
        public EventType Type { get; }
        
        private bool _isClear;

        public bool IsClear
        {
            get => _isClear;
            set
            {
                _isClear = value;
                if(_isClear) UIEvent.Clear();
            }
        }

        public void On()
        {
            UIEvent.On();
        }

        public void Dispose()
        {
            UIEvent = null;
        }
    }
}