using System.Collections;
using System.Collections.Generic;
using Cardinals.Game;
using UnityEngine;

namespace Cardinals.Board {
    
    public class TileAttack: TileAction {
        public override void Act(int value, BaseEntity target) {
            if (target == null) {
                Debug.LogError("공격  타겟이 없습니다.");
            }
            target.Hit(value);
        }

        public override void ArriveAction() {
            throw new System.NotImplementedException();
        }
    }
    
}