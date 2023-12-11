using System.Linq;
using Cardinals;
using Cardinals.Enums;

namespace Modules.Entity.Buff
{
    public class Doll : BaseBuff
    {
        public Doll() : base(BuffType.Doll, BuffCountDecreaseType.Event)
        {
            
        }

        public static bool CheckDoll()
        {
            return GameManager.I.Stage.Summons.Any(x=> x is Cardinals.Enemy.Summon.Doll);
        }
    }
}