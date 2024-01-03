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
            set => _isClear = value;
        }

        /// <summary>
        /// 이벤트 플로우 시작 전 실행
        /// </summary>
        public IEnumerator On(bool isStartEvent, Action onComplete=null)
        {
            yield return UIEvent.On(isStartEvent);
            onComplete?.Invoke();
        }

        public void ImmediateOn(bool isStartEvent)
        {
            UIEvent.ImmediateOn(isStartEvent);
        }

        public void Dispose()
        {
            UIEvent = null;
        }

        public abstract IEnumerator Flow(StageController stageController);
        
        public bool IsSelect { protected get; set; }
    }
}