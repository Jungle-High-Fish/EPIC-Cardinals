using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cardinals.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;
using Random = UnityEngine.Random;

namespace Cardinals.BoardEvent.Tile
{
    public class UITileEvent : MonoBehaviour
    {
        [Header("Component")]
        [SerializeField] private Button _closeBTN;
        [SerializeField] private GameObject _explainObj;
        [SerializeField] private TextMeshProUGUI _explainTMP;
        
        [Header("Event-Related")]
        [SerializeField] private Transform _typeParentTr;
        public TileMagicType SelectedMagicType { set; private get; }
        private IEnumerator _eventSeq;
        private IEnumerator _selectedSeq;
        

        [Header("Data")]
        private List<Board.Tile> _selectedTiles;
        public void Awake()
        {
            _closeBTN.onClick.AddListener(End);
        }

        public void Init()
        {
            _explainTMP.text = string.Empty;

            _closeBTN.gameObject.SetActive(false);
            _explainObj.SetActive(false);
            _typeParentTr.gameObject.SetActive(false);
            
            gameObject.SetActive(true);
            
            // 플로우 시작
            StartCoroutine(EventFlow());
        }

        private void End()
        {
            gameObject.SetActive(false);
                    
            if (_selectedSeq != null)
            {
                StopCoroutine(_selectedSeq);
                _selectedSeq = null;
            }
            
            if (_eventSeq != null)
            {
                StopCoroutine(_eventSeq);
                _eventSeq = null;
            }
        }
        
        IEnumerator EventFlow()
        {
            int idx = Random.Range(0, 3);
            _eventSeq = idx switch
            {
                0 => MoveExp(),
                1 => AddExpToLine(2),
                2 => AddExpToNTile(5),
                3 => ChangeTileType(),
                4 => AddExpByType(2),
            };

            yield return _eventSeq; // 각 이벤트 수행
            
            SetMessage("성공적으로 이벤트가 완료되었다.");
            yield return new WaitForSeconds(1.5f); // 이벤트 완료 후, 잠깐 대기
            End();
        }

        /// <summary>
        /// 오류 출력 함수
        /// </summary>
        /// <param name="text"></param>
        void PrintFailMessage(string text)
        {
            Debug.Log(text);
        }

        void SetMessage(string msg)
        {
            _explainObj.SetActive(true);
            _explainTMP.text = msg;
        }
        
        /// <summary>
        /// 공용 타일 선택 함수
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerator SelectTile(TileSelectionType type,
                               string title = "타일을 선택하세요.",
                               string description = "")
        {
            var tileSelection = GameManager.I.Stage.Board.RequestTileSelect(type, title, description);
            
            _selectedSeq = tileSelection.tileRequester(); 
            yield return _selectedSeq;

            if (tileSelection.selectedTiles.Any())
            {
                _selectedTiles = tileSelection.selectedTiles;
            }
            else // 선택 취소함
            {
                SetMessage("선택을 취소했습니다.\n이벤트가 종료됩니다.");
                yield return new WaitForSeconds(1f);
                End();
            }
            
            _selectedSeq = null;
            
        }

        /// <summary>
        /// 한 타일의 경험치를 0으로 만들고, 그 경험치 만큼 다른 타일에 적용
        /// </summary>
        /// <returns></returns>
        IEnumerator MoveExp()
        {
            Board.Tile senderTile = null;
            Board.Tile receiverTile = null;

            // [TODO] 이동할 경험치 타일이 존재하지 않는 경우, 이벤트 종료?
            
            // sender 타일 선택
            do
            {
                yield return SelectTile(TileSelectionType.Single,
                                        title: "경험치를 옮기려는 타일을 선택해주세요.", 
                                        description: "한 타일의 경험치를 0으로 만들고, 그 경험치 만큼 다른 타일에 적용");
                
                var tile = _selectedTiles.First();
                if (tile.TileMagic.Exp == 0) // 이동할 경험치가 있는지 체크
                {
                    PrintFailMessage("이동할 경험치가 없다는 안내 필요");
                }
                else
                {
                    senderTile = tile;
                }
            } while (senderTile == null);

            // receiver 타일 선택
            do
            {
                yield return SelectTile(TileSelectionType.Single,
                                        title: "경험치를 적용할 타일을 선택해주세요.", 
                                        description: "한 타일의 경험치를 0으로 만들고, 그 경험치 만큼 다른 타일에 적용");
                var tile = _selectedTiles.First();
                
                if (tile.TileMagic.Level == Constants.GameSetting.Tile.MaxLevel)
                {
                    PrintFailMessage("이미 최대 레벨에 도달했습니다.");
                }
                else
                {
                    receiverTile = tile;
                }
            } while (receiverTile == null);

            // 경험치 이전
            receiverTile.TileMagic.GainExp(senderTile.TileMagic.Exp);
            senderTile.TileMagic.Exp = 0;
        }

