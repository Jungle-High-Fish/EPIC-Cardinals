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
                case EnemyType.Krol:
                    return typeof(Krol);
                case EnemyType.TweTwe:
                    return typeof(TweTwe);
                case EnemyType.PiPi:
                    return typeof(PiPi);
                case EnemyType.PoPo:
                    return typeof(PoPo);
                case EnemyType.PicPic:
                    return typeof(PicPic);
                case EnemyType.Pazizizizic:
                    return typeof(Pazizizizic);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}