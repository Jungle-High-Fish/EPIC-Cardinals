using System;
using Cardinals.Enums;
using Cardinals.Game;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI.NewDice
{
    public class UINewDicePanel : MonoBehaviour
    {
        [Header("Component")] 
        [SerializeField] private TextMeshProUGUI _headerTMP;
        [SerializeField] private TextMeshProUGUI _changedDiceTMP;
        [SerializeField] Button _tradeBTN;
        [SerializeField] Button _rerollBTN;
        [SerializeField] Button _cancelBTN;

        [Header("Effect")]
        [SerializeField] private GameObject _newDiceCoverObj;
        [SerializeField] private Transform _newDiceCoverIcon;
        [SerializeField] private Transform _newDiceInfoTr;
        private Vector3 _backupCoverPos;
        private Vector3 _backupIconPos;

        
        [Header("Info")]
        [SerializeField] private UIDice _newUIDice; 
        [SerializeField] private Transform _curDiceParentTr;

        private Dice _newDice; 
        private UIDice _selectedUIDice;
        private Action _tradeEvent;
        private bool _hasRerolled;
        public bool HasRerolled
        {
            set => _hasRerolled = value;
        }
        
        void Start()
        {
            _tradeBTN.onClick.AddListener(B_Trade);
            _rerollBTN.onClick.AddListener(B_Reroll);
            _cancelBTN.onClick.AddListener(B_Cancel);
            _changedDiceTMP.text = GameManager.I.Localization.Get(LocalizationEnum.UI_GETDICE);
            _headerTMP.text = GameManager.I.Localization.Get(LocalizationEnum.UI_DICE_TITLE);
            _tradeBTN.GetComponentInChildren<TextMeshProUGUI>().text
                = GameManager.I.Localization[LocalizationEnum.UI_DICE_TRADE];
            _rerollBTN.GetComponentInChildren<TextMeshProUGUI>().text
                = GameManager.I.Localization[LocalizationEnum.MAGIC_LEVELUP_BT3];

            if (GameManager.I.Localization.IsJapanese)
            {
                _rerollBTN.GetComponentInChildren<TextMeshProUGUI>().fontSize *= .7f;
            }

            _backupCoverPos = _newDiceCoverObj.transform.position;
            _backupIconPos = _newDiceCoverIcon.position;
        }

        
        [Button]
        public void Init(Dice newDice, Action tradeAction)
        {
            gameObject.SetActive(true);
            _tradeEvent = tradeAction;
            

            if (_newDice != newDice && !_hasRerolled)
            {
                _rerollBTN.interactable = false;
                _newDice = newDice;
                _newUIDice.Init(newDice);

                // 애니메이션 재생
                GameManager.I.Sound.CardReroll();
                _newDiceCoverObj.SetActive(true);
                _newDiceCoverObj.transform.DOShakePosition(.6f, 10, 20);
                _newDiceCoverIcon.DOPunchPosition(new Vector3(10, 10), .6f)
                    .OnComplete(() =>
                    {
                        _rerollBTN.interactable = true;
                        _newDiceInfoTr.transform.localScale = Vector3.zero;
                        _newDiceCoverObj.SetActive(false);
                        _newDiceInfoTr.transform.DOScale(1, 0.1f).SetEase(Ease.InQuad)
                            .OnComplete(() =>
                            {
                                _newDiceCoverObj.transform.localScale = Vector3.one;
                                _newDiceCoverObj.transform.position = _backupCoverPos;
                                _newDiceCoverIcon.localScale = Vector3.one;
                                _newDiceCoverIcon.position = _backupIconPos;
                            });
                    });
            }
            
            
            // 초기화 기존 선택 전체 취소 및 초기화
            for (int i = 0, cnt = _curDiceParentTr.childCount; i < cnt; i++)
            {
                var dice = GameManager.I.Stage.DiceManager.Dices[i];
                _curDiceParentTr.GetChild(i).GetComponent<UIDice>().Init(dice);
                _curDiceParentTr.GetChild(i).GetComponent<SelectedEffect>().IsSelected = false;
            }

            _selectedUIDice = null;
            _tradeBTN.interactable = _selectedUIDice is not null;
        }

        public void RerollRewardDice(Dice newDice, Action tradeAction)
        {
            gameObject.SetActive(true);
            _tradeEvent = tradeAction;

            _rerollBTN.interactable = false;
            _newDice = newDice;
            _newUIDice.Init(newDice);

            // 애니메이션 재생
            GameManager.I.Sound.CardReroll();
            _newDiceCoverObj.SetActive(true);
            _newDiceCoverObj.transform.DOShakePosition(.6f, 10, 20);
            _newDiceCoverIcon.DOPunchPosition(new Vector3(10, 10), .6f)
                .OnComplete(() =>
                {
                    _rerollBTN.interactable = true;
                    _newDiceInfoTr.transform.localScale = Vector3.zero;
                    _newDiceCoverObj.SetActive(false);
                    _newDiceInfoTr.transform.DOScale(1, 0.1f).SetEase(Ease.InQuad)
                        .OnComplete(() =>
                        {
                            _newDiceCoverObj.transform.localScale = Vector3.one;
                            _newDiceCoverObj.transform.position = _backupCoverPos;
                            _newDiceCoverIcon.localScale = Vector3.one;
                            _newDiceCoverIcon.position = _backupIconPos;
                        });
                });


            // 초기화 기존 선택 전체 취소 및 초기화
            for (int i = 0, cnt = _curDiceParentTr.childCount; i < cnt; i++)
            {
                var dice = GameManager.I.Stage.DiceManager.Dices[i];
                _curDiceParentTr.GetChild(i).GetComponent<UIDice>().Init(dice);
                _curDiceParentTr.GetChild(i).GetComponent<SelectedEffect>().IsSelected = false;
            }

            _selectedUIDice = null;
            _tradeBTN.interactable = _selectedUIDice is not null;
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
            _tradeBTN.interactable = _selectedUIDice is not null;
            
        }
        
        void B_Trade()
        {
            int idx = default;
            for (int i = 0, cnt = _curDiceParentTr.childCount; i < cnt; i++)
            {
                if (_selectedUIDice.transform == _curDiceParentTr.GetChild(i))
                {
                    idx = i;
                    break;
                }
            }
            GameManager.I.Sound.GetDice();
           GameManager.I.Stage.DiceManager.ChangeDice(idx, _newDice);
           _tradeEvent?.Invoke();
           B_Cancel();
           _hasRerolled = false;
        }

        void B_Cancel()
        {
            gameObject.SetActive(false);
            
        }

        void B_Reroll()
        {
            if (GameManager.I.Player.PlayerInfo.Gold < 2)
            {
                _rerollBTN.transform.DOShakePosition(0.6f);
                return;
            }
            GameManager.I.Player.PlayerInfo.UseGold(2);
            EnemyGradeType type = (GameManager.I.Stage.CurEvent as BattleEvent).EnemyGrade;
            Dice newDice=GameManager.I.Stage.GetRewardDice(type);
            RerollRewardDice(newDice, _tradeEvent);
            _hasRerolled = true;

        }
    }
}