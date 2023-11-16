using System;
using Cardinals;
using Cardinals.Enemy;
using Cardinals.Enums;

namespace Cardinals.Enemy
{
    public class EnemyFactory
    {
        public static Type GetEnemy(EnemyType type, out (string name, int hp) enemyData)
        {
            switch (type) {
                case EnemyType.One:
                    enemyData.name = "크롤";
                    enemyData.hp = 10;
                    return typeof(One);
                case EnemyType.Two:
                    enemyData.name = "퉤퉤기";
                    enemyData.hp = 10;
                    return typeof(Two);
                case EnemyType.Three1:
                    enemyData.name = "삐삐";
                    enemyData.hp = 15;
                    return typeof(Three1);
                case EnemyType.Three2:
                    enemyData.name = "뽀뽀";
                    enemyData.hp = 15;
                    return typeof(Three2);
                case EnemyType.Four:
                    enemyData.name = "삒삒이";
                    enemyData.hp = 30;
                    return typeof(Four);
                case EnemyType.Boss:
                    enemyData.name = "파지지지직이";
                    enemyData.hp = 40;
                    return typeof(Boss);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}