        /// <summary>
        /// 한 라인에 경험치를 제공
        /// </summary>
        /// <param name="value">제공할 경험치</param>
        /// <returns></returns>
        IEnumerator AddExpToLine(int value)
        {
            yield return SelectTile(TileSelectionType.Edge,
                                    "라인을 선택해주세요.",
                                    $"라인 하나에 경험치를 {value}씩 제공");

            foreach (var tile in _selectedTiles)
            {
                tile.TileMagic.GainExp(value);
            }
        }

        /// <summary>
        /// total Exp를 원하는 만큼 분배해서 적용
        /// </summary>
        /// <param name="totalExp"></param>
        /// <returns></returns>
        IEnumerator AddExpToNTile(int totalExp)
        {
            int cnt = 0;
            do
            {
                // 타일 지정
                yield return SelectTile(TileSelectionType.Single,
                                    "아래 횟수만큼 원하는 타일에 경험치를 적용",
                                    $"{cnt}/{totalExp}");
                Board.Tile tile = _selectedTiles.First();
                
                if (tile.TileMagic.Level == Constants.GameSetting.Tile.MaxLevel)
                {
                    PrintFailMessage("이미 최대 레벨에 도달했습니다.");
                }
                else
                {
                    tile.TileMagic.GainExp(1);
                    cnt++;
                }
            } while (cnt < totalExp);
        }

        /// <summary>
        /// 원하는 타일의 속성을 변경
        /// </summary>
        /// <returns></returns>
        IEnumerator ChangeTileType()
        {
            // [TODO] 레벨 1이상의 타일만 지정 필요
            Board.Tile tile;
            do
            {
                yield return SelectTile(TileSelectionType.Single,
                                         description: "원하는 타일의 속성을 변경");
                tile = _selectedTiles.First();

                if (tile.TileMagic.Type == TileMagicType.None)
                {
                    PrintFailMessage("타일에 타입이 존재하지 않아 선택할 수 없습니다.");
                    tile = null;
                }
            } while (tile == null);
            
            
            // 변경할 타입을 지정
            var typeList =
                GameManager.I.Stage.Board.TileSequence
                    .Select(t => t.TileMagic.Type)
                    .Where(type => type != TileMagicType.None && type != tile.TileMagic.Type ); // 기본과 기존 타입과 다르게 설정하기
            InstTypeItems(typeList);

            // 타입을 지정할 때까지 대기
            SelectedMagicType = TileMagicType.None;
            yield return new WaitUntil(() => SelectedMagicType != TileMagicType.None);
            
            tile.TileMagic.SetType(SelectedMagicType);
        }

        /// <summary>
        /// 원하는 속성 타일들에 경험치 제공
        /// </summary>
        /// <param name="value"></param>
        IEnumerator AddExpByType(int value)
        {
            _explainTMP.gameObject.SetActive(true);
            _explainTMP.text = $"원하는 속성 타일들에 경험치 {value}씩 제공";
            
            // _typeSelectObj 에 현재 플레이어가 보유한 타입만 설정 필요
            var typeList =
                GameManager.I.Stage.Board.TileSequence
                    .Where(t => t.TileMagic != null && t.TileMagic.Type != TileMagicType.None)
                    .Select(t => t.TileMagic.Type);

            if (typeList?.Count() > 0)
            {
                _closeBTN.gameObject.SetActive(true);
                
                InstTypeItems(typeList);
            
                // 타입을 지정할 때까지 대기
                SelectedMagicType = TileMagicType.None;
                yield return new WaitUntil(() => SelectedMagicType != TileMagicType.None); 

                // 지정한 타입들만 경험치 부여
                var tiles = GameManager.I.Stage.Board.TileSequence.Select(x => x.TileMagic)
                    .Where(x => x.Type == SelectedMagicType);
                foreach (var tile in tiles)
                {
                    tile.GainExp(value);
                }
            }
            else
            {
                SetMessage("지정 가능한 타입이 존재하지 않아 이벤트 생략댐");
                yield return new WaitForSeconds(1.5f);
            }
            
        }

        /// <summary>
        /// 입력된 타입들의 선택지를 사용자에게 제공 
        /// </summary>
        private void InstTypeItems(IEnumerable<TileMagicType> typeList)
        {
            // 기존 타입 아이콘이 존재하는 경우, 제거
            if (_typeParentTr.childCount > 0)
            {
                for (int i = _typeParentTr.childCount; i >= 0; i--)
                {
                    Destroy(_typeParentTr.GetChild(i).gameObject);    
                }
            }
            
            // 아이템 생성
            foreach (var type in typeList)
            {
                var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_BoardEvent_TypeItem);
                var item = Instantiate(prefab, _typeParentTr).GetComponent<UITypeItem>();
                item.Init(type);
            }
            
            // 오브젝트 활성화
            _typeParentTr.gameObject.SetActive(true);
        }
    }
}
