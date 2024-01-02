using Cardinals;
using Cardinals.Enemy;
using Cardinals.Enums;
using Cardinals.Game;

namespace Cardinals.Enemy
{
    public class Krol : BaseEnemy
    {
        public override void Init(EnemyDataSO enemyData)
        {
            base.Init(enemyData);

            Patterns = new[]
            {
                new Pattern(EnemyActionType.Attack, 3),
                new Pattern(EnemyActionType.Defense, 3),
            };
            
            Rewards = new Reward[]
            {
                new(RewardType.Gold, 50),
            };
        }
    }
}