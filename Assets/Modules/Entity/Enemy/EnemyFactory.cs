using System;
using Cardinals;
using Cardinals.Enemy;
using Cardinals.Enums;

namespace Cardinals.Enemy
{
    public class EnemyFactory
    {
        public static Type GetEnemy(EnemyType type)
        {
            switch (type) {
                case EnemyType.One:
                    return typeof(One);
                case EnemyType.Two:
                    return typeof(Two);
                case EnemyType.Three1:
                    return typeof(Three1);
                case EnemyType.Three2:
                    return typeof(Three2);
                case EnemyType.Four:
                    return typeof(Four);
                case EnemyType.Boss:
                    return typeof(Boss);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}