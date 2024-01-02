using Cardinals;
using Cardinals.Enums;

namespace Modules.Entity.Buff
{
    public class Stun : BaseBuff
    {
        public Stun() : base(BuffType.Stun, BuffCountDecreaseType.Once)
        {
        }
    }
}