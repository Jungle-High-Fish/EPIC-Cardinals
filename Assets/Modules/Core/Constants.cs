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

            public const string Prefabs_SoundManager = "SoundManager";

            public const string Prefabs_Tile = "Board/Tiles/TileUV2";
            public const string Prefabs_UI_HoveredTileInfo = "UI/TileInfo/HoveredTileInfoUI";
            public const string Prefabs_UI_TileInfo = "UI/TileInfo/TileInfoUI";
            public const string Prefabs_UI_TileDescription = "UI/TileInfo/TileDescriptionUI";
            public const string Prefabs_UI_OnTileDescription = "UI/TileInfo/OnTileDescriptionUI";

            public const string Prefabs_CornerTile = "Board/Tiles/CornerTile";
            public const string Prefabs_MouseDetector = "Board/MouseDetector";
            
            public const string Prefabs_Stage_RewardBox = "Stage/RewardBox";
            public const string Prefabs_Stage_Totem = "Stage/Totem";
            
            public const string Prefabs_UI_Canvas = "UI/Canvas";

            public const string Prefabs_UI_TurnEndButton = "UI/TurnEndButton";
            public const string Prefabs_UI_MapButton = "UI/MapButton";
            
            public const string Prefabs_UI_BoardEvent_Shop = "UI/BoardEvent/UIShopPanel";
            public const string Prefabs_UI_BoardEvent_Product = "UI/BoardEvent/UIProduct";
            public const string Prefabs_UI_BoardEvent_Card = "UI/BoardEvent/UICardEvent";
            public const string Prefabs_UI_BoardEvent_Roulette = "UI/BoardEvent/UIRoulette";
            public const string Prefabs_UI_BoardEvent_Tile = "UI/BoardEvent/UITileEvent";
            public const string Prefabs_UI_BoardEvent_TypeItem = "UI/BoardEvent/UITypeItem";
            
            public const string Prefabs_UI_Description = "UI/DescriptionPrefab";
            public const string Prefabs_UI_RewardPanel = "UI/Reward/UIRewardPanel";
            public const string Prefabs_UI_RewardItem = "UI/Reward/UIRewardItem";

            public const string Prefabs_UI_Entity_AddBuffDescription = "UI/Entity/AddBuffDescription";
            public const string Prefabs_UI_Entity_ExecuteBuffPrefab = "UI/Entity/ExecuteBuffPrefab";
            public const string Prefabs_UIEnemyStatus = "UI/Enemy/UIEnemyStatus";
            public const string Prefabs_EnemyRenderer = "Enemy/EnemyRenderer";
            public const string Prefabs_Player = "Player/Player";
            public const string Prefabs_UIPlayerInfo = "UI/Player/UIPlayerInfo";
            public const string Prefabs_UINewPlayerInfo = "UI/Player/UINewPlayerInfo";
            public const string Prefabs_UIPlayerStatus = "UI/Player/UIPlayerStatus";
            public const string Prefabs_UIPotion = "UI/Player/UIPotion";
            public const string Prefabs_UIArtifact = "UI/Player/UIArtifact";
            public const string Prefabs_UIBless = "UI/Player/UIBless";
            
            public const string Prefabs_UI_Stage_Map = "UI/Stage/TEMP_StageMap";
            

            public const string Prefabs_UI_PlayerDetail = "UI/Player/Player Detail Info/Detail Info Panel";
            public const string Prefabs_UI_PlayerDetail_Potion = "UI/Player/Player Detail Info/Potion Slot";
            public const string Prefabs_UI_PlayerDetail_Artifact = "UI/Player/Player Detail Info/Artifact Slot";
            public const string Prefabs_UI_PlayerDetail_Bless = "UI/Player/Player Detail Info/Bless Info Panel";

            public const string Prefabs_UI_StageInfo = "UI/Stage/Stage Info UI";
            public const string Prefabs_UI_EventNode = "UI/Stage/EventNode";

            public const string Prefabs_UI_CardSystem = "UI/Card/CardSystemUI";
            public const string Prefabs_UI_Card = "UI/Card/CardUI";

            public const string Prefabs_UI_Dice = "UI/Dice/DiceUI";

            public const string Prefabs_UI_TileSelection = "UI/Stage/Tile Selection UI";

            public const string Prefabs_UI_MagicLevelUpPanel = "UI/Stage/Magic Level Up/Magic Level Up Panel";
            public const string Prefabs_UI_MagicSlot = "UI/Stage/Magic Level Up/Magic Slot";

            public const string Prefabs_Player_Skill_Meteor = "Player/Skill/Player_Skill_Meteor";

            public const string Prefabs_Enemy_Spawn_Summon = "Enemy/Spawn/Summon";
            public const string Prefabs_Enemy_Spawn_Fireball = "Enemy/Spawn/Fireball";
            public const string Prefabs_Enemy_Spawn_ThunderBolt= "Enemy/Spawn/ThunderBolt";

            public const string Prefabs_UI_Tutorial = "UI/Tutorial/TutorialUI";
            public const string Prefabs_UI_TutorialQuest = "UI/Tutorial/QuestUI";

            public const string Prefabs_UI_MouseHint = "UI/MouseHint";

            // Sprite
            public const string Sprites = "Sprites/";
            public const string Sprites_Potion = "Player/Potion/";
            public const string Sprites_BoardEvent = "BoardEvent/";
            public const string Sprites_Artifact = "Player/Artifact/";
            public const string Sprites_Card_Basic1 = "Card/CardBasic1";
            public const string Sprites_Card_Basic2 = "Card/CardBasic2";
            public const string Sprites_Card_Basic3 = "Card/CardBasic3";
            public const string Sprites_Card_Basic4 = "Card/CardBasic4";
            public const string Sprites_Card_Basic5 = "Card/CardBasic5";
            public const string Sprites_Card_Attack_Mark = "Card/Card_State_Attack";
            public const string Sprites_Card_Defense_Mark = "Card/Card_State_Defense";
            
            public const string Sprite_UI_Entity_SuccessDefense = "Entity/SuccessDefense";
            public const string Sprite_UI_Entity_BrokenDefense = "Entity/BrokenDefense";

            public const string Sprites_Player_Action = "Player/Action/";
            
            public const string Sprites_UI_Coin = "UI/Icons/UI_Coin";
            public const string Sprites_UI_IconPrefab = "UI/IconPrefab";
            
            public const string Sprites_UI_Enemy_Ballon_LeftTail = "UI/Enemy/UI_Ballon_LeftTail";
            public const string Sprites_UI_Enemy_Ballon_RightTail = "UI/Enemy/UI_Ballon_RightTail";
            
            public const string Sprites_Enemy_Sprite = "Sprites/Enemy/Image";

            public const string Sprites_Enemy_Skill_Fireball = "Sprites/Enemy/Skill/Fireball";
            public const string Sprites_Enemy_Skill_ThunderBolt = "Sprites/Enemy/Skill/ThunderBolt";

            public const string Sprites_BoardInput_Square = "BoardInputHandler/selectSquare";
            public const string Sprites_BoardInput_Triangle = "BoardInputHandler/selectTriangle";

            // Material
            public const string Materials = "Materials/";
            public const string Materials_BoardInputHandler = "Board/BoardInputHandler/BoardInputHandlerTexture";

            // Scriptable Object
            public const string SO = "SO/";
            public const string SO_BoardData = "Stage/BoardData/";
            public const string SO_BlessData = "Bless/";
            public const string SO_ArtifactData = "Artifacts/";
            public const string SO_PotionData = "Potions/";
            public const string SO_BuffData = "Buffs/";
            public const string SO_MagicData = "Magic/";
            public const string SO_TileSymbolsData = "TileSymbols/TileSymbols";
            public const string SO_TileCurseUIData = "TileCurses/";
            public const string SO_BoardEventData = "BoardEvent/";
            public const string SO_EnemyData = "Stage/Enemies/";
            public const string SO_TutorialData = "Tutorial/";
            
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

            
            public const string SoundManager = "Sound Manager";
            
            public const string UI = "UI";
            public const string MainUICanvas = "Main UI Canvas";
            public const string PlayerUICanvas = "Player UI Canvas";
            public const string CardUICanvas = "Card UI Canvas";
            public const string EnemyUICanvas = "Enemy UI Canvas";
            public const string SystemUICanvas = "System UI Canvas";
            public const string DescriptionUICanvas = "Description UI Canvas";


            public const string EnemyPlace = "Enemy";
            public const string EnemyUI = "EnemyUI";
            public const string Board = "Board";

            public const string Core = "Core";
            public const string CardManager = "CardManager";
        }

        public static class Colors {
            public static readonly Color CardinalsWhite = new Color(0.976f, 0.968f, 0.949f);
            public static readonly Color CardinalsBlack = new Color(0.282f, 0.270f, 0.243f);
        }
    }

    namespace GameSetting {
        public static class Board {
            public const float TileWidth = 1;
            public const float TileHeight = 1.5f;
            public const float TileDepth = 0.2f;
            public const float GroundDepth = 0f;
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

        public static class Player
        {
            public const int MaxPotionCapacity = 3;
            public const int MaxArtifactCapacity = 12;
            public const int CardDrawCount = 5;

            public const int InitialMagicPickNum = 2;
        }
    }
}

