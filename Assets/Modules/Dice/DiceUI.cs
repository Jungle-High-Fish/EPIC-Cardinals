using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;
using UnityEngine.UI;
using Cardinals.Enums;
using DG.Tweening;
using TMPro;

namespace Cardinals
{
    public class DiceUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private int _diceIndex;
        private bool _canAction;
        private bool _canMove;
        private bool _isSelect;
        private bool _isSelectable;
        private bool _isDiscard = false;

        private Vector2 _DiceUIPos;
        private Dice _dice;
        private DiceManager _diceManager;
        ComponentGetter<Image> _image
                = new ComponentGetter<Image>(TypeOfGetter.This);
        [SerializeField] private TextMeshProUGUI _numberText;

        public bool IsSelect
        {
            set => _isSelect = value;
        }

        public bool IsDiscard
        {
            set => _isDiscard = value;
        }

        public bool IsSelectable
        {
            get => _isSelectable;
            set
            {
                _isSelectable = value;
                if (_isSelectable)
                {
                    _image.Get(gameObject).color = Color.white;
                }
                else
                {
                    _image.Get(gameObject).color = Color.gray;
                }
            }
        }

        public bool CanAction
        {
            get => _canAction;
            set
            {
                _canAction = value;
                if (GameManager.I.Player.OnTile.Type == TileType.Start || GameManager.I.Player.OnTile.Type == TileType.Blank)
                {
                    _canAction = false;
                }
            }
        }

        public bool CanMove
        {
            get => _canMove;
            set
            {
                _canMove = value;
            }
        }
        public Dice Dice => _dice;

        public void Init(Dice dice, int index, DiceManager diceManager)
        {
            _dice = dice;
            _diceIndex = index;
            _diceManager = diceManager;
            _isSelectable = true;
            Image image = GetComponent<Image>();
            _DiceUIPos = (transform as RectTransform).anchoredPosition;
        }

        public void UpdateDiceUI(int number)
        {
            _numberText.text = number.ToString();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_diceManager.State != CardState.Idle)
                return;
            if (!_isSelectable)
                return;

            transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.1f);
            _diceManager.SelectCardIndex = _diceIndex;
            _isSelect = true;
            _diceManager.State = CardState.Select;

            GameManager.I.Sound.CardClick();

            StartCoroutine(_diceManager.Dragging());
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            SetCardUIHovered();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetCardUIRestore();
        }

        public void StartDraggingState()
        {
            _image.Get(gameObject).raycastTarget = false;
        }

        public void ClickDismiss()
        {
            _image.Get(gameObject).raycastTarget = true;
        }

        private void MoveCardUI()
        {
            if (_isSelect)
            {
                transform.position = Input.mousePosition;
            }
        }

        private void SetCardUIHovered()
        {
            if (!_isDiscard && _isSelectable)
            {
                (transform as RectTransform).DOAnchorPosY(_DiceUIPos.y + 5, 0.1f);
            }

        }

        private void SetCardUIRestore()
        {
            if (!_isDiscard && _isSelectable)
            {
                (transform as RectTransform).DOAnchorPosY(_DiceUIPos.y, 0.1f);
            }

        }

        private void Update()
        {
            MoveCardUI();
        }
    }
}

