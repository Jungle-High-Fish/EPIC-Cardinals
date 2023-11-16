using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Cardinals
{
    public class CardManager : MonoBehaviour
    {
        [SerializeField] private Player _player;
        private int _prevCardNumber=-1;
        private int _selectCardIndex;
        [SerializeField] private int _drawCardCount;
        private bool _canActionUse;
        private CardState _state;
        [SerializeField] private MouseState _mouseState = MouseState.Cancel;

        [ShowInInspector] private List<Card> _drawPile;
        [ShowInInspector] private List<Card> _handCards;
        [ShowInInspector] private List<Card> _discardPile;
        private List<CardUI> _handcardsUI;

        [SerializeField] private Transform _cardDeckUIParent;
        [SerializeField] private GameObject _cardUIPrefab;

        private void Start()
        {
            Init();
        }
        private void Init()
        {
            _drawPile = new();
            _handCards = new();
            _handcardsUI = new();
            _discardPile = new();
            //테스트용
            Card card1 = new Card(1, false);
            Card card2 = new Card(2, false);
            Card card3 = new Card(3, false);
            Card card4 = new Card(1, false);
            Card card5 = new Card(4, false);
            Card card6 = new Card(5, false);
            _drawPile.Add(card1);
            _drawPile.Add(card2);
            _drawPile.Add(card3);
            _drawPile.Add(card4);
            _drawPile.Add(card5);
            _drawPile.Add(card6);
        }

        public int SelectCardIndex
        {
            set
            {
                _selectCardIndex = value;
            }
        }

        public CardState State
        {
            get => _state;
            set
            {
                _state = value;
            }
        }
        public MouseState MouseState
        {
            set
            {
                _mouseState = value;
            }
        }

        [Button]
        public void OnTurn()
        {
            Draw(_drawCardCount);
            _canActionUse = false;
        }

        [Button]
        public void EndTurn()
        {
            int count = _handCards.Count;
            for(int i = 0; i < count; i++)
            {
                Discard(0);
            }
        }

        [Button]
        public void Draw(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (_drawPile.Count == 0)
                {
                    Shuffle();
                }

                int randIndex = Random.Range(0, _drawPile.Count);
                Card card = _drawPile[randIndex];

                if (_handCards.Count == 0)
                {
                    _handCards.Add(card);
                    UpdateCardUI(card, 0);
                    _drawPile.Remove(card);
                    continue;
                }

                for (int j = 0; j < _handCards.Count; j++)
                {
                    
                    if (_handCards[j].CardNumber >= card.CardNumber)
                    {
                        _handCards.Insert(j, card);
                        UpdateCardUI(card, j);
                        break;
                    }

                    if (j == _handCards.Count - 1)
                    {
                        _handCards.Add(card);
                        UpdateCardUI(card, j + 1);
                        break;
                    }
                }
                _drawPile.Remove(card);

                UpdateCardIndex();
            }

        }
        private void Discard(int index)
        {
            Destroy(_handcardsUI[index].gameObject);
            _handcardsUI.RemoveAt(index);

            Card card = _handCards[index];
            _discardPile.Add(card);
            _handCards.Remove(card);
            UpdateCardIndex();
        }

        [Button]
        private void Shuffle()
        {
            int count = _discardPile.Count;
            for (int i = 0; i < count; i++)
            {
                Card card = _discardPile[0];
                _drawPile.Add(card);
                _discardPile.Remove(card);
            }
        }
        private void UpdateCardUI(Card card, int index)
        {
            GameObject cardUI = Instantiate(_cardUIPrefab, _cardDeckUIParent);
            cardUI.GetComponent<CardUI>().Init(card, index, this);
            cardUI.transform.SetSiblingIndex(index);
            _handcardsUI.Insert(index, cardUI.GetComponent<CardUI>());

        }

        public IEnumerator Dragging()
        {
            while (_state == CardState.Select)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    switch (_mouseState)
                    {
                        case MouseState.Action:
                            if (!CardUseAction(_handCards[_selectCardIndex].CardNumber))
                            {
                                break;
                            }
                            Discard(_selectCardIndex);
                            _state = CardState.Idle;
                            yield break;

                        case MouseState.Move:
                            CardUseMove(_handCards[_selectCardIndex].CardNumber);
                            Discard(_selectCardIndex);
                            _state = CardState.Idle;
                            _prevCardNumber = -1;
                            _canActionUse = true;
                            yield break;
                    }

                    _handcardsUI[_selectCardIndex].IsSelect = false;
                    _handcardsUI[_selectCardIndex].gameObject.SetActive(false);
                    _handcardsUI[_selectCardIndex].gameObject.SetActive(true);
                    _state = CardState.Idle;
                    yield break;
                }
                yield return null;
            }
        }
        private void CardUseMove(int num)
        {
            StartCoroutine(_player.MoveTo(num,0.4f));
        }

        private bool CardUseAction(int num)
        {
            if (!_canActionUse)
            {
                return false;
            }

            if(_prevCardNumber == -1 || _prevCardNumber + 1 == num)
            {
                Debug.Log($"카드 액션에 {_handCards[_selectCardIndex].CardNumber} 사용");
                //타일에서 액션 호출
                _prevCardNumber = num;
                return true;
            }

            return false;

        }

        private void UpdateCardIndex()
        {
            for (int i = 0; i < _handCards.Count; i++)
            {
                _handcardsUI[i].CardIndex = i;
            }
        }
    }

}
