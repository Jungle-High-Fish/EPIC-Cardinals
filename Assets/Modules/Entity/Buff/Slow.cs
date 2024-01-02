using Cardinals.Enums;

namespace Cardinals.Buff
{
    public class Slow : BaseBuff
    {
        public Slow() : base(BuffType.Slow, BuffCountDecreaseType.Event)
        {
        }
    }
}