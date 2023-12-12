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

        private Vector2 _diceUIPos;
        private Dice _dice;
        private DiceManager _diceManager;
        ComponentGetter<Image> _image
                = new ComponentGetter<Image>(TypeOfGetter.This);
        [SerializeField] private TextMeshProUGUI _numberText;
        private DiceAnimation _diceAnimation;
        private DiceDescription _diceDescription;

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
                    //_image.Get(gameObject).color = Color.white;
                }
                else
                {
                    //_image.Get(gameObject).color = Color.gray;
                }
            }
        }

        public bool CanAction
        {
            get => _canAction;
            set
            {
                _canAction = value;
                if (GameManager.I.Stage.Board.IsBoardSquare) {
                    if (GameManager.I.Player.OnTile.Type == TileType.Start || GameManager.I.Player.OnTile.Type == TileType.Blank) {
                        _canAction = false;
                    }
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
        public DiceAnimation DiceAnimation => _diceAnimation;
        public DiceDescription DiceDescription => _diceDescription;
        public void Init(Dice dice, int index, DiceManager diceManager)
        {
            _dice = dice;
            _diceIndex = index;
            _diceManager = diceManager;
            _isSelectable = true;
            _diceUIPos = (transform as RectTransform).anchoredPosition;
            _diceAnimation = GetComponent<DiceAnimation>();
            _diceDescription = GetComponent<DiceDescription>();

            string path = "Dice/Dice_" + _dice.DiceType.ToString() + "_" + "1";
            _image.Get(gameObject).sprite = ResourceLoader.LoadSprite(path);

        }

        public void EnableCardUI()
        {
            IsDiscard = false;
            GetComponent<RectTransform>().anchoredPosition = _diceUIPos;
            transform.localScale = new Vector3(1, 1, 1);
            gameObject.SetActive(true);
        }

        public void UpdateDiceUI(int number)
        {
            string path = "Dice/Dice_" + _dice.DiceType.ToString() + "_" + number.ToString();
            _image.Get(gameObject).sprite = ResourceLoader.LoadSprite(path);
        }
   
        public void DismissDiceUI()
        {
            IsSelect = false;
            transform.localScale = new Vector3(1, 1, 1);
            GetComponent<RectTransform>().anchoredPosition = _diceUIPos;
            DiceDescription.SetDescriptionUIRestored();

        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if(eventData.button== PointerEventData.InputButton.Left)
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
                DiceDescription.SetDescriptionUIRestored();
                StartCoroutine(_diceManager.Dragging());
            }

            if(eventData.button == PointerEventData.InputButton.Right)
            {
                _diceManager.Roll(_diceIndex);
            }
            
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
            if (!_isDiscard && _isSelectable&&!_isSelect)
            {
                (transform as RectTransform).DOAnchorPosY(_diceUIPos.y + 15f, 0.1f);
                DiceDescription.SetDescriptionUIHovered(Dice.RollResultIndex);
            }

        }

        private void SetCardUIRestore()
        {
            if (!_isDiscard && _isSelectable)
            {
                (transform as RectTransform).DOAnchorPosY(_diceUIPos.y, 0.1f);
                DiceDescription.SetDescriptionUIRestored();
            }

        }

        private void Update()
        {
            MoveCardUI();
        }
    }
}

