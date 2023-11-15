using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Cardinals.Enums {

    public static class EnumHelper {
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
    
    public enum StageEventType
    {
        Ignore,
        Battle,
    }
    
}