using Cardinals.Enemy.Summon;
using Cardinals.Enums;
using Cardinals.Game;

namespace Cardinals.Enemy
{
    public class temp_Ten : BaseEnemy
    {
        public override void Init(EnemyDataSO enemyData)
        {
            base.Init(enemyData);

            Patterns = new[]
            {
                new Pattern(EnemyActionType.Attack, 10),
                new Pattern(EnemyActionType.Defense, 7),
            };
            
            Rewards = new Reward[]
            {
                new(RewardType.Gold, 100),
            };
        }
    }
}