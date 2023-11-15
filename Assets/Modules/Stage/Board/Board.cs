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

        private int _boardWidth;
        private int _boardHeight;
        private Vector2Int _startBoardPos;
        private Vector3 _tileInstantiateLeftTopPos;

        [Button("테스트", ButtonSizes.Large)]
        public void SetBoard(BoardDataSO boardDataSO) {
            ClearBoard();

            _tileSequence = new List<Tile>();
            _board = new List<List<Tile>>();

            StartCoroutine(LoadDataSO(boardDataSO, 0.1f));
        }

        [Button("테스트 애니메이션", ButtonSizes.Large)]
        public void PlayTileAnimation(float delay=0.1f) {
            StartCoroutine(TileAnimation(delay));
        }

        private IEnumerator TileAnimation(float delay=0f) {
            for (int i = 0; i < _tileSequence.Count; i++) {
                _tileSequence[i].Animation.Shake();
                yield return new WaitForSeconds(delay);
            }
        }

        private IEnumerator LoadDataSO(BoardDataSO boardDataSO, float delay=0f) {
            _board.Clear();
            _startBoardPos = new Vector2Int(-1, -1);

            var boardData = boardDataSO.initialTileData;
            _boardWidth = boardDataSO.width;
            _boardHeight = boardDataSO.height;

            SetTopLeftPosition(_boardWidth, _boardHeight);

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
                        _startBoardPos = new Vector2Int(j, i);
                    }

                    yield return new WaitForSeconds(delay);
                }
            }
            MakeSequenceFromBoard();
        }

        private void SetTopLeftPosition(int tileNumX, int tileNumY) {
            float tileWidth = Constants.GameSetting.Board.TileWidth;
            float tileHeight = Constants.GameSetting.Board.TileHeight;
            float tileSpace = Constants.GameSetting.Board.TileSpace;

            float topLeftX;
            if (tileNumX % 2 == 0) {
                topLeftX = 
                    Constants.GameSetting.Board.BoardCenter.x - 
                    (tileNumX / 2 - 1) * (tileWidth + tileSpace) - 
                    (tileHeight + tileSpace) / 2;
            } else {
                topLeftX = 
                    Constants.GameSetting.Board.BoardCenter.x - 
                    (tileNumX / 2 - 1) * tileWidth -
                    tileNumX / 2 * tileSpace - 
                    tileWidth / 2 - 
                    tileHeight / 2;
            }
            
            float topLeftY; 
            if (tileNumY % 2 == 0) {
                topLeftY = 
                    Constants.GameSetting.Board.BoardCenter.y + 
                    (tileNumY / 2 - 1) * (tileWidth + tileSpace) + 
                    (tileHeight + tileSpace) / 2;
            } else {
                topLeftY = 
                    Constants.GameSetting.Board.BoardCenter.y + 
                    (tileNumY / 2 - 1) * tileWidth +
                    tileNumY / 2 * tileSpace + 
                    tileWidth / 2 + 
                    tileHeight / 2;
            }

            _tileInstantiateLeftTopPos = new Vector3(
                topLeftX, 
                Constants.GameSetting.Board.TileInstantiateHeight, 
                topLeftY
            );
        }

        private Tile InstantiateTile(TileData tileData, Vector2Int boardPos) {
            float targetX;
            if (boardPos.x == 0) {
                targetX = 0;
            } else if (boardPos.x == _boardWidth - 1) {
                targetX = 
                    (_boardWidth - 2) * Constants.GameSetting.Board.TileWidth +
                    (_boardWidth - 1) * Constants.GameSetting.Board.TileSpace +
                    Constants.GameSetting.Board.TileHeight;
            } else {
                targetX =
                    (Constants.GameSetting.Board.TileWidth + Constants.GameSetting.Board.TileHeight) / 2 + 
                    Constants.GameSetting.Board.TileSpace + 
                    (boardPos.x - 1) * (Constants.GameSetting.Board.TileWidth + Constants.GameSetting.Board.TileSpace);
            }

            float targetY;
            if (boardPos.y == 0) {
                targetY = 0;
            } else if (boardPos.y == _boardHeight - 1) {
                targetY = 
                    (_boardHeight - 2) * Constants.GameSetting.Board.TileWidth +
                    (_boardHeight - 1) * Constants.GameSetting.Board.TileSpace +
                    Constants.GameSetting.Board.TileHeight;
            } else {
                targetY =
                    (Constants.GameSetting.Board.TileWidth + Constants.GameSetting.Board.TileHeight) / 2 + 
                    Constants.GameSetting.Board.TileSpace + 
                    (boardPos.y - 1) * (Constants.GameSetting.Board.TileWidth + Constants.GameSetting.Board.TileSpace);
            }

            float yDegree;
            if (tileData.direction == TileDirection.Up) {
                yDegree = 90;
            } else if (tileData.direction == TileDirection.Right) {
                yDegree = 180;
            } else if (tileData.direction == TileDirection.Down) {
                yDegree = 270;
            } else if (tileData.direction == TileDirection.Left) {
                yDegree = 0;
            } else {
                yDegree = 0;
            }

            bool isCornerTile = 
                boardPos.x == 0 && boardPos.y == 0 ||
                boardPos.x == 0 && boardPos.y == _boardHeight - 1 ||
                boardPos.x == _boardWidth - 1 && boardPos.y == 0 ||
                boardPos.x == _boardWidth - 1 && boardPos.y == _boardHeight - 1;

            Debug.Log(isCornerTile);

            Vector3 targetPos = 
                _tileInstantiateLeftTopPos + new Vector3(targetX, 0, -targetY);

            GameObject tileObj = Instantiate(
                ResourceLoader.LoadPrefab(
                    isCornerTile ? 
                    Constants.FilePath.Resources.Prefabs_CornerTile :
                    Constants.FilePath.Resources.Prefabs_Tile
                ),
                targetPos,
                Quaternion.Euler(0, yDegree, 0),
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
            if (_startBoardPos.x == -1 || _startBoardPos.y == -1) {
                //Debug.LogError("Start Position is not set");
                return false;
            }

            Vector2Int targetPos = _startBoardPos;
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

