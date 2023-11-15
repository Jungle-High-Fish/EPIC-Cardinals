using Cardinals;
using Cardinals.Enemy;
using Cardinals.Enums;

namespace Modules.Enemy
{
    public class One : BaseEnemy
    {
        public One(string name, int hp, int maxHp) : base(name, hp, maxHp)
        {
            Patterns = new[]
            {
                new Pattern(EnemyActionType.Attack, 3),
                new Pattern(EnemyActionType.Defense, 3),
            };
        }
    }
}