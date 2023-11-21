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
    public class UICardEvent : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] [ReadOnly] private CardEventDataSO _curEvent;
        [SerializeField] private CardEventDataSO[] _eventData;
        
        [Header("Component")]
        [SerializeField] private GameObject _eventObj;
        [SerializeField] private TextMeshProUGUI _eventGuideTMP;
        
        [Header("End Component")]
        [SerializeField] private GameObject _endObj;
        [SerializeField] private TextMeshProUGUI _endTextTMP;
        [SerializeField] private Button _closeBTN;

        public void Awake()
        {
            _closeBTN.onClick.AddListener(Close);
        }

        [Button]
        public void Init()
        {
            gameObject.SetActive(true);
            _eventObj.SetActive(true);
            _endObj.SetActive(false);
            
            // 랜덤 이벤트 지정
            _curEvent = _eventData[Random.Range(0, _eventData.Length)];
            _eventGuideTMP.text = _curEvent.description;
            
            if (GameManager.I.Stage.CardManager.HandCards.Count() == 0) // 카드가 없으면
            {
                // 이벤트 생략
                EndEvent("이벤트를 수행하기 위한 카드가 부족합니다.");
            }
        }

        public void SelectedCard(int value)
        {
            switch (_curEvent.type)
            {
                case BoardEventCardType.Draw:
                    GameManager.I.Stage.DrawCard(value);
                    break;
                case BoardEventCardType.CopyOneTimeCard:
                    // [TODO] 일회성 카드 복제 기능 구현된 후, 연결 필요
                    Debug.Log($"카드가 {value}만큼 복제됨. (사실은 안됨)");
                    break;
                case BoardEventCardType.Heal:
                    GameManager.I.Stage.Player.Heal( value * 2);
                    break;
                case BoardEventCardType.Money:
                    GameManager.I.Stage.AddGold( value * 20);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            EndEvent(_curEvent.endText);
        }

        void EndEvent(string text)
        {
            _curEvent = null;
            
            _endTextTMP.text = text;
            
            _eventObj.SetActive(false);
            _endObj.SetActive(true);
        }

        void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
