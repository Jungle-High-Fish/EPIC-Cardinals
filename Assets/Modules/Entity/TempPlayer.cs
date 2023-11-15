using UnityEngine;

namespace Cardinals
{
    public class TempPlayer : BaseEntity
    {
        public TempPlayer() : base(40, 40)
        {
            
        }

        public override void OnTurn()
        {
            Debug.Log("[임시] 플레이어 턴 수행");
        }
    }
}