using Cardinals.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cardinals.BoardEvent
{
    [CreateAssetMenu(fileName = "NewBoardEventData", menuName = "Cardinals/NewBoardEventData", order = 1)]
    public class NewBoardObjectDataSO : ScriptableObject
    {
        [Header("Type")]
        public NewBoardEventType evtType;
        public NewBoardObjectType objType;
        
        [Header("Default Info")]
        public string eventName;
        
        [TextArea] public string description;

        [Tooltip("보드 위에 유지 되는 턴 수")] public int keepTurnCount;
        
        public Sprite sprite;
        
        public NewBoardEventOnType onBoardType;

        public NewBoardEventExecuteType executeType;
        
        [Tooltip("on Board Type이 Move일 때, 매 턴 이동하는 거리")] public int moveCount;
        
        [Header("Special")]
        public Sprite spec_sprite_1;
        public Sprite spec_sprite_2;
        public Sprite spec_sprite_3;
    }
}