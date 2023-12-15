using System;
using System.Collections.Generic;
using Cardinals.Enums;
using UnityEngine;

namespace Cardinals.Game {
    [CreateAssetMenu(fileName = "StageEventListSO", menuName = "Cardinals/StageEventListSO")]
    public class StageEventListSO : ScriptableObject {
        public List<StageEventBindData> StageEventBindDataList;
        
        [Serializable]
        public class StageEventBindData {
            public StageEventList StageEventName;
            public BaseEvent EventData;
        }
    }
}