using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cardinals.Board;
using Cardinals.Enums;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace Cardinals.Game
{
    [CreateAssetMenu(fileName = "Stage", menuName = "Cardinals/Stage", order = 0)]
    public class Stage: ScriptableObject, IEnumerator
    {
        public BaseEvent[] Events => _events;

        public StageEventList[] EventNames => _eventNames;

        public string Name => _name;
        public BoardData BoardData => _boardDataSO;
        public int EventCount => _events.Length;

        [SerializeField] private string _name;
        [SerializeField] private StageEventListSO _eventPool;
        [SerializeField] private StageEventType[] _eventSequence;
        [SerializeField] private BoardData _boardDataSO;

        [ShowInInspector, ReadOnly] private BaseEvent[] _events;
        [ShowInInspector, ReadOnly] private StageEventList[] _eventNames;
        
        public int Index { get; private set; }

        public void Init(int index, StageEventList[] events=null, bool skipTutorial=false) {
            if (events != null) {
                _events = events.ToList().ConvertAll((e) => _eventPool.StageEventBindDataList.Find((d) => d.StageEventName == e).EventData).ToArray();
            } else {
                GenerateEventsFromSequence(skipTutorial);
            }

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

        public void GenerateEventsFromSequence(bool skipTutorial)
        {
            int t = 0, c = 0, e = 0, b = 0, en = 0, bless = 0;

            var tutorialList = GetEventListFromPool(StageEventType.Tutorial);
            var commonList = GetEventListFromPool(StageEventType.BattleCommon, skipTutorial);
            var eliteList = GetEventListFromPool(StageEventType.BattleElite);
            var bossList = GetEventListFromPool(StageEventType.BattleBoss);
            var endingList = GetEventListFromPool(StageEventType.DemoClearEnding);
            var blessList = GetEventListFromPool(StageEventType.Bless);

            StageEventType[] eventSequence;
            if (skipTutorial)
            {
                eventSequence 
                    = _eventSequence.Where((e) => true).ToList()
                    .Select((e) => e == StageEventType.Tutorial ? StageEventType.BattleCommon : e).ToArray();
            } else {
                eventSequence = _eventSequence.Where((e) => true).ToArray();
            }

            _events = new BaseEvent[eventSequence.Length];
            _eventNames = new StageEventList[eventSequence.Length];

            for (int i = 0; i < eventSequence.Length; i++)
            {
                switch (eventSequence[i])
                {
                    case StageEventType.Tutorial:
                        _events[i] = tutorialList[t].EventData;
                        _eventNames[i] = tutorialList[t++].StageEventName;
                        break;
                    case StageEventType.BattleCommon:
                        _events[i] = commonList[c].EventData;
                        _eventNames[i] = commonList[c++].StageEventName;
                        break;
                    case StageEventType.BattleElite:
                        _events[i] = eliteList[e].EventData;
                        _eventNames[i] = eliteList[e++].StageEventName;
                        break;
                    case StageEventType.BattleBoss:
                        _events[i] = bossList[b].EventData;
                        _eventNames[i] = bossList[b++].StageEventName;
                        break;
                    case StageEventType.DemoClearEnding:
                        _events[i] = endingList[en].EventData;
                        _eventNames[i] = endingList[en++].StageEventName;
                        break;
                    case StageEventType.Bless:
                        _events[i] = blessList[bless].EventData;
                        _eventNames[i] = blessList[bless++].StageEventName;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public List<StageEventListSO.StageEventBindData> GetEventListFromPool(StageEventType eventType, bool skipTutorial=false) {
            var entireList = _eventPool.StageEventBindDataList;

            var targetList = entireList.FindAll((e) => e.EventData.Type == eventType);
            var needCount = _eventSequence.Count((e) => e == eventType);

            if (skipTutorial && eventType == StageEventType.BattleCommon) {
                needCount += _eventSequence.Count((e) => e == StageEventType.Tutorial);
            }
            
            Unity.Mathematics.Random random = new Unity.Mathematics.Random();
            random.InitState((uint)((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds());
            var result = targetList.OrderBy(a => random.NextInt()).ToList().GetRange(0, needCount);

            return result;
        }
        #endregion
    }
}