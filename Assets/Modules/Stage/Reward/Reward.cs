using System;
using Cardinals.Enums;
using Cardinals.UI;

namespace Cardinals.Game
{
    public class Reward
    {
        public RewardType Type { get; set; }
        
        /// <summary>
        /// Type에 따라 다르게 사용됨
        /// Gold: 금액, Potion, Card, Artifact: 값 (Reward Box에서 구체화 됨)
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Type에 따라 다르게 사용됨
        /// Gold: -, Potion, Card: 갯수, Artifact: 선택지
        /// </summary>
        public int Count { get; set; }
        
        public object Data { get; set; }
        
        public UIRewardItem UI { get; set; }

        public Reward(RewardType type, int value = 0, int count = 0, object data = null)
        {
            Type = type;
            Value = value;
            Count = count;
            Data = data;
        }

        public void Remove()
        {
            UI.Remove();
        }
    }
}