using System;
using System.Collections;
using UnityEngine;

namespace Cardinals.Game
{
    public class Stage : IEnumerator
    {
        private BaseEvent[] _events;
        public BaseEvent[] Events { get; }
        
        private int _index;
        
        /// <summary>
        /// 신규 이벤트 등록
        /// </summary>
        /// <param name="events"></param>
        public Stage(BaseEvent[] events)
        {
            // 데이타 초기화
            Events = events;
            
            // 초기 값 등록
            GameManager.I.UI.UIStage.Init(this);
        }

        /// <summary>
        /// 기존 데이타가 존재하는 경우
        /// </summary>
        /// <param name="events">이벤트</param>
        /// <param name="index">이전 클리어한 사건의 인덱스</param>
        public Stage(BaseEvent[] events, int index) : this(events)
        {
            for (int i = 0; i < index; i++)
            {
                events[i].IsClear = true;
            }

            _index = index;
        }

        #region IEnumerator
        public bool MoveNext()
        {
            _index++;
            return (_index < _events.Length);
        }

        public void Reset()
        {
            _index = -1;
        }

        public object Current
        {
            get
            {
                try
                {
                    return _events[_index];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
        #endregion
    }
}