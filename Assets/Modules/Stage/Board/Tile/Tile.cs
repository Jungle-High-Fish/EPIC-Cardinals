using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;

namespace Cardinals.Board {

    public class Tile: MonoBehaviour {
        public TileType Type => _tileData.type;
        public TileState TileState=> _tileState;
        public TileDirection Direction => _tileData.direction;

        public Vector3 TilePositionOnGround => _tilePositionOnGround;
        public Vector3 TileRotation => _tileRotation;

        public int Level => _tileMagic.Level;
        public int Exp => _tileMagic.Exp;

        public TileCurse TileCurse => _tileCurse;
        public TileMagic TileMagic => _tileMagic;
        public TileEvent TileEvent=> _tileEvent;
        public bool HasEvent => _tileEvent.EventType != BoardEventType.Empty;
        public bool IsSealed => _isSealed;
        public bool HasTouchedGround => _hasTouchedGround;

        public UITile UITile => _uiTile.Get(gameObject);

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
        public ParticleSystem ParticleSystem => _particleSystem.Get(gameObject);

        public bool IsSelectable => _isSelectable;
        public bool IsSelected {
            get {
                if (_isSelectable == false) {
                    return false;
                }

                return _isSelected;
            }
        }
        public bool IsMouseHovered => _isMouseHovered;

        private TileData _tileData;
        
        private ComponentGetter<Rigidbody> _rigidBody
            = new ComponentGetter<Rigidbody>(TypeOfGetter.This);

        private ComponentGetter<TileAnimation> _tileAnimation
            = new ComponentGetter<TileAnimation>(TypeOfGetter.This);

        private ComponentGetter<ParticleSystem> _particleSystem
            = new ComponentGetter<ParticleSystem>(TypeOfGetter.ChildByName, "Effect Particle");
        
        // 타일 UI 관련 변수
        private ComponentGetter<UITile> _uiTile
            = new ComponentGetter<UITile>(TypeOfGetter.ChildByName, "Renderer");

        // 타일을 링크드 리스트 형태로 관리
        private Tile _next;
        private Tile _prev;

        // 타일 선택 이벤트 관련 변수
        private Action<Tile> _onClicked;

        // 타일 상태 관련 변수
        private TileState _tileState;
        private bool _isSelectable;
        private bool _isSelected;
        private bool _isMouseHovered;
        private bool _isPlayerOn;
        private bool _hasTouchedGround = false;

        // 타일 위치 관련 변수
        private Vector3 _tilePositionOnGround;
        private Vector3 _tileRotation;

        // 타일의 이벤트 관련 변수
        private TileEvent _tileEvent;

        // 타일의 마법 관련 변수
        private TileMagic _tileMagic; 

        // 타일의 이펙트 관련 변수
        private TileEffect _tileEffect;

        // 타일의 저주 관련 변수
        private TileCurse _tileCurse;

        // 타일 위 기물 관련 변수
        private List<IBoardPiece> _boardPieces = new List<IBoardPiece>();

        // 타일 봉인 관련 변수
        private bool _isSealed = false;
        
        public void Init(
            TileData tileData, 
            Action<Tile> onClicked, 
            Vector3 tilePositionOnGround,
            Vector3 tileRotation,
            TileState tileState=TileState.Normal
        ) {
            _tileData = tileData;
            _onClicked = onClicked;
            _tilePositionOnGround = tilePositionOnGround;
            _tileRotation = tileRotation;
            _tileState = tileState;

            _tileAnimation.Get(gameObject).Init();

            _tileEvent = GetComponent<TileEvent>();
            if (_tileEvent == null) {
                _tileEvent = gameObject.AddComponent<TileEvent>();
            }

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

            _tileMagic = GetComponent<TileMagic>();
            if (_tileMagic == null) {
                _tileMagic = gameObject.AddComponent<TileMagic>();
            }
            TileMagicType initialMagicType;
            if (_tileData.type == TileType.Attack) {
                initialMagicType = TileMagicType.Attack;
            } else if (_tileData.type == TileType.Defence) {
                initialMagicType = TileMagicType.Defence;
            } else {
                initialMagicType = TileMagicType.None;
            }
            _tileMagic.Init(initialMagicType);

            if (GameManager.I.Stage.Board.IsBoardSquare) {
                if (tileData.type == TileType.Attack || tileData.type == TileType.Defence) {
                    _uiTile.Get(gameObject).Init(this);
                }
            } else {
                _uiTile.Get(gameObject).Init(this);
            }
            
        }

        public void OnTurnEnd() {
            _tileEffect.OnTurnEnd();

            bool _hasCurseTerminated = _tileCurse.OnTurnEnd();
            if (_hasCurseTerminated) {
                if (_tileState == TileState.Cursed) {
                    ChangeState(TileState.Normal);
                }
            }
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
                    _tileEvent.Activate();
                    _tileEffect.ArriveAction(boardPiece);
                    break;
                case TileState.Cursed:
                    _tileEffect.ArriveAction(boardPiece);
                    _tileCurse.ClearCurse();
                    if (_isSealed == false) {
                        ChangeState(TileState.Normal);
                    } else {
                        ChangeState(TileState.Seal);
                    }
                    break;
                case TileState.Seal:
                default:
                    break;
            }

            if (_tileMagic != null) {
                _tileMagic.ApplyLevelUp();
            }

            if (boardPiece is Player) {
                transform.position = _tilePositionOnGround;
                _tileAnimation.Get(gameObject).Play(TileAnimationType.Float, false);
                _rigidBody.Get(gameObject).isKinematic = true;

                _isPlayerOn = true;
            }
        }

        public void Leave(IBoardPiece boardPiece) {
            _boardPieces.Remove(boardPiece);

            (boardPiece as MonoBehaviour).transform.SetParent(null);

            if (boardPiece is Player) {
                _rigidBody.Get(gameObject).isKinematic = false;
                _isPlayerOn = false;
            }
        }

        public IEnumerator CardAction(int value, BaseEntity target) {
            _tileMagic.OnAction(value, target);

            float animTime = _tileAnimation.Get(gameObject).Play(TileAnimationType.Attack);
            GameManager.I.CameraController.ShakeCamera(0.1f, 0.7f, 0.1f);

            yield return new WaitForSeconds(animTime);
        }

        public void SetCurse(TileCurseType curseType, int turn) {
            var data = EnumHelper.GetTileCurseInstanceType(curseType);
            data.Init(this, turn);
            _tileCurse.SetCurse(data);
            _tileEvent.ClearEvent();
            ChangeState(TileState.Cursed);
        }

        public IEnumerator SetMagicSaveData(SaveFileData.TileSaveData tileSaveData, Action onComplete) {
            yield return _tileMagic.SetSaveData(tileSaveData.TileMagicType, tileSaveData.Level, tileSaveData.Exp);
            onComplete?.Invoke();
        }

        public void SetEffect(TileEffectData data) {
            _tileEffect.SetEffect(data);
        }

        public void Select() {
            if (_isSelectable == false) {
                _isSelected = false;
                return;
            }

            _isSelected = true;

            _tileAnimation.Get(gameObject).Play(TileAnimationType.Jump, true);
        }

        public void Unselect() {
            if (_isSelectable == false) {
                _isSelected = false;
                return;
            }

            _isSelected = false;

            _tileAnimation.Get(gameObject).StopAll();
        }

        public void SetSelectable(bool isSelectable) {
            _isSelectable = isSelectable;

            _tileAnimation.Get(gameObject).StopAll();
        }

        public void ChangeState(TileState state) {
            TileState originalState = _tileState;
            _tileState = state;
            if (_tileState == TileState.Seal) {
                _isSealed = true;
            }
            ApplyState(originalState);
        }

        public void ClearSealedState() {
            _isSealed = false;
        }

        // 타일 상태에 따라서 뒤집기. 필요한 경우 애니메이션 재생
        private void ApplyState(TileState originalState) {
            if (originalState == TileState.Normal) {
                if (_tileState == TileState.Cursed) {
                    _tileAnimation.Get(gameObject).Play(TileAnimationType.Flip);
                }
            }

            if (originalState == TileState.Cursed) {
                if (_tileState == TileState.Normal || _tileState == TileState.Seal) {
                    _tileAnimation.Get(gameObject).Play(TileAnimationType.FlipBack);
                }
            }
            
            if (originalState == TileState.Seal) {
                if (_tileState == TileState.Cursed) {
                    _tileAnimation.Get(gameObject).Play(TileAnimationType.Flip);
                }
            }
        }

        private void OnMouseDown() {
            if (_isSelectable == false) {
                _isSelected = false;
                return;
            }
            _onClicked?.Invoke(this);
        }

        private void OnMouseEnter() {
            _isMouseHovered = true;

            if (_isPlayerOn == false && _isSelected == false) {
                
            }
            //GameManager.I.UI.UIHoveredTileInfo.Show(this, false, false);
        }

        private void OnMouseExit() {
            _isMouseHovered = false;

            if (_isPlayerOn == false && _isSelected == false) {
                GameManager.I.UI.UIHoveredTileInfo.Hide(false);
            }
            //GameManager.I.UI.UIHoveredTileInfo.Hide(false);
            GameManager.I.UI.UIMouseHint.Hide();
        }

        private void OnMouseOver() {
            if (Input.GetMouseButtonDown(1)) {
                GameManager.I.UI.UIHoveredTileInfo.Show(this, false, false);
            }

            if (Input.GetMouseButtonUp(1)) {
                GameManager.I.UI.UIHoveredTileInfo.Hide(false);
            }

            GameManager.I.UI.UIMouseHint.Show();
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.CompareTag("Ground")) {
                _hasTouchedGround = true;
            }
        }
    }

}
