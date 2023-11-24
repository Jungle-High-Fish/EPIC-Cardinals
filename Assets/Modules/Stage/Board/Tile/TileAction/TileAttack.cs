using System.Collections;
using System.Collections.Generic;
using Cardinals.Game;
using UnityEngine;

namespace Cardinals.Board {
    
    public class TileAttack: TileAction {
        public override void Act(int value, BaseEntity target) {
            if (target == null) {
                Debug.LogError("공격  타겟이 없습니다.");
                return;
            }
            
            GameManager.I.Player.Attack(target, value);
        }

        public override void ArriveAction() {
            
        }
    }
    
}