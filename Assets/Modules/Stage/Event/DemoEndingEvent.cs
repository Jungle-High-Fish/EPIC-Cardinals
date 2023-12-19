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

            yield return new WaitUntil(() => false);
        }
    }
}
