using Cardinals;
using Cardinals.Enums;
using Cardinals.Game;

namespace Cardinals.Enemy
{
    public class Four :BaseEnemy
    {
        public Four(string name, int maxHp) : base("삒삒이", 30)
        {
            Patterns = new[]
            {
                new Pattern(EnemyActionType.Magic, action: SpawnFireball),
                new Pattern(EnemyActionType.Attack, 5),
                new Pattern(EnemyActionType.Defense, 5),
            };
        }

        void SpawnFireball()
        {
            
        }

        public override void Init()
        {
            base.Init("삒삒이", 30);
            
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
    }
}