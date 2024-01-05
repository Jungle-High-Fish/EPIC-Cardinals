using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cardinals.Enums;
using Util;
using System;

namespace Cardinals.Board {
    public class CircleBoardBuilder: IBoardBuilder
    {
        public Tile this[int idx] => _board[idx];
        public Tile StartTile => _board == null ? null : _board[_startTileIdx];
        public int StartTileIdx => _startTileIdx;
        public int TileCount => _tileCount;
        public float BoardRadius => _boardRadius;
        public int EdgeCount => _edgeCount;
        public int TileCountPerEdge => _tileCountPerEdge;
        public IReadOnlyList<Tile> Board => _board;

        // Board 전체 관리 컨트롤러
        private Board _boardController;

        // Board 데이터
        private List<Tile> _board;
        private int _tileCount;
        private float _boardRadius;
        private int _startTileIdx;
        private int _edgeCount;
        private int _tileCountPerEdge;

        private Action<Tile> _onTileClicked;

        #region 생성자
        public CircleBoardBuilder(Board boardController, Action<Tile> onTileClicked) {
            _boardController = boardController;
            _onTileClicked = onTileClicked;

            _board = new List<Tile>();
        }
        #endregion

        #region 보드 리소스 로드 관련 함수
        /// <summary>
        /// BoardDataSO를 이용하여 보드를 로드합니다.
        /// </summary>
        /// <param name="boardDataSO"></param>
        public void Load(BoardData boardDataSO) {
            ImmediateLoadDataSO(boardDataSO as CircleBoardDataSO);
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
        public IEnumerator LoadWithAnimation(BoardData boardDataSO, float animationDelay=0.1f) {
            yield return LoadDataSO(boardDataSO as CircleBoardDataSO, animationDelay);
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
                if (_board[i] != null) {
                    GameObject.Destroy(_board[i].gameObject);
                }
            }
        }

	    private IEnumerator LoadDataSO(CircleBoardDataSO boardDataSO, float delay=0f) {
            _board.Clear();

            _tileCount = boardDataSO.tileCount;
            _boardRadius = boardDataSO.radius;
            _startTileIdx = boardDataSO.startTileIndex;
            _edgeCount = boardDataSO.edgeCount;

            _tileCountPerEdge = _tileCount / _edgeCount;
            GameManager.I.Sound.GrassDrop();
            for (int i = 0; i < _tileCount; i++) {
                TileData tileData = new TileData() {
                    type = i % 2 == 0 ? TileType.Defence : TileType.Attack, // 짝 수 타일은 방어, 홀 수 타일은 공격
                };
                Tile tile = InstantiateTile(tileData, i);
                _board.Add(tile);
                yield return new WaitForSeconds(delay);
            }
        }

        private void ImmediateLoadDataSO(CircleBoardDataSO boardDataSO) {
            _board.Clear();
            
            _tileCount = boardDataSO.tileCount;
            _boardRadius = boardDataSO.radius;
            _startTileIdx = boardDataSO.startTileIndex;
            _edgeCount = boardDataSO.edgeCount;

            _tileCountPerEdge = _tileCount / _edgeCount;

            for (int i = 0; i < _tileCount; i++) {
                TileData tileData = new TileData() {
                    type = i % 2 == 0 ? TileType.Defence : TileType.Attack, // 짝 수 타일은 방어, 홀 수 타일은 공격
                };
                Tile tile = InstantiateTile(tileData, i);
                _board.Add(tile);
            }
        }

        private Tile InstantiateTile(TileData tileData, int idx) {
            float targetAngle = 360f / _tileCount * idx;

            Vector3 targetPos = new Vector3(
                Mathf.Sin(targetAngle * Mathf.Deg2Rad) * _boardRadius,
                0,
                Mathf.Cos(targetAngle * Mathf.Deg2Rad) * _boardRadius
            );

            GameObject tileObj = GameObject.Instantiate(
                ResourceLoader.LoadPrefab(
                    Constants.FilePath.Resources.Prefabs_Tile
                ),
                targetPos + new Vector3(0, Constants.GameSetting.Board.TileInstantiateHeight, 0),
                Quaternion.Euler(0, 180f + targetAngle, 0),
                _boardController.transform
            );

            Tile tile = tileObj.GetComponent<Tile>();
            Vector3 tilePos = targetPos;
            tilePos.y = 
                Constants.GameSetting.Board.TileDepth / 2 + 
                Constants.GameSetting.Board.GroundDepth / 2 + 
                0.05f;
            tile.Init(
                tileData, 
                _onTileClicked,
                tilePos,
                new Vector3(0, 180f + targetAngle, 0)
            );

            return tile;
        }
    }
}
    