using System;
using Cardinals;
using Cardinals.Enums;

namespace Modules.Enemy
{
    public class EnemyFactory
    {
        public static BaseEnemy GetEnemy(EnemyType type)
        {
            return type switch
            {
                EnemyType.One => new One("첫번째몬스터", 10),
                EnemyType.Boss => new One("보스", 10),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}