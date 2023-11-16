using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using UnityEngine;
using Util;

namespace Cardinals.Board {

    public class Tile : MonoBehaviour {
        public TileType Type => _tileData.type;
        public TileDirection Direction => _tileData.direction;

        public Tile Next {
            get => _next;
            set {
                _next = value;

                if (_next != null) {
                    _next._prev = this;
                }
            }
        }

        public Tile Prev {
            get => _prev;
            set {
                _prev = value;

                if (_prev != null) {
                    _prev._next = this;
                }
            }
        }

        public TileAnimation Animation => _tileAnimation.Get(gameObject);

        private TileData _tileData;
        private ComponentGetter<TileAnimation> _tileAnimation
            = new ComponentGetter<TileAnimation>(TypeOfGetter.This);

        // 타일을 링크드 리스트 형태로 관리
        private Tile _next;
        private Tile _prev;

        // 타일의 액션 관련 변수
        private TileAction _tileAction;

        // 타일 위 기물 관련 변수
        private List<IBoardPiece> _boardPieces = new List<IBoardPiece>();
        
        public void Init(TileData tileData) {
            _tileData = tileData;

            _tileAction = GetComponent(EnumHelper.GetTileActionType(_tileData.type)) as TileAction;
            if (_tileAction == null) {
                _tileAction = gameObject.AddComponent(EnumHelper.GetTileActionType(_tileData.type)) as TileAction;
            }
        }

        public void StepOn(IBoardPiece boardPiece) {
            
        }

        public void Arrive(IBoardPiece boardPiece) {
            _boardPieces.Add(boardPiece);

            (boardPiece as MonoBehaviour).transform.SetParent(transform);
        }

        public void Leave(IBoardPiece boardPiece) {
            _boardPieces.Remove(boardPiece);

            (boardPiece as MonoBehaviour).transform.SetParent(null);
        }

        public void CardAction(int value) {
            _tileAction.Act(value);
        }
    }

}
