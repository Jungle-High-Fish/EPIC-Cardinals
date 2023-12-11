using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.UI;
using UnityEngine;
using UnityEngine.UI;
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

        ComponentGetter<Image> _sealedImage = new ComponentGetter<Image>(
            TypeOfGetter.ChildByName, 
            "Normal Tile UI/Sealed Image"
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

        private void Update() {
            if (_tile == null) return;

            if (_tile.IsSealed) {
                _sealedImage.Get(gameObject).gameObject.SetActive(true);
            }
            else {
                _sealedImage.Get(gameObject).gameObject.SetActive(false);
            }
        }
    }
}
