using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif

namespace Cardinals.Board {

    [CreateAssetMenu(fileName = "BoardData", menuName = "Stage/Board Data", order = 0)]
    public class BoardDataSO: ScriptableObject {
        [System.Serializable]
        public class LineWrapper {
            public List<TileType> tiles;
            public List<TileDirection> directions;
        }

        public const int DefaultBoardSize = 5;

        [LabelText("보드 이름")]
        public string boardName = "New Board";
        
        [FoldoutGroup("보드 크기")]
        [SerializeField, LabelText("정사각형 여부"), OnValueChanged("UpdateBoardSize")]
        private bool _isSquare = true;
        [FoldoutGroup("보드 크기")]
        [LabelText("가로"), OnValueChanged("UpdateWidth"), Range(1, 10)]
        public int width = DefaultBoardSize;
        [FoldoutGroup("보드 크기")]
        [LabelText("세로"), OnValueChanged("UpdateHeight"), Range(1, 10)]
        public int height = DefaultBoardSize;
        
        #if UNITY_EDITOR
        [FoldoutGroup("보드 구성")]
        [ShowInInspector, EnumToggleButtons, HideLabel] 
        #endif
        private static TileType _selectedTile;

        #if UNITY_EDITOR
        [FoldoutGroup("보드 구성")]
        [ShowInInspector, TableMatrix(DrawElementMethod="DrawTile", SquareCells=true)]
        [OnValueChanged("UpdateBoardData")]
        #endif
        private TileType[,] _initialTiles;

        #if UNITY_EDITOR
        [FoldoutGroup("보드 방향 구성"), LabelText("수동 방향 설정"), ShowInInspector]
        public bool manualDirection = false;
        [FoldoutGroup("보드 방향 구성")]
        [ShowInInspector, EnumToggleButtons, HideLabel, ShowIf("manualDirection")] 
        #endif
        private static TileDirection _selectedDirection;

        #if UNITY_EDITOR
        [FoldoutGroup("보드 방향 구성")]
        [ShowInInspector, TableMatrix(DrawElementMethod="DrawTileDirection", SquareCells=true, IsReadOnly=true), ShowIf("manualDirection")]
        [OnValueChanged("UpdateBoardData")]
        #endif
        private TileDirection[,] _initialTileDirections;

        public List<LineWrapper> initialTileData;

        public void Init(string name) {
            boardName = name;
            
            _initialTiles = new TileType[width, height];
            _initialTileDirections = new TileDirection[width, height];
        }

        #if UNITY_EDITOR
        private static TileType DrawTile(Rect rect, TileType tileType) {
            TileType tile = tileType;

            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition)) {
                if (Event.current.button == 0) {
                    tile = _selectedTile;
                } else if (Event.current.button == 1) {
                    tile = TileType.Null; 
                }
                GUI.changed = true;
                Event.current.Use();
            }

            SdfIcons.DrawIcon(rect, EnumHelper.GetTileIcon(tile));

            return tile;
        }

        private static TileDirection DrawTileDirection(Rect rect, TileDirection tileDirection) {
            TileDirection direction = tileDirection;

            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition)) {
                if (Event.current.button == 0) {
                    direction = _selectedDirection;
                } else if (Event.current.button == 1) {
                    direction = TileDirection.None; 
                }
                GUI.changed = true;
                Event.current.Use();
            }

            SdfIcons.DrawIcon(rect, EnumHelper.GetTileDirectionIcon(direction));

            return direction;
        }
        #endif

        private void UpdateWidth() {
            if (_isSquare) {
                height = width;
            }

            UpdateBoardSize();
        }

        private void UpdateHeight() {
            if (_isSquare) {
                width = height;
            }

            UpdateBoardSize();
        }

        private void UpdateBoardSize() {
            if (_isSquare) {
                if (height < width) {
                    height = width;
                } else {
                    width = height;
                };
            }

            _initialTiles = new TileType[width, height];
            _initialTileDirections = new TileDirection[width, height];
        }

        private void UpdateBoardData() {
            initialTileData = new List<LineWrapper>();

            for (int y = 0; y < height; y++) {
                LineWrapper line = new LineWrapper();
                line.tiles = new List<TileType>();
                line.directions = new List<TileDirection>();

                for (int x = 0; x < width; x++) {
                    line.tiles.Add(_initialTiles[x, y]);
                    line.directions.Add(_initialTileDirections[x, y]);
                }

                initialTileData.Add(line);
            }
        }

        [Button("로드"), OnInspectorInit]
        private void DataRead() {
            if (_initialTiles != null && _initialTileDirections != null) {
                UpdateBoardData();
                return;
            }

            width = initialTileData[0].tiles.Count;
            height = initialTileData.Count;

            _initialTiles = new TileType[width, height];
            _initialTileDirections = new TileDirection[width, height];

            for (int y = 0; y < height; y++) {
                LineWrapper line = initialTileData[y];

                for (int x = 0; x < width; x++) {
                    _initialTiles[x, y] = line.tiles[x];
                    _initialTileDirections[x, y] = line.directions[x];
                }
            }
        }
    }

}

