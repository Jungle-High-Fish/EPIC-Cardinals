using Cardinals.Enums;

namespace Cardinals.Buff
{
    public class Burn : BaseBuff
    {
        public Burn(int count, int value = 1) : base(BuffType.Burn, count: count)
        {
            Value = value;
        }
        
        public override void Execute(BaseEntity entity)
        {
            entity.Hit(Value);
            base.Execute(entity);
        }
    }
}