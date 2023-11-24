using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cardinals.Enums;
using TMPro;
using Util;

namespace Cardinals
{
    public class CardUI : MonoBehaviour, IPointerDownHandler
    {
        private int _cardIndex;
        [SerializeField] private TextMeshProUGUI _numberText;
        [SerializeField] private GameObject _volatileText;
        [SerializeField] private bool _isSelect;
        private Card _card;
        private CardManager _cardManager;
        public bool IsSelect
        {
            set
            {
                _isSelect = value;
            }
        }
        public Card Card => _card;
        public int CardIndex
        {
            get => _cardIndex;
            set
            {
                _cardIndex = value;
            }
        }
        public void Init(Card card, int index, CardManager cardManager)
        {
            _card = card;
            _cardIndex = index;
            _cardManager = cardManager;
            Image image = GetComponent<Image>();
            _numberText.text = card.CardNumber.ToString();
            _volatileText.SetActive(card.IsVolatile);

            switch (card.CardNumber)
            {
                case 1:
                    image.sprite = ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_Card_Basic1);
                    break;
                case 2:
                    image.sprite = ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_Card_Basic2);
                    break;
                case 3:
                    image.sprite = ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_Card_Basic3);
                    break;
                case 4:
                    image.sprite = ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_Card_Basic4);
                    break;
                case 5:
                    image.sprite = ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_Card_Basic5);
                    break;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_cardManager.State != CardState.Idle)
                return;
            _cardManager.SelectCardIndex = _cardIndex;
            _isSelect = true;
            _cardManager.State = CardState.Select;
            StartCoroutine(_cardManager.Dragging());

        }

        private void MoveCardUI()
        {
            if (_isSelect)
            {
                transform.position = Input.mousePosition;
            }
        }
        private void Update()
        {
            MoveCardUI();
        }
    }
}

