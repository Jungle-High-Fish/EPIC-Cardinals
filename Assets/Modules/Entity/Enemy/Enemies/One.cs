using Cardinals;
using Cardinals.Enemy;
using Cardinals.Enums;

namespace Cardinals.Enemy
{
    public class One : BaseEnemy
    {
        public One(string name, int maxHp) : base(name, maxHp)
        {
            Patterns = new[]
            {
                new Pattern(EnemyActionType.Attack, 3),
                new Pattern(EnemyActionType.Defense, 3),
            };
        }
    }
}