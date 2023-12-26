using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.Game
{
    [CreateAssetMenu(fileName = "BlessEvent", menuName = "Cardinals/Event/Bless")]
    public class BlessEvent : BaseEvent
    {
        public override IEnumerator Flow(StageController stageController)
        {
            yield return GameManager.I.Stage.SelectBlessFlow();
            IsClear = true;
        }
    }
}