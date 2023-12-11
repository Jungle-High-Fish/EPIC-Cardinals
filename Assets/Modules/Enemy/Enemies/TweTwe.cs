using Cardinals;
using Cardinals.Buff;
using Cardinals.Enums;
using Cardinals.Game;

namespace Cardinals.Enemy
{
    public class TweTwe : BaseEnemy {
        public override void Init(EnemyDataSO enemyData)
        {
            base.Init(enemyData);

            FixPattern = new Pattern(EnemyActionType.UserDebuff, action: Slow);
            Patterns = new[]
            {
                new Pattern(EnemyActionType.Defense, 5),
                new Pattern(EnemyActionType.Attack, 5),
            };
            
            Rewards = new Reward[]
            {
                new(RewardType.Gold, 60),
                new(RewardType.Potion, 1)
            };
        }

        private void Slow()
        {
            GameManager.I.Player.AddBuff(new Slow());
        }
    }
}