using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Cardinals.Board;
using Cardinals.Enemy;

namespace Cardinals.Enums {

    public static class EnumHelper {
        public static Type GetTileActionType(TileType tileType) {
            switch (tileType) {
                case TileType.Null:
                    return typeof(TileNullAction);
                case TileType.Blank:
                case TileType.Start:
                    return typeof(TileEventAction);
                case TileType.Attack:
                    return typeof(TileAttack);
                case TileType.Defence:
                    return typeof(TileDefence);
                default:
                    return typeof(TileNullAction);
            }
        }

        public static Potion GetPotion(PotionType potionType)
        {
            switch (potionType)
            {
                case PotionType.Empty:
                    return null;
                case PotionType.Jump:
                    return new JumpPotion(30, "JumpPotion");
                case PotionType.Reverse:
                    return new ReversePotion(30, "ReversePotion");
                case PotionType.Quick:
                    return new QuickPotion(40, "QuickPotion");
                case PotionType.Heal:
                    return new HealPotion(30, "HealPotion");
                default:
                    return null;
            }
        }

        public static Artifact GetArtifact(ArtifactType artifactType)
        {
            switch (artifactType)
            {
                case ArtifactType.Empty:
                    return null;
                case ArtifactType.Verdant:
                    return new VerdantBlossom(200, "VerdantBlossom", artifactType);
                case ArtifactType.Grimoire:
                    return new GrimoireSatchel(250, "GrimoireSatchel", artifactType);
                case ArtifactType.Rigloo:
                    return new RiglooShoes(180, "RiglooShoes", artifactType);
                case ArtifactType.Warp:
                    return new WarpAmulet(230, "WarpAmulet", artifactType);
                default:
                    return null;
            }
        }

        public static SdfIconType GetTileIcon(TileType tileType) {
            switch (tileType) {
                case TileType.Null:
                    return SdfIconType.None;
                case TileType.Blank:
                    return SdfIconType.XSquareFill;
                case TileType.Start:
                    return SdfIconType.PlayBtnFill;
                case TileType.Attack:
                    return SdfIconType.Magic;
                case TileType.Defence:
                    return SdfIconType.DiamondFill;
                default:
                    return SdfIconType.None;
            }
        }

        public static SdfIconType GetTileDirectionIcon(TileDirection tileDirection) {
            switch (tileDirection) {
                case TileDirection.None:
                    return SdfIconType.None;
                case TileDirection.Up:
                    return SdfIconType.ArrowUp;
                case TileDirection.Down:
                    return SdfIconType.ArrowDown;
                case TileDirection.Left:
                    return SdfIconType.ArrowLeft;
                case TileDirection.Right:
                    return SdfIconType.ArrowRight;
                case TileDirection.UpLeft:
                    return SdfIconType.ArrowUpLeft;
                case TileDirection.UpRight:
                    return SdfIconType.ArrowUpRight;
                case TileDirection.DownLeft:
                    return SdfIconType.ArrowDownLeft;
                case TileDirection.DownRight:
                    return SdfIconType.ArrowDownRight;
                default:
                    return SdfIconType.None;
            }
        }

        public static Vector2Int TileDirectionToVector2Int(TileDirection tileDirection) {
            switch (tileDirection) {
                case TileDirection.None:
                    return Vector2Int.zero;
                case TileDirection.Up:
                    return Vector2Int.down;
                case TileDirection.Down:
                    return Vector2Int.up;
                case TileDirection.Left:
                    return Vector2Int.left;
                case TileDirection.Right:
                    return Vector2Int.right;
                case TileDirection.UpLeft:
                    return Vector2Int.down + Vector2Int.left;
                case TileDirection.UpRight:
                    return Vector2Int.down + Vector2Int.right;
                case TileDirection.DownLeft:
                    return Vector2Int.up + Vector2Int.left;
                case TileDirection.DownRight:
                    return Vector2Int.up + Vector2Int.right;
                default:
                    return Vector2Int.zero;
            }
        }

