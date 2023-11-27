using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.UI;
using UnityEngine;
using Util;

namespace Cardinals.Board {
    public class UITile: MonoBehaviour {
        ComponentGetter<UITileExp> _levelUI = new ComponentGetter<UITileExp>(
            TypeOfGetter.ChildByName, 
            "Normal Tile UI/Level Guage"
        );

        ComponentGetter<UITileCurse> _curseUI = new ComponentGetter<UITileCurse>(
            TypeOfGetter.ChildByName, 
            "Tile Curse UI/"
        );

        ComponentGetter<UITileMaterial> _materialUI = new ComponentGetter<UITileMaterial>(
            TypeOfGetter.ChildByName, 
            "Cube"
        );

        private Tile _tile;

        public void Init(Tile tile) {
            _tile = tile;

            _levelUI.Get(gameObject).Init(tile);
            _curseUI.Get(gameObject).Init(tile);
            _materialUI.Get(gameObject).Set(tile);
        }

        public void SetMaterial() {
            _materialUI.Get(gameObject).Set(_tile);
        }
    }
}
