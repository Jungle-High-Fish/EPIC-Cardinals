using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cardinals.Enums;
using Cardinals.UI.NewDice;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;
using Random = UnityEngine.Random;

namespace Cardinals.BoardEvent.Alchemy
{
    public class UIAlchemyEventPanel : MonoBehaviour
    {
        [Header("Component")] 
        [SerializeField] private TextMeshProUGUI _headerTMP;
        [SerializeField] private TextMeshProUGUI _chooseDiceDescTMP;
        [SerializeField] Button _rollingBTN;
        [SerializeField] Button _cancelBTN;
        [SerializeField] private GameObject _rollingShield;
        [SerializeField] private GameObject _resultObj;
        [SerializeField] private Image _resultImg;
        
        [Header("Info")]
        [SerializeField] private TextMeshProUGUI _eventDescriptionTMP;
        [SerializeField] private TextMeshProUGUI _targetNumberTMP;
        [SerializeField] private Transform _curDiceParentTr;
        private UIDice _selectedUIDice;
        private AlchemyEventDataSO _evtData;
        private bool _isRolling;

        [Header("EventValue")]
        [SerializeField] private int _allTileExpValue;
        [SerializeField] private int _upMaxHpValue;
        [SerializeField] private int _getMoneyValue;
        [SerializeField] private int _damage;
        
        void Start()
        {
            _rollingBTN.onClick.AddListener(B_Rolling);
            _cancelBTN.onClick.AddListener(B_Cancel);
        }
        
        public IEnumerator Init()
        {
            gameObject.SetActive(true);
            _rollingShield.SetActive(false);
            _resultObj.SetActive(false);
            
            // 초기화 기존 선택 전체 취소 및 초기화
            for (int i = 0, cnt = _curDiceParentTr.childCount; i < cnt; i++)
            {
                var dice = GameManager.I.Stage.DiceManager.Dices[i];
                _curDiceParentTr.GetChild(i).GetComponent<UIDice>().Init(dice);
                _curDiceParentTr.GetChild(i).GetComponent<SelectedEffect>().IsSelected = false;
            }

            _selectedUIDice = null;
            _rollingBTN.interactable = _selectedUIDice is not null;
            
            // 이벤트 설정
            var idx = Random.Range(1, Enum.GetNames(typeof(BoardEventAlchemyType)).Length);
            var evt = (BoardEventAlchemyType)idx;
            _evtData = ResourceLoader.LoadSO<AlchemyEventDataSO>(Constants.FilePath.Resources
                .SO_BoardEvent_AlchemyEventData + evt);
            _targetNumberTMP.text = idx.ToString();
            _eventDescriptionTMP.text = _evtData.eventDescription;
            
            // 버튼 입력 대기
            _isRolling = false;
            yield return new WaitUntil(() => _isRolling);
            yield return Rolling();
        }

        private void B_Rolling() => _isRolling = true;
        
        IEnumerator Rolling()
        {
            _rollingShield.SetActive(true);
            
            // 돌림
            var idx = Random.Range(0, _selectedUIDice.Data.DiceNumbers.Count);
            var resultNumber = _selectedUIDice.Data.DiceNumbers[idx];

            yield return new WaitForSeconds(1f);

            // 결과 처리
            _resultObj.SetActive(true);
            bool result = (int)_evtData.type == resultNumber;
            var path = result
                ? Constants.FilePath.Resources.Sprites_UI_Result_Success
                : Constants.FilePath.Resources.Sprites_UI_Result_Fail;
            _resultImg.sprite = ResourceLoader.LoadSprite(path);
            _resultImg.transform.DOPunchScale(Vector3.one, .2f, 1, 1);
            
            if (result)
            {
                switch (_evtData.type)
                {
                    case BoardEventAlchemyType.GetMoney:
                        GameManager.I.Stage.AddGold(_getMoneyValue);
                        break;
                    case BoardEventAlchemyType.GetRandomPotion:
                        GameManager.I.Stage.AddRandomPotion();
                        break;
                    case BoardEventAlchemyType.AllTileExp:
                        foreach (var tile in GameManager.I.Stage.Board.TileSequence)
                        {
                            tile.TileMagic.GainExp(_allTileExpValue);
                        }
                        break;
                    case BoardEventAlchemyType.MaxHpUp:
                        var maxHp = GameManager.I.Player.MaxHp + _upMaxHpValue; 
                        GameManager.I.Player.SetMaxHP(maxHp);
                        break;
                    case BoardEventAlchemyType.DamageAllEnemy:
                        var list = GameManager.I.CurrentEnemies.ToList();
                        for (int i = list.Count - 1; i >= 0; i--)
                        {
                            GameManager.I.Player.Attack(list[i], _damage);
                        }
                        break;
                }
            }
            
            yield return new WaitForSeconds(1.5f);
            B_Cancel();
        }
        
        public void SelectedItem(UIDice dice)
        {
            if (!gameObject.activeSelf) return;
            
            // 기존 선택한 항목이 존재한다면 선택 취소 처리
            if (_selectedUIDice is not null)
            {
                _selectedUIDice.GetComponent<SelectedEffect>().IsSelected = false;
                _selectedUIDice = null;
            }

            _selectedUIDice = dice;
            _rollingBTN.interactable = _selectedUIDice is not null;
        }
        
        void B_Cancel()
        {
            gameObject.SetActive(false);
        }
    }
}