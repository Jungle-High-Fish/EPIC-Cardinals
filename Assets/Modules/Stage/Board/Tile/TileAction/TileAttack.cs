using System.Collections;
using System.Collections.Generic;
using Cardinals.Game;
using UnityEngine;

namespace Cardinals.Board {
    
    public class TileAttack: TileAction {
        public override void Act(int value) {
            //throw new System.NotImplementedException();
            Debug.Log(value);
            (GameManager.I.CurStage.CurEvent as BattleEvent).Enemies[0].Hit(value);
        }

        public override void ArriveAction() {
            throw new System.NotImplementedException();
        }
    }
    
}