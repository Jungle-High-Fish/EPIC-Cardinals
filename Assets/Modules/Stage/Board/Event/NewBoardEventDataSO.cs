using Cardinals.Enums;
using UnityEngine;

namespace Cardinals.BoardEvent
{
    [CreateAssetMenu(fileName = "NewBoardEventData", menuName = "Cardinals/NewBoardEventData", order = 1)]
    public class NewBoardEventDataSO : ScriptableObject
    {
        public string name;
        
        [TextArea] public string description;

        [Tooltip("보드 위에 유지 되는 턴 수")] public int keepTurnCount;
        
        public Sprite sprite;
        
        public NewBoardEventOnType onBoardType;
        
        [Tooltip("on Board Type이 Move일 때, 매 턴 이동하는 거리")] public int moveDistance;
    }
}