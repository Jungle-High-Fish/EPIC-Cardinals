using Cardinals.Enums;
using UnityEngine;

namespace Cardinals.BoardEvent
{
    [CreateAssetMenu(fileName = "NewBoardEventData", menuName = "Cardinals/NewBoardEventData", order = 1)]
    public class NewBoardEventDataSO : ScriptableObject
    {
        public NewBoardEventType type;
        
        public string eventName;
        
        [TextArea] public string description;

        [Tooltip("보드 위에 유지 되는 턴 수")] public int keepTurnCount;
        
        public Sprite sprite;
        
        public NewBoardEventOnType onBoardType;

        public NewBoardEventExecuteType executeType;
        
        [Tooltip("on Board Type이 Move일 때, 매 턴 이동하는 거리")] public int moveCount;
    }
}