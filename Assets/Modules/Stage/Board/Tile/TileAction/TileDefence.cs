using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.Board {
    
    public class TileDefence: TileAction {
        public override void Act(int value, BaseEntity _) {
            FindAnyObjectByType<Player>().DefenseCount += value;
        }

        public override void ArriveAction() { }
    }

}