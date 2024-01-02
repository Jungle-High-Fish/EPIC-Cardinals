using System.Collections;
using System.Collections.Generic;
using Cardinals.Board;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    public class UITileMaterial: MonoBehaviour {
        private ComponentGetter<MeshRenderer> _tileMeshRenderer 
            = new ComponentGetter<MeshRenderer>(TypeOfGetter.This);

        private Tile _tile;
        
        public void Set(Tile tile) {
            _tile = tile;
            
            _tileMeshRenderer.Get(gameObject).material
                = TileMagic.Data(_tile.TileMagic.Type).tileMaterial;
        }
    }
}
