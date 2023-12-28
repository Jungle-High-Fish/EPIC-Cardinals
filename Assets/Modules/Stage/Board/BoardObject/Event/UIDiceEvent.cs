using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cardinals.Enums;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Cardinals.BoardEvent.Card
{
    /// <summary>
    /// 보드 위에서 실행되는 카드 이벤트
    /// </summary>
    public class UIDiceEvent : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] [ReadOnly] private CardEventDataSO _curEvent;
        [SerializeField] private CardEventDataSO[] _eventData;
        
        [Header("Component")]
        [SerializeField] private GameObject _eventObj;
        [SerializeField] private TextMeshProUGUI _eventGuideTMP;
        
        [Header("End Component")]
        [SerializeField] private GameObject _endObj;
        [SerializeField] private GridSizeUpdator _updator;
        [SerializeField] private TextMeshProUGUI _endTextTMP;
        [SerializeField] private Button _closeBTN;

        public void Awake()
        {
            _closeBTN.onClick.AddListener(Close);
        }

        [Button]
        public void Init()
        {
            GameManager.I.UI.UIEndTurnButton.Deactivate();
            gameObject.SetActive(true);
            _eventObj.SetActive(true);
            _endObj.SetActive(false);
            
            // 랜덤 이벤트 지정
            _curEvent = _eventData[Random.Range(0, _eventData.Length)];
            _eventGuideTMP.text = _curEvent.description;

            Invoke(nameof(CheckDiceExist), 1f);
        }

        void CheckDiceExist()
        {
            if (GameManager.I.Stage.DiceManager.DiceUis.All(d => !d.gameObject.activeSelf))
            {
                // 이벤트 생략
                StartCoroutine(nameof(EndEvent), GameManager.I.Localization.Get(LocalizationEnum.EVENT_DICEEVENT_WARNING));
            }
        }

        public void SelectedCard(int value)
        {
            switch (_curEvent.type)
            {
                // case BoardEventCardType.Draw:
                //     GameManager.I.Stage.CardManager.Draw(value);
                //     break;
                // case BoardEventCardType.CopyOneTimeCard:
                //     GameManager.I.Stage.CardManager.AddCard(value, true, CardPileType.Hand);
                //     GameManager.I.Stage.CardManager.AddCard(value, true, CardPileType.Hand);
                //     break;
                case BoardEventCardType.Heal:
                    GameManager.I.Stage.Player.Heal( value * 2);
                    break;
                case BoardEventCardType.Money:
                    GameManager.I.Stage.AddGold(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            StartCoroutine(nameof(EndEvent), _curEvent.endText);
        }

        IEnumerator EndEvent(string text)
        {
            // 초기화
            GameManager.I.UI.UIEndTurnButton.Activate();
            _eventObj.SetActive(false);
            _curEvent = null;
            
            _endTextTMP.text = text;
            _endObj.SetActive(true);
            _updator.Resizing();
            
            yield return new WaitForSeconds(1f);
            Close();
        }

        void Close()
        {
            gameObject.SetActive(false);
            GameManager.I.UI.UIEndTurnButton.Activate();
        }
    }
}
