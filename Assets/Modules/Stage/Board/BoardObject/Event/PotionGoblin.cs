using System.Collections;
using UnityEngine;

namespace Cardinals.BoardEvent.Event
{
    public class PotionGoblin : BaseBoardEventObject
    {
        protected override IEnumerator Execute()
        {
            var potion =  GameManager.I.Stage.AddRandomPotion();
            Debug.Log($"고블린이 {potion}을 떨구고 사라졌습니다.");
            yield return null;
        }
    }
}