using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Cardinals.Constants {
    namespace FilePath {
        public static class Resources {
            public const string Path = "Assets/Resources/";

            // Prefab
            public const string Prefabs = "Prefabs/";
            public const string Prefabs_StageGround = "Stage/Ground";

            public const string Prefabs_Tile = "Board/TileUV2";
            public const string Prefabs_CornerTile = "Board/CornerTile";
            
            public const string Prefabs_Stage_RewardBox = "Stage/RewardBox";
            
            public const string Prefabs_UI_Canvas = "UI/Canvas";
            public const string Prefabs_UI_Description = "UI/DescriptionPrefab";
            public const string Prefabs_UI_RewardPanel = "UI/Reward/UIRewardPanel";
            public const string Prefabs_UI_RewardItem = "UI/Reward/UIRewardItem";
            
            public const string Prefabs_UIEnemyInfo = "Enemy/UIEnemyInfo";
            public const string Prefabs_EnemyRenderer = "Enemy/EnemyRenderer";
            public const string Prefabs_Player = "Player/Player";
            public const string Prefabs_UIPlayerInfo = "UI/Player/UIPlayerInfo";

            public const string Prefabs_UI_StageInfo = "UI/Stage/Stage Info UI";
            public const string Prefabs_UI_EventNode = "UI/Stage/EventNode";

            public const string Prefabs_UI_CardSystem = "UI/Card/CardSystemUI";
            public const string Prefabs_UI_Card = "UI/Card/CardUI";

            // Sprite
            public const string Sprites = "Sprites/";

            // Scriptable Object
            public const string SO = "SO/";
            public const string SO_BoardData = "Stage/BoardData/";
            
            public const string Enemy = "Enemy/";
            public const string Enemy_Pattern = "Enemy/Pattern/";

            public static string Get(string path) {
                return Path + path;
            }
        }
    }

    namespace Common {
        public static class InstanceName {
            public const string StageController = "StageContoller";
            
            public const string UI = "UI";
            public const string MainUICanvas = "Main UI Canvas";
            public const string PlayerUICanvas = "Player UI Canvas";
            public const string CardUICanvas = "Card UI Canvas";
            public const string EnemyUICanvas = "Enemy UI Canvas";


            public const string EnemyPlace = "Enemy";
            public const string EnemyUI = "EnemyUI";
            public const string Board = "Board";

            public const string Core = "Core";
            public const string CardManager = "CardManager";
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

        public static class Tile {
            public const int MaxLevel = 3;
            public static readonly List<int> LevelUpExp = new List<int> { 
                3, 7, 15,
            };

            public static readonly List<int> FireMagicMainDamage = new List<int> {
                2, 4, 6,
            };

            public static readonly List<int> WaterMagicMainCure = new List<int> {
                1, 2, 3,
            };

            public static readonly List<int> EarthMagicMainDefense = new List<int> {
                2, 4, 6,
            };
        }
    }
}

