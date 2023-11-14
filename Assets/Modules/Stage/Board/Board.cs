using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Util;

namespace Cardinals.Board {

    public class Board: MonoBehaviour {
        [ShowInInspector, ReadOnly]
        private List<Tile> _tileSequence;
        [ShowInInspector, ReadOnly]
        private List<List<Tile>> _board;

        private Vector2Int _startPos;

        [Button("테스트", ButtonSizes.Large)]
        public void SetBoard(BoardDataSO boardDataSO) {
            ClearBoard();

            _tileSequence = new List<Tile>();
            _board = new List<List<Tile>>();

            LoadDataSO(boardDataSO);
            MakeSequenceFromBoard();
        }

        private void LoadDataSO(BoardDataSO boardDataSO) {
            _board.Clear();
            _startPos = new Vector2Int(-1, -1);

            var boardData = boardDataSO.initialTileData;
            for (int i = 0; i < boardData.Count; i++) {
                _board.Add(new List<Tile>());
                for (int j = 0; j < boardData[i].tiles.Count; j++) {
                    if (boardData[i].tiles[j] == TileType.Null) {
                        _board[i].Add(null);
                        continue;
                    }

                    TileData tileData = new TileData() {
                        type = boardData[i].tiles[j],
                        direction = boardData[i].directions[j]
                    };

                    Tile tile = InstantiateTile(tileData, new Vector2Int(j, i));
                    _board[i].Add(tile);

                    if (tile.Type == TileType.Start) {
                        _startPos = new Vector2Int(j, i);
                    }
                }
            }
        }

        private Tile InstantiateTile(TileData tileData, Vector2Int boardPos) {
            GameObject tileObj = Instantiate(
                ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Tile), 
                transform
            );
            Tile tile = tileObj.GetComponent<Tile>();
            tile.Init(tileData);

            return tile;
        }

        private void ClearBoard() {
            if (_board == null) {
                return;
            }

            for (int i = 0; i < _board.Count; i++) {
                for (int j = 0; j < _board[i].Count; j++) {
                    if (_board[i][j] != null) {
                        Destroy(_board[i][j].gameObject);
                    }
                }
            }
        }

        /// <summary>
        /// 보드의 타일들을 시작 타일부터 순서대로 배치하여 _tileSequence에 저장합니다.
        /// </summary>
        /// <returns> 타일 시퀀스가 루프를 이룰 때 true를 반환합니다. </returns>
        private bool MakeSequenceFromBoard() {
            _tileSequence.Clear();
            if (_startPos.x == -1 || _startPos.y == -1) {
                //Debug.LogError("Start Position is not set");
                return false;
            }

            Vector2Int targetPos = _startPos;
            TileDirection targetDirection = _board[targetPos.y][targetPos.x].Direction;

            while (true) {
                _tileSequence.Add(_board[targetPos.y][targetPos.x]);

                targetPos = targetPos + EnumHelper.TileDirectionToVector2Int(targetDirection);
                if (targetPos.y < 0 || targetPos.y >= _board.Count || targetPos.x < 0 || targetPos.x >= _board[targetPos.y].Count) {
                    return false;
                }

                Tile nextTile = _board[targetPos.y][targetPos.x];
                if (nextTile == null) {
                    return false;
                }

                _tileSequence[_tileSequence.Count - 1].Next = nextTile;

                if (nextTile.Type == TileType.Start) {
                    return true;
                }
                
                targetDirection = nextTile.Direction;
            }
        }
    }

}

