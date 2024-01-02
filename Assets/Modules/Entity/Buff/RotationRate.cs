using Cardinals;
using Cardinals.Enemy;
using Cardinals.Enums;
using UnityEngine.Rendering;

namespace Modules.Entity.Buff
{
    /// <summary>
    /// 현재 8번 몬스터 대상으로 코드가 작성됨
    /// </summary>
    public class RotationRate : BaseBuff
    {
        private BaseEntity _target;
        private int _damage;
        
        public RotationRate (BaseEntity target, int damage) : base(BuffType.RotationRate, BuffCountDecreaseType.Event)
        {
            _target = target;
            _damage = damage;
            
            GameManager.I.Player.HomeReturnEvent += RotationRateHit;
            
            // 이벤트 구독 (죽으면 이벤트 해지)
            _target.DieEvent += () =>
            {
                GameManager.I.Player.HomeReturnEvent -= RotationRateHit;
            };
        }

        private void RotationRateHit()
        {
            if ( _target is DolDol)
            {
                (_target as DolDol).EffectiveHit(_damage);
            }
        }
    }
}