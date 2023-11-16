using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using DG.Tweening;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Util;

namespace Cardinals.Board {

    public class Board: MonoBehaviour {
        [ShowInInspector, ReadOnly]
        private List<Tile> _tileSequence;
        private BoardBuilder _boardBuilder;

        [Button("테스트", ButtonSizes.Large)]
        public IEnumerator SetBoard(BoardDataSO boardDataSO) {
            if (_boardBuilder != null) {
                _boardBuilder.Clear();
                _boardBuilder = null;
            }

            _tileSequence = new List<Tile>();
            _boardBuilder = new BoardBuilder(this);

            yield return BoardLoadWithAnimation(boardDataSO);
        }

        [Button("테스트 애니메이션", ButtonSizes.Large)]
        public void PlayTileAnimation(float delay=0.1f) {
            StartCoroutine(TileAnimation(delay));
        }

        public Tile GetStartTile() {
            if (_boardBuilder == null) {
                return null;
            }

            return _boardBuilder.StartTile;
        }

        public void PlacePieceToTile(IBoardPiece piece, Tile tile) {
            if (tile == null) {
                Debug.LogError("Tile is null");
                return;
            }

            // if (tile.Piece != null) {
            //     return null;
            // }

            tile.Arrive(piece);

            if (piece is Player) {
                (piece as Player).SetTile(tile);
            }
        }

        public void OnTurnEnd() {
            for (int i = 0; i < _tileSequence.Count; i++) {
                _tileSequence[i].OnTurnEnd();
            }
        }

        public List<Tile> GetBoardEdgeTileSequence(int edgeIndex, bool includeCorner=true) {
            if (edgeIndex >= _boardBuilder.CornerTiles.Count) {
                return null;
            }

            List<Tile> tileSequence = new List<Tile>();

            Tile targetTile = _boardBuilder.CornerTiles[edgeIndex];

            if (includeCorner) {
                tileSequence.Add(targetTile);
            }

            while (true) {
                targetTile = targetTile.Next;
                if (targetTile == null) {
                    return null;
                }

                if (targetTile.Type == TileType.Start) {
                    return tileSequence;
                }

                if (targetTile.Type == TileType.Blank) {
                    if (includeCorner == false) {
                        return tileSequence;
                    } else {
                        tileSequence.Add(targetTile);
                        return tileSequence;
                    }
                }

                tileSequence.Add(targetTile);
            }
        }

        public Tile GetRandomTile() {
            return _tileSequence[UnityEngine.Random.Range(0, _tileSequence.Count)];
        }

        public int GetBoardEdgeNum() {
            return _boardBuilder.CornerTiles.Count;
        }

        private IEnumerator BoardLoadWithAnimation(BoardDataSO boardDataSO) {
            yield return _boardBuilder.LoadWithAnimation(boardDataSO, 0.1f);
            MakeSequenceFromBoard();
        }

        private IEnumerator TileAnimation(float delay=0f) {
            for (int i = 0; i < _tileSequence.Count; i++) {
                _tileSequence[i].Animation.Shake();
                yield return new WaitForSeconds(delay);
            }
        }

        /// <summary>
        /// 보드의 타일들을 시작 타일부터 순서대로 배치하여 _tileSequence에 저장합니다.
        /// </summary>
        /// <returns> 타일 시퀀스가 루프를 이룰 때 true를 반환합니다. </returns>
        private bool MakeSequenceFromBoard() {
            _tileSequence.Clear();
            if (_boardBuilder.IsStartTileValid() == false) {
                //Debug.LogError("Start Position is not set");
                return false;
            }

            Vector2Int targetPos = _boardBuilder.StartTilePos;
            TileDirection targetDirection = _boardBuilder[targetPos].Direction;

            while (true) {
                _tileSequence.Add(_boardBuilder[targetPos]);

                targetPos = targetPos + EnumHelper.TileDirectionToVector2Int(targetDirection);
                if (_boardBuilder.IsTilePosValid(targetPos) == false) {
                    return false;
                }

                Tile nextTile = _boardBuilder[targetPos];
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

