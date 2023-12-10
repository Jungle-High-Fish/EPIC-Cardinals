using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Cardinals.Board;
using Cardinals.Board.Curse;
using Cardinals.Enemy;
using Cardinals.Game;

namespace Cardinals.Enums {

    public static class EnumHelper {
        public static Potion GetPotion(PotionType potionType)
        {
            switch (potionType)
            {
                case PotionType.Empty:
                    return null;
                case PotionType.Growth:
                    return new GrowthPotion();
                case PotionType.Jump:
                    return new JumpPotion();
                case PotionType.Reverse:
                    return new ReversePotion();
                case PotionType.Quick:
                    return new QuickPotion();
                case PotionType.Heal:
                    return new HealPotion();
                case PotionType.One:
                    return new OnePotion();
                case PotionType.Two:
                    return new TwoPotion();
                case PotionType.Three:
                    return new ThreePotion();
                case PotionType.Four:
                    return new FourPotion();
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
                    return new VerdantBlossom(artifactType);
                case ArtifactType.Grimoire:
                    return new GrimoireSatchel(artifactType);
                case ArtifactType.Rigloo:
                    return new RiglooShoes(artifactType);
                case ArtifactType.Warp:
                    return new WarpAmulet(artifactType);
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

        public static TileCurseData GetTileCurseInstanceType(TileCurseType type)
        {
            return type switch
            {
                TileCurseType.Fireball => new Fireball(),
                TileCurseType.ThunderBolt => new ThunderBolt(),
                _ => null
            };

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
	
    public enum TileMagicType : int {
        None = 0,
        Fire,
        Water,
        Earth,
        Attack,
        Defence
    }

    public enum TileCurseType {
        None,
        Fireball,
        ThunderBolt
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
        Jump,
        Flip,
        FlipBack,
        Float,
        FloatLittle,
        FloatDown,
        Attack,
        Defence,
        Rotate360
    }
    
    public enum StageEventType
    {
        Empty,
        Tutorial,
        BattleCommon,
        BattleElite,
        BattleBoss,
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
        TileCurse,
        TileDebuff,
        UserDebuff,
        Unknown, // 알 수 없음 ?
    }

    public enum BuffType
    {
        Empty,
        Burn,
        Weak,
        ElectricShock, // 감전
        Poison,
        Slow,
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
        Growth,
        Jump,
        Reverse,
        Quick,
        Heal,
        One,
        Two,
        Three,
        Four

    }

    public enum BlessType : int
    {
        Empty,
        BlessFire1,
        BlessFire2,
        BlessWater1,
        BlessWater2,
        BlessEarth1,
        BlessEarth2,
    }

    #region Board Event
    public enum BoardEventType : int
    {
        Empty,
        Tile,
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
        //RandomCard,
        RandomPotion,
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

    public enum ArtifactType : int
    {
        Empty = 0,
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

    public enum DiceType
    {
        Empty,
        Normal,
        Fire,
        Water,
        Earth,
        Gold
    }

    public enum DiceAnimationType
    {
        Empty,
        UseAttack,
        UseDefense,
        UseMove,
        TurnEnd
    }

    public enum CardState
    {
        Idle,
        Select
    }
    public enum CardAnimationType
    {
        Empty,
        UseAttack,
        UseDefense,
        UseMove,
        TurnEnd
    }
    public enum MouseState
    {
        Action,
        Move,
        Cancel,
        CardEvent,
    }

    public enum UIMouseDetectorType
    {
        CardPile,
        CardEvent,
    }

    public enum CardPileType 
    {
        Empty,
        DrawPile, // 뽑을 수 있는 카드를 가진 파일
        Hand, // 손에 들고있는 카드
        DiscardPile // 카드무덤
    }
    
    public enum PlayerActionType 
    {
        None,
        Cancel,
        CantUsed,
        Move,
        Attack,
        Defense,
    }
    
    public enum TutorialQuestType
    {
        Card,
        TileMagicSelect,
        KillMonster,
        EndTurn,
        BlessSelect
    }
}