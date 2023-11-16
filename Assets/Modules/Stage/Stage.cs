using System;
using System.Collections;
using System.Linq;
using Cardinals.Board;
using UnityEngine;

namespace Cardinals.Game
{
    [CreateAssetMenu(fileName = "Stage", menuName = "Cardinals/Stage", order = 0)]
    public class Stage: ScriptableObject, IEnumerator
    {
        public BaseEvent[] Events
        {
            get => _events;
            set => _events = value;
        }

        public string Name => _name;
        public BoardDataSO BoardData => _boardDataSO;

        [SerializeField] private string _name;

        [SerializeField] private BaseEvent[] _events;
        [SerializeField] private BoardDataSO _boardDataSO;
        
        private int Index { get; set; }
        
        /// <summary>
        /// 신규 이벤트 등록
        /// </summary>
        /// <param name="events"></param>
        public Stage(string name, params BaseEvent[] events)
        {
            // 데이타 초기화
            _name = name;
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

        public void Init(int index) {
            for (int i = 0; i < _events.Length; i++)
            {
                _events[i].IsClear = i < index;
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