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
        [SerializeField] private TextMeshProUGUI _explainTMP;
        [SerializeField] private TextMeshProUGUI _addTextTMP;
        
        [Header("Event-Related")]
        [SerializeField] private Transform _typeParentTr;
        public TileMagicType SelectedMagicType { set; private get; }

        [Header("Data")]
        private List<Board.Tile> _selectedTiles;
        public void Awake()
        {
            _closeBTN.onClick.AddListener(() => gameObject.SetActive(false));
        }

        public void Init()
        {
            _explainTMP.text = string.Empty;
            _addTextTMP.text = string.Empty;
            _typeParentTr.gameObject.SetActive(false);
         
            gameObject.SetActive(true);
            
            // 플로우 시작
            StartCoroutine(EventFlow());
        }
        
        IEnumerator EventFlow()
        {
            int idx = Random.Range(0, 5);
            IEnumerator nextSeq = idx switch
            {
                0 => MoveExp(),
                1 => AddExpToLine(2),
                2 => AddExpToNTile(5),
                3 => ChangeTileType(),
                4 => AddExpByType(2),
            };

            yield return nextSeq; // 각 이벤트 수행
            
            yield return new WaitForSeconds(1.5f); // 이벤트 완료 후, 잠깐 대기
        }
        
        

        /// <summary>
        /// 오류 출력 함수
        /// </summary>
        /// <param name="text"></param>
        void PrintFailMessage(string text)
        {
            Debug.Log(text);
        }
        
        /// <summary>
        /// 공용 타일 선택 함수
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerator SelectTile(TileSelectionType type)
        {
            var tileSelection = GameManager.I.Stage.Board.RequestTileSelect(
                type,
                "타일을 선택하세요",
                ""
            );
            yield return tileSelection.tileRequester();
            _selectedTiles = tileSelection.selectedTiles;
        }

        /// <summary>
        /// 한 타일의 경험치를 0으로 만들고, 그 경험치 만큼 다른 타일에 적용
        /// </summary>
        /// <returns></returns>
        IEnumerator MoveExp()
        {
            _explainTMP.text = "한 타일의 경험치를 0으로 만들고, 그 경험치 만큼 다른 타일에 적용";
            
            Board.Tile senderTile = null;
            Board.Tile receiverTile = null;

            // sender 타일 선택
            do
            {
                yield return SelectTile(TileSelectionType.Single);
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
                yield return SelectTile(TileSelectionType.Single);
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
            _explainTMP.text = $"라인 하나에 경험치를 {value}씩 제공";
            
            yield return SelectTile(TileSelectionType.Edge);

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
            _explainTMP.text = $"아래 횟수만큼 원하는 타일에 경험치를 적용";
            
            int cnt = 0;
            do
            {
                _addTextTMP.text = $"{cnt}/{totalExp}";
                    
                // 타일 지정
                yield return SelectTile(TileSelectionType.Single);
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
            
            yield return new WaitForSeconds(1f);
        }

        /// <summary>
        /// 원하는 타일의 속성을 변경
        /// </summary>
        /// <returns></returns>
        IEnumerator ChangeTileType()
        {
            _explainTMP.text = $"원하는 타일의 속성을 변경";

            Board.Tile tile;
            do
            {
                yield return SelectTile(TileSelectionType.Single);
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
            _explainTMP.text = $"원하는 속성 타일들에 경험치 {value}씩 제공";
            
            // _typeSelectObj 에 현재 플레이어가 보유한 타입만 설정 필요
            var typeList = 
                GameManager.I.Stage.Board.TileSequence
                    .Select(t => t.TileMagic.Type)
                    .Where(type => type != TileMagicType.None );
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
