using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using UnityEngine;

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

        private TileData _tileData;

        private Tile _next;
        private Tile _prev;
        
        public void Init(TileData tileData) {
            _tileData = tileData;
        }
    }

}
