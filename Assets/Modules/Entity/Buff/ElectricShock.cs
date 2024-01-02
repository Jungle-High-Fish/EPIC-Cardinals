using Cardinals.Enums;

namespace Cardinals.Buff
{
    public class ElectricShock : BaseBuff
    {
        public ElectricShock() : base(BuffType.ElectricShock,BuffCountDecreaseType.Event)
        {
            
        }
    }
}