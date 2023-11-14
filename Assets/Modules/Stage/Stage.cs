using System;
using System.Collections;
using UnityEngine;

namespace Cardinals.Game
{
    public class Stage : IEnumerator
    {
        private BaseEvent[] _events;
        public BaseEvent[] Events { get; }
        
        private int Index { get; set; }
        
        public string Name { get; }
        
        /// <summary>
        /// 신규 이벤트 등록
        /// </summary>
        /// <param name="events"></param>
        public Stage(string name, BaseEvent[] events)
        {
            // 데이타 초기화
            Name = name;
            Events = events;
            
            // 초기 값 등록
            GameManager.I.UI.UIStage.Init(this);
        }

        /// <summary>
        /// 기존 데이타가 존재하는 경우
        /// </summary>
        /// <param name="events">이벤트</param>
        /// <param name="index">이전 클리어한 사건의 인덱스</param>
        public Stage(string name, BaseEvent[] events, int index) : this(name, events)
        {
            for (int i = 0; i < index; i++)
            {
                events[i].IsClear = true;
            }

            Index = index;
        }

        #region IEnumerator
        public bool MoveNext()
        {
            Index++;
            return (Index < _events.Length);
        }

        public void Reset()
        {
            Index = -1;
        }

        public object Current
        {
            get
            {
                try
                {
                    return _events[Index];
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