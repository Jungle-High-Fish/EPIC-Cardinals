using Cardinals;
using Cardinals.Enums;
using Cardinals.Game;

namespace Cardinals.Enemy
{
    public class Four :BaseEnemy
    {
        public override void Init(EnemyDataSO enemyData)
        {
            base.Init(enemyData);
            
            Patterns = new[]
            {
                new Pattern(EnemyActionType.Magic, action: SpawnFireball),
                new Pattern(EnemyActionType.Attack, 5),
                new Pattern(EnemyActionType.Defense, 5),
            };
            
            
            Rewards = new Reward[]
            {
                new(RewardType.Gold, 80),
                new(RewardType.Potion, 1),
            };
        }

        void SpawnFireball()
        {
            
        }
    }
}