using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif

namespace Cardinals.Board {

    [CreateAssetMenu(fileName = "CircleBoardData", menuName = "Cardinals/Circle Board Data", order = 0)]
    public class CircleBoardDataSO: BoardData {
        public const int DefaultTileCount = 16;

        [LabelText("보드 이름")]
        public string boardName = "New Board";
        
        [LabelText("타일 갯수"), Range(5, 30)]
        public int tileCount = DefaultTileCount;
        [LabelText("보드 반지름"), Range(1f, 10f)]
        public float radius = 3f;
        [LabelText("시작 타일 인덱스")]
        public int startTileIndex = 0;
        [LabelText("보드 타일 분할 갯수")]
        public int edgeCount;
    }

}

