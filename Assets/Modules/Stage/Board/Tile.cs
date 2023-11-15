using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using UnityEngine;
using Util;

namespace Cardinals.Board {

    public class Tile : MonoBehaviour {
        public TileType Type => _tileData.type;
        public TileDirection Direction => _tileData.direction;

        public Tile Next {
            get => _next;
            set {
                _next = value;

                if (_next != null) {
                    _next._prev = this;
                }
            }
        }

        public Tile Prev {
            get => _prev;
            set {
                _prev = value;

                if (_prev != null) {
                    _prev._next = this;
                }
            }
        }

        public TileAnimation Animation => _tileAnimation.Get(gameObject);

        private TileData _tileData;
        private ComponentGetter<TileAnimation> _tileAnimation
            = new ComponentGetter<TileAnimation>(TypeOfGetter.This);

        private Tile _next;
        private Tile _prev;
        
        public void Init(TileData tileData) {
            _tileData = tileData;
        }
    }

}
