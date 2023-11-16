using System;
using Cardinals;
using Cardinals.Enemy;
using Cardinals.Enums;

namespace Modules.Enemy
{
    public class EnemyFactory
    {
        public static BaseEnemy GetEnemy(EnemyType type)
        {
            return type switch
            {
                EnemyType.One => new One("크롤", 10),
                EnemyType.Two => new Two("퉤퉤기", 10),
                EnemyType.Three1 => new Three1("삐삐", 15),
                EnemyType.Three2 => new Three2("뽀뽀", 15),
                EnemyType.Four => new Four("삒삒이", 30),
                EnemyType.Boss => new Boss("파지지지직이", 40),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}