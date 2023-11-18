using Cardinals;
using Cardinals.Enemy;
using Cardinals.Enums;
using Cardinals.Game;

namespace Cardinals.Enemy
{
    public class One : BaseEnemy
    {
        public One(string name, int maxHp) : base("크롤", 10)
        {
            Patterns = new[]
            {
                new Pattern(EnemyActionType.Attack, 3),
                new Pattern(EnemyActionType.Defense, 3),
            };
        }

        public override void Init()
        {
            base.Init("크롤", 10);

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