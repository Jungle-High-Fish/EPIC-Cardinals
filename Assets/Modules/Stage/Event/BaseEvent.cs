using System;
using System.Collections;
using Cardinals.Enums;
using UnityEngine;

namespace Cardinals.Game
{
    public abstract class BaseEvent: ScriptableObject, IDisposable
    {   
        public UIEvent UIEvent { protected get; set; }
        public StageEventType Type => _type;

        [SerializeField] private StageEventType _type;
        
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

        public abstract IEnumerator Flow(StageController stageController);
    }
}