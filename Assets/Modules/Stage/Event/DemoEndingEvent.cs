using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using UnityEngine;

namespace Cardinals.Game
{
    [CreateAssetMenu(fileName = "DemoEndingEvent", menuName = "Cardinals/Event/DemoEnding")]
    public class DemoEndingEvent : BaseEvent
    {
        public override IEnumerator Flow(StageController stageController)
        {
            List<Reward> rewards = new() { new Reward(RewardType.NextStageMap) };
            GameManager.I.Stage.RewardBox.Set(rewards);

            // IsSelect = false;
            // IsClear = true;
            // GameManager.I.UI.UIEndTurnButton.Activate(true);
            // yield return new WaitUntil(() => IsSelect);
            // Destroy(GameManager.I.Stage.RewardBox.gameObject);

            GameManager.I.UI.UIEndTurnButton.Activate(false);
            yield return new WaitUntil(() => false);
        }
    }
}
