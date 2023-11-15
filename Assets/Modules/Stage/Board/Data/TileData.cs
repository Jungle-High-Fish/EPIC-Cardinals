using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using UnityEngine;

namespace Cardinals.Board {

    [System.Serializable]
    public class TileData
    {
        public TileType type;
        public TileMagicType magicType;
        public TileDirection direction;
        public int level;
        public int guage;
    }

}