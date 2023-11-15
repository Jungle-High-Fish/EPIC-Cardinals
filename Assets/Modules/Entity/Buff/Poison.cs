using Cardinals.Enums;

namespace Cardinals.Buff
{
    public class Poison : BaseBuff
    {
        public Poison(int count) : base(BuffType.Poison, count: count)
        {
            
        }

        public override void Execute(BaseEntity entity)
        {
            entity.Hit(Count);
        }
    }
}