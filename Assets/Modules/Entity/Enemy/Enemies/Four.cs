using Cardinals;
using Cardinals.Enums;

namespace Cardinals.Enemy
{
    public class Four :BaseEnemy
    {
        public Four(string name, int maxHp) : base(name, maxHp)
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
    }
}