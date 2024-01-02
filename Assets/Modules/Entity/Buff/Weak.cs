using Cardinals.Enums;

namespace Cardinals.Buff
{
    public class Weak : BaseBuff
    {
        public Weak(int count) : base(BuffType.Weak, count: count)
        {
        }
    }
}