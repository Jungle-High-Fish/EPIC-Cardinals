using Cardinals;
using Cardinals.Enums;
using UnityEngine.Rendering;

namespace Modules.Entity.Buff
{
    public class RotationRate : BaseBuff
    {
        public RotationRate(BaseEntity target, int damage) : base(BuffType.RotationRate, BuffCountDecreaseType.Event)
        {
            // 
            // 이벤트 구독 (죽으면 이벤트 해지)
            target.DieEvent += () =>
            {

            };
        }
    }
}