using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cardinals.Enums;
using Cardinals.Constants;
using Util;
using System.Runtime.InteropServices;

namespace Cardinals.Board {
    public class BoardBuilder
    {
        public Tile this[int x, int y] => _board[y][x];
        public Tile this[Vector2Int pos] => _board[pos.y][pos.x];
        public List<Tile> CornerTiles => _cornerTiles;
        public int TileNumW => _boardWidth;
        public int TileNumH => _boardHeight;
        public Vector2Int StartTilePos => _startTilePos;
        public Tile StartTile => _board[_startTilePos.y][_startTilePos.x];

        // Board 전체 관리 컨트롤러
        private Board _boardController;

        // Board 데이터
        private List<List<Tile>> _board;
        private List<Tile> _cornerTiles;
        private int _boardWidth;
        private int _boardHeight;
        private Vector2Int _startTilePos;

        // Tile 생성 임시 변수
        private Vector3 _tileInstantiateLeftTopPos;

        #region 생성자
        public BoardBuilder(Board boardController) {
            _boardController = boardController;
            _board = new List<List<Tile>>();
            _cornerTiles = new List<Tile>();

            _boardWidth = 0;
            _boardHeight = 0;

            _startTilePos = new Vector2Int(-1, -1);
        }
        #endregion

        #region 보드 리소스 로드 관련 함수
        /// <summary>
        /// BoardDataSO를 이용하여 보드를 로드합니다.
        /// </summary>
        /// <param name="boardDataSO"></param>
        public void Load(BoardDataSO boardDataSO) {
            ImmediateLoadDataSO(boardDataSO);
        }

        /// <summary>
        /// BoardDataSO를 이용하여 보드를 로드하는 코루틴입니다.
        /// 애니메이션과 함께 재생되며, 애니메이션이 끝날 때까지 데이터 로드가 보장되지 않습니다.
        /// </summary>
        /// <param name="boardDataSO"></param>
        /// <param name="animationDelay">
        /// 타일과 타일 로드 간 시간 간격입니다. 
        /// 애니메이션 재생 시간은 이 값에 타일 갯수를 곱한 것과 같습니다.
        /// </param>
        /// <returns></returns>
        public IEnumerator LoadWithAnimation(BoardDataSO boardDataSO, float animationDelay=0.1f) {
            yield return LoadDataSO(boardDataSO, animationDelay);
        }
        #endregion

        /// <summary>
        /// 보드 데이터를 삭제합니다.
        /// </summary>
        public void Clear() {
            if (_board == null) {
                return;
            }

            for (int i = 0; i < _board.Count; i++) {
                for (int j = 0; j < _board[i].Count; j++) {
                    if (_board[i][j] != null) {
                        GameObject.Destroy(_board[i][j].gameObject);
                    }
                }
            }

            _cornerTiles.Clear();
            _cornerTiles = null;
        }

        /// <summary>
        /// 로드 된 데이터에서 시작 타일의 위치가 유효한지 확인합니다.
        /// </summary>
        public bool IsStartTileValid() {
            return _startTilePos.x != -1 && _startTilePos.y != -1;
        }

        /// <summary>
        /// 매개변수로 주어진 좌표가 보드 내에 존재하는지 확인합니다.
        /// </summary>
        /// <param name="pos">보드를 N*M 크기의 행렬로 보았을 때, 최좌상단을 (0, 0)으로 간주한 좌표값입니다.</param>
        /// <returns></returns>
        public bool IsTilePosValid(Vector2Int pos) {
            return pos.x >= 0 && pos.x < _boardWidth && pos.y >= 0 && pos.y < _boardHeight;
        }

	    private IEnumerator LoadDataSO(BoardDataSO boardDataSO, float delay=0f) {
            _board.Clear();
            _startTilePos = new Vector2Int(-1, -1);

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

                    Tile tile = InstantiateTile(tileData, new Vector2Int(j, i), _boardController.transform);
                    _board[i].Add(tile);

                    if (tile.Type == TileType.Start) {
                        _startTilePos = new Vector2Int(j, i);
                    }

                    if (tile.Type == TileType.Blank || tile.Type == TileType.Start) {
                        _cornerTiles.Add(tile);
                    }

                    yield return new WaitForSeconds(delay);
                }
            }
        }

        private void ImmediateLoadDataSO(BoardDataSO boardDataSO) {
            _board.Clear();
            _startTilePos = new Vector2Int(-1, -1);

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

                    Tile tile = InstantiateTile(tileData, new Vector2Int(j, i), _boardController.transform);
                    _board[i].Add(tile);

                    if (tile.Type == TileType.Start) {
                        _startTilePos = new Vector2Int(j, i);
                    }
                }
            }
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

        private Tile InstantiateTile(TileData tileData, Vector2Int boardPos, Transform boardObjTransform) {
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

            // bool isCornerTile = 
            //     boardPos.x == 0 && boardPos.y == 0 ||
            //     boardPos.x == 0 && boardPos.y == _boardHeight - 1 ||
            //     boardPos.x == _boardWidth - 1 && boardPos.y == 0 ||
            //     boardPos.x == _boardWidth - 1 && boardPos.y == _boardHeight - 1;
            bool isCornerTile = tileData.type == TileType.Blank;

            //Debug.Log(isCornerTile);

            Vector3 targetPos = 
                _tileInstantiateLeftTopPos + new Vector3(targetX, 0, -targetY);

            GameObject tileObj = GameObject.Instantiate(
                ResourceLoader.LoadPrefab(
                    isCornerTile ? 
                    Constants.FilePath.Resources.Prefabs_CornerTile :
                    Constants.FilePath.Resources.Prefabs_Tile
                ),
                targetPos,
                Quaternion.Euler(0, yDegree, 0),
                boardObjTransform
            );
            Tile tile = tileObj.GetComponent<Tile>();
            tile.Init(tileData);

            return tile;
        }
    }
}
    