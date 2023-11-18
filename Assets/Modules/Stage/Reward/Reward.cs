using System;
using Cardinals.Enums;

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
        
        public Action DeleteEvent { get; set; }

        public Reward(RewardType type, int value = 0, int count = 0)
        {
            Type = type;
            Value = value;
            Count = count;
        }

        public void Remove()
        {
            DeleteEvent?.Invoke();
        }
    }
}