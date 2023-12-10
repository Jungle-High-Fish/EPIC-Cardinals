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

        public static Type GetEnemyInstanceType(EnemyType enemyType)
        {
            return enemyType switch
            {
                EnemyType.Krol        => typeof(Krol),
                EnemyType.TweTwe      => typeof(TweTwe),
                EnemyType.PiPi        => typeof(PiPi),
                EnemyType.PoPo        => typeof(PoPo),
                EnemyType.PicPic      => typeof(PicPic),
                EnemyType.Pazizizizic => typeof(Pazizizizic),
                EnemyType.Five        => typeof(temp_Five),
                EnemyType.Six         => typeof(temp_Six),
                EnemyType.Seven       => typeof(Temp_Seven),
                EnemyType.Eight       => typeof(Temp_Eight),
                EnemyType.Nine        => typeof(Temp_Nine),
                _ => null
            };
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
        Earth
    }

    public enum TileCurseType {
        None,
        Fireball,
        ThunderBolt,
        Seal,
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
        Krol, 
        TweTwe,
        PiPi,
        PoPo,
        PicPic,
        Pazizizizic,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
    }

    public enum EnemyGrade
    {
        Ignore,
        Common,
        Elite,
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
        Sleep,
        Confusion,
        Spawn,
        NoAction,
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
        Confusion,
        RotationRate,
        Doll,
        Stun,
    }

    public enum BuffCountDecreaseType : int
    {
        Empty = 0,
        Turn,   // 턴 마다 감소
        Event,  // 한 사건 동안 유지 (이후 해제)
        Stage,   // 한 게임 내내 유지 (이후 해제)
        Once,
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