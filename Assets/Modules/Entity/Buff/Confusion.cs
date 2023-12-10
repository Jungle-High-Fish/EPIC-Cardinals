using Cardinals;
using Cardinals.Enums;

namespace Modules.Entity.Buff
{
    public class Confusion : BaseBuff
    {
        public Confusion() : base(BuffType.Confusion, BuffCountDecreaseType.Event)
        {
        }
    }
}