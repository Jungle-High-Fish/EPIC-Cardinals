using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Cardinals.Constants {
    namespace FilePath {
        public static class Resources {
            public const string Path = "Assets/Resources/";

            public const string Prefabs = "Prefabs/";
            public const string Prefabs_Tile = "Board/Tile";
            public const string Prefabs_CornerTile = "Board/CornerTile";
            public const string Prefabs_UI_Description = "UI/DescriptionPrefab";

            public const string Sprites = "Sprites/";

            public const string SO = "SO/";
            public const string SO_BoardData = "Stage/BoardData/";
            
            
            public const string Enemy = "Enemy/";
            public const string Enemy_Pattern = "Enemy/Pattern/";

            public static string Get(string path) {
                return Path + path;
            }
        }
    }

    namespace GameSetting {
        public static class Board {
            public const float TileWidth = 1;
            public const float TileHeight = 1.5f;
            public const float TileSpace = 0.5f;
            public const float TileInstantiateHeight = 3f;

            public static readonly Vector2 BoardCenter = new Vector2(0, 0); 
        }
    }
}

