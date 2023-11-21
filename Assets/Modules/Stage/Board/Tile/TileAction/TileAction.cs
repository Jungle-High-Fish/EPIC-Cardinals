using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.Board {

    public abstract class TileAction: MonoBehaviour {
        protected Tile _tile;
        
        public virtual void Init(Tile tile) {
            _tile = tile;
        }
        
        public abstract void Act(int value, BaseEntity target);
        public abstract void ArriveAction();
    }

}

