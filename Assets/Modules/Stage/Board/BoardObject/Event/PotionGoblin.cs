using Cardinals.Enums;
using Cardinals.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.BoardObject.Event
{
    public class PotionGoblin : BaseBoardObject
    {
        protected override IEnumerator Execute()
        {
            yield return GameManager.I.UI.UIRewardPanel.GetRandomPotionEvent();
        }
    }
}