        public static Type GetEnemyInstanceType(EnemyType enemyType) {
            switch (enemyType) {
                case EnemyType.Ignore:
                    return null;
                case EnemyType.One:
                    return typeof(One);
                case EnemyType.Two:
                    return typeof(Two);
                case EnemyType.Three1:
                    return typeof(Three1);
                case EnemyType.Three2:
                    return typeof(Three2);
                case EnemyType.Four:
                    return typeof(Four);
                case EnemyType.Boss:
                    return typeof(Boss);
                default:
                    return null;
            }
        }
    }

    public enum TileType {
        #if UNITY_EDITOR
        [LabelText("타일 없음")]
        #endif
        Null,

        #if UNITY_EDITOR
        [LabelText("빈 타일", Icon=SdfIconType.X)]
        #endif
        Blank,

        #if UNITY_EDITOR
        [LabelText("시작 타일", Icon=SdfIconType.Play)] 
        #endif
        Start,

        #if UNITY_EDITOR
        [LabelText("공격 타일", Icon=SdfIconType.Magic)] 
        #endif
        Attack,

        #if UNITY_EDITOR
        [LabelText("방어 타일", Icon=SdfIconType.DiamondFill)]
        #endif
        Defence
    }

    public enum TileDirection {
        None,
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }
	
    public enum TileMagicType {
        None,
        Fire,
        Water,
        Earth,
        Wind,
        Light,
        Dark
    }

    public enum TileState {
        Normal,
        Cursed
    }
    
    public enum TileSelectionType {
        Single,
        Multiple,
        Sequence,
        Edge
    }

    public enum TileAnimationType {
        None,
        Shake,
        Jump
    }
    
    public enum StageEventType
    {
        Empty,
        Battle,
    }

    public enum EnemyType
    {
        Ignore,
        One, 
        Two,
        Three1,
        Three2,
        Four,
        Boss
    }
    
    public enum EnemyActionType
    {
        Empty,
        Attack,
        Defense,
        AreaAttack,
        Buff, // 버프 / 디버프
        Magic, // 마법 ?
        Unknown, // 알 수 없음 ?
    }

    public enum BuffType
    {
        Empty,
        Burn,
        Weak,
        ElectricShock, // 감전
        Poison,
        Fracture, // 골절
        Slow,
        Wet, // 젖음
        Heal,
    }

    public enum BuffCountDecreaseType : int
    {
        Empty = 0,
        Turn,   // 턴 마다 감소
        Event,  // 한 사건 동안 유지 (이후 해제)
        Stage   // 한 게임 내내 유지 (이후 해제)
    }

    public enum RewardType
    {
        Empty,
        Gold,
        Potion,
        Card,
        Artifact,
    }

    public enum PotionType : int
    {
        Empty = 0,
        Jump,
        Reverse,
        Quick,
        Heal

    }

    #region Board Event
    public enum BoardEventType : int
    {
        Empty,
        // Tile
        Roulette,
        Number,
        Shop,
    }

    public enum BoardEventRoulette
    {
        Empty,
        DrawCard,
        GetGold,
        RandomArtifact,
        RandomCard,
        RandomTileGradeUp,
        ReducedHp,
    }

    public enum BoardEventCardType
    {
        Empty,
        Draw,
        CopyOneTimeCard,
        Heal,
        Money
    }
    #endregion

    public enum ArtifactType
    {
        Empty,
        Verdant,
        Grimoire,
        Rigloo,
        Warp
    }
    public enum CardType
    {
        Normal,
        Gold
    }

    public enum CardState
    {
        Idle,
        Select
    }

    public enum MouseState
    {
        Action,
        Move,
        Cancel,
        CardEvent,
    }
    public enum CardPileType 
    {
        Empty,
        DrawPile, // 뽑을 수 있는 카드를 가진 파일
        Hand, // 손에 들고있는 카드
        DiscardPile // 카드무덤
    }

}