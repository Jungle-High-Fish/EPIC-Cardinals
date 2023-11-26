using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cardinals.Enums;
using DG.Tweening;
using JetBrains.Annotations;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Cardinals.Board {

    public class Board: MonoBehaviour {
        public Tile this[int x] => _tileSequence[x];
        public Tile this[int x, int y] => _boardBuilder[x, y];
        public IEnumerable<Tile> TileSequence => _tileSequence;

        public event Action<bool> OnMouseHover {
            add => _mouseDetector.OnMouseHover += value;
            remove => _mouseDetector.OnMouseHover -= value;
        }

        [ShowInInspector, ReadOnly]
        private List<Tile> _tileSequence;
        private BoardBuilder _boardBuilder;
        private MouseDetector _mouseDetector;
        
        private bool _isTileSelectable;
        private TileSelectionType _selectionType;
        private List<Tile> _selectedTiles;

        [Button("테스트", ButtonSizes.Large)]
        public IEnumerator SetBoard(BoardDataSO boardDataSO) {
            if (_boardBuilder != null) {
                _boardBuilder.Clear();
                _boardBuilder = null;
            }

            _tileSequence = new List<Tile>();
            _boardBuilder = new BoardBuilder(this, OnTileClicked);

            var mouseDetectorObj = Instantiate(
                ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_MouseDetector),
                transform
            );
            _mouseDetector = mouseDetectorObj.GetComponent<MouseDetector>();

            yield return BoardLoadWithAnimation(boardDataSO);
            _mouseDetector.Init(_boardBuilder);
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
        
        

        public IEnumerable<Tile> GetCursedTilesList()
        {
            var list = TileSequence
                .Where(t => t.Type == TileType.Attack || t.Type == TileType.Defence)
                .Where(t => t.TileState == TileState.Normal);

            if (!list.Any())
            {   
                Debug.Log("저주를 걸 수 있는 타일이 존재하지 않습니다.");
                list = null;
            }
            
            return list;
        }

        /// <summary>
        /// 타일 선택 요청을 합니다.
        /// </summary>
        /// <returns>
        /// 선택된 타일 리스트와 코루틴 함수를 반환합니다. 
        /// 해당 코루틴 함수가 실행 완료되어야 리스트에 제대로 된 값이 들어있음이 보장됩니다.
        /// </returns>
        public (List<Tile> selectedTiles, Func<IEnumerator> tileRequester) RequestTileSelect(
            TileSelectionType selectionType,
            string title="",
            string description=""
        ) {
            bool hasRequestHandled = false;
            List<Tile> result = new List<Tile>();

            void OnConfirmSelect() {
                result.AddRange(_selectedTiles);
                hasRequestHandled = true;
            }

            void OnCancelSelect() {
                result.Clear();
                hasRequestHandled = true;
            }

            IEnumerator TileRequestCoroutine() {
                GameManager.I.UI.UITileSelection.Init(
                    () => _selectedTiles,
                    OnConfirmSelect,
                    OnCancelSelect,
                    title,
                    description
                );

                _selectionType = selectionType;
                SetTileSelectable();
                yield return new WaitUntil(() => hasRequestHandled);
                SetTileUnSelectable();
            }

            return (result, TileRequestCoroutine);
        }

        /// <summary>
        /// 타일이 레벨업하는 이벤트를 발생시키는 함수입니다.
        /// </summary>
        /// <returns>
        /// Requester 코루틴과 Result 함수를 반환합니다. 
        /// Requester가 먼저 실행완료 되어야 Result가 올바른 값을 반환하는 것이 보장됩니다.
        /// </returns>
        public (Func<IEnumerator> Requester, Func<(TileMagicType newMagic, int newLevel)> Result) RequestTileLevelUp(
            TileMagicType originalMagicType,
            int originalLevel
        ) {
            return GameManager.I.UI.UIMagicLevelUpPanel.RequestTileLevelUp(originalMagicType, originalLevel);
        }

        private IEnumerator BoardLoadWithAnimation(BoardDataSO boardDataSO) {
            yield return _boardBuilder.LoadWithAnimation(boardDataSO, 0.1f);
            MakeSequenceFromBoard();
        }

        private IEnumerator TileAnimation(float delay=0f) {
            for (int i = 0; i < _tileSequence.Count; i++) {
                _tileSequence[i].Animation.Play(TileAnimationType.Shake);
                yield return new WaitForSeconds(delay);
            }
        }

        private void SetTileSelectable() {
            _isTileSelectable = true;
            _selectedTiles = new List<Tile>();

            foreach (Tile tile in _tileSequence) {
                tile.SetSelectable(true);
            }
        }


        private void SetTileUnSelectable() {
            _isTileSelectable = false;
            _selectedTiles = null;

            foreach (Tile tile in _tileSequence) {
                tile.SetSelectable(false);
            }
        }

        private void OnTileClicked(Tile target) {
            if (_isTileSelectable == false) {
                return;
            }

            if (_selectionType == TileSelectionType.Single) {
                SingleTileSelect(target);
                return;
            }

            if (_selectionType == TileSelectionType.Multiple) {
                MultipleTileSelect(target);
                return;
            }

            if (_selectionType == TileSelectionType.Sequence) {
                SequenceTileSelect(target);
                return;
            }

            if (_selectionType == TileSelectionType.Edge) {
                EdgeTileSelect(target);
                return;
            }
        }

        private void SingleTileSelect(Tile target) {
            UnselectAll();
            _selectedTiles.Add(target);
            target.Select();
        }

        private void MultipleTileSelect(Tile target) {
            if (_selectedTiles.Contains(target)) {
                _selectedTiles.Remove(target);
                target.Unselect();
            } else {
                List<Tile> temp = new List<Tile>(_selectedTiles);
                UnselectAll();
                _selectedTiles.Add(target);
                _selectedTiles.AddRange(temp);
                foreach (Tile tile in _selectedTiles) {
                    tile.Select();
                }
            }
        }

        private void SequenceTileSelect(Tile target) {
            if (_selectedTiles.Count == 0) {
                _selectedTiles.Add(target);
                target.Select();
                return;
            }

            if (_selectedTiles.Contains(target)) {
                int index = _selectedTiles.IndexOf(target);
                for (int i = _selectedTiles.Count - 1; i > index; i--) {
                    _selectedTiles[i].Unselect();
                    _selectedTiles.RemoveAt(i);
                }
                return;
            }

            if (_selectedTiles[_selectedTiles.Count - 1].Next == target) {
                _selectedTiles.Add(target);
                target.Select();
            } else if (_selectedTiles[0].Prev == target) {
                _selectedTiles.Insert(0, target);
                target.Select();
            } else {
                foreach (Tile tile in _selectedTiles) {
                    tile.Unselect();
                }
                _selectedTiles.Clear();
                _selectedTiles.Add(target);
                target.Select();
            }
        }

        private void EdgeTileSelect(Tile target) {
            if (_selectedTiles.Contains(target)) {
                UnselectAll();
                return;
            }

            UnselectAll();
            _selectedTiles.AddRange(GetEdgeContains(target));
                
            foreach(Tile tile in _selectedTiles) {
                tile.Select();
            }
        }

        private void UnselectAll() {
            foreach (Tile tile in _selectedTiles) {
                tile.Unselect();
            }
            _selectedTiles.Clear();
        }

        private List<Tile> GetEdgeContains(Tile target) {
            List<Tile> result = new List<Tile>();
            
            List<Tile> prevs = new List<Tile>();
            for (Tile t = target.Prev; _boardBuilder.CornerTiles.Contains(t) == false; t = t.Prev) {
                prevs.Add(t);
            }
            prevs.Reverse();

            result.AddRange(prevs);
            result.Add(target);

            for (Tile t = target.Next; _boardBuilder.CornerTiles.Contains(t) == false; t = t.Next) {
                result.Add(t);
            }

            return result;
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


        /// <summary>
        /// 이벤트 발생 가능한 타일을 반환하는 함수
        /// </summary>
        /// <returns>발생 가능한 타일이 없는 경우, null 반환 </returns>
        public TileEventAction GetCanSetEventTileEventAction()
        {
            TileEventAction returnEvtAction = null;

            List<TileEventAction> list = new();
            foreach (var tile in _tileSequence.Where(x=> x.Type == TileType.Start || x.Type == TileType.Blank)) // 코너 타일만
            {
                var eventAction = tile.TileAction as TileEventAction;
                if(eventAction != null) // 혹시 형변환 몰라 체크
                { 
                    if (eventAction.EventType == default) // 현재 이벤트 지정이 되있지 않은 경우
                    {
                        list.Add(eventAction);
                    }
                }
            }

            if (list.Count > 0)
            {
                var idx = Random.Range(0, list.Count());
                returnEvtAction = list[idx];
            }
            
            return returnEvtAction;
        }
    }

}

