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

        // 타일 상태 관련 변수
        private TileState _tileState;

        // 타일의 액션 관련 변수
        private TileAction _tileAction;
        public TileAction TileAction=> _tileAction;

        // 타일의 마법 관련 변수
        private TileMagic _tileMagic;

        // 타일의 이펙트 관련 변수
        private TileEffect _tileEffect;

        // 타일의 저주 관련 변수
        private TileCurse _tileCurse;

        // 타일 위 기물 관련 변수
        private List<IBoardPiece> _boardPieces = new List<IBoardPiece>();
        
        public void Init(TileData tileData, TileState tileState=TileState.Normal) {
            _tileData = tileData;
            _tileState = tileState;

            _tileAction = GetComponent(EnumHelper.GetTileActionType(_tileData.type)) as TileAction;
            if (_tileAction == null) {
                _tileAction = gameObject.AddComponent(EnumHelper.GetTileActionType(_tileData.type)) as TileAction;
            }
            
            _tileAction.Init(this);

            _tileEffect = GetComponent<TileEffect>();
            if (_tileEffect == null) {
                _tileEffect = gameObject.AddComponent<TileEffect>();
            }
            _tileEffect.Init();

            _tileCurse = GetComponent<TileCurse>();
            if (_tileCurse == null) {
                _tileCurse = gameObject.AddComponent<TileCurse>();
            }
            _tileCurse.Init();

            if (_tileAction is TileEventAction or TileNullAction) {
                _tileMagic = GetComponent<TileMagic>();
                if (_tileMagic == null) {
                    _tileMagic = gameObject.AddComponent<TileMagic>();
                }
                _tileMagic.Init();
            } else {
                _tileMagic = null;
            }
        }

        public void OnTurnEnd() {
            _tileEffect.OnTurnEnd();
            _tileCurse.OnTurnEnd();
        }

        public void Place(IBoardPiece boardPiece) {
            _boardPieces.Add(boardPiece);

            Transform pieceTransform = (boardPiece as MonoBehaviour).transform;
            pieceTransform.SetParent(transform);
            pieceTransform.localPosition = Vector3.zero + new Vector3(0, 1.3f, 0);
        }

        public void StepOn(IBoardPiece boardPiece) {
            _tileEffect.StepOnAction(boardPiece);
        }

        public void Arrive(IBoardPiece boardPiece) {
            Place(boardPiece);

            switch (_tileState) {
                case TileState.Normal:
                    _tileAction.ArriveAction();
                    _tileEffect.ArriveAction(boardPiece);
                    break;
                case TileState.Cursed:
                    _tileAction.ArriveAction();
                    _tileEffect.ArriveAction(boardPiece);
                    _tileCurse.ClearCurse();
                    ChangeState(TileState.Normal);
                    break;
                default:
                    break;
            }
        }

        public void Leave(IBoardPiece boardPiece) {
            _boardPieces.Remove(boardPiece);

            (boardPiece as MonoBehaviour).transform.SetParent(null);
        }

        public void CardAction(int value, BaseEntity target) {
            _tileAction.Act(value, target);

            if (_tileMagic != null) {
                _tileMagic.OnAction(value, target);
            }
        }

        public void SetCurse(TileCurseData data) {
            _tileCurse.SetCurse(data);
            ChangeState(TileState.Cursed);
        }

        public void SetEffect(TileEffectData data) {
            _tileEffect.SetEffect(data);
        }

        // 타일 상태에 따라서 뒤집기. 필요한 경우 애니메이션 재생
        private void ApplyState() {
            
        }

        private void ChangeState(TileState state) {
            _tileState = state;
            ApplyState();
        }
    }

}
