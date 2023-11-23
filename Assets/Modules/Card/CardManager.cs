using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Util;
using Cardinals.Enums;

namespace Cardinals
{
    public class CardManager : MonoBehaviour
    {
        private int _prevCardNumber=-1;
        private int _selectCardIndex;
        private bool _canActionUse;
        private int _continuousUseCount;
        private CardState _state;
        [SerializeField] private MouseState _mouseState = MouseState.Cancel;

        [ShowInInspector] private List<Card> _drawPile;
        [ShowInInspector] private List<Card> _handCards;
        [ShowInInspector] private List<Card> _discardPile;
        private List<CardUI> _handcardsUI;

        public IEnumerable<Card> HandCards => _handCards;

        [SerializeField] private Transform _cardDeckUIParent;

        public void Init()
        {
            _drawPile = new();
            _handCards = new();
            _handcardsUI = new();
            _discardPile = new();

            AddCard(1, false, CardPileType.DrawPile);
            AddCard(1, false, CardPileType.DrawPile);
            AddCard(2, false, CardPileType.DrawPile);
            AddCard(3, false, CardPileType.DrawPile);
            AddCard(4, false, CardPileType.DrawPile);
            AddCard(5, false, CardPileType.DrawPile);
        }

        public void SetCardDeckUIParent(Transform parent)
        {
            _cardDeckUIParent = parent;
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

        public bool IsElectricShock { get; set; }

        [Button]
        public void OnTurn()
        {
            int drawCardOffset = 0;
            if (GameManager.I.Player.PlayerInfo.CheckArtifactExist(Enums.ArtifactType.Grimoire))
            {
                drawCardOffset = 1;
            }
            Draw(Constants.GameSetting.Player.CardDrawCount+drawCardOffset);
            _canActionUse = false;
            _continuousUseCount = 0;
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
        public void AddCard(int num, bool isVolatile, CardPileType cardPileType)
        {
            Card card = new Card(num, isVolatile);
            switch (cardPileType)
            {
                case CardPileType.DrawPile:
                    _drawPile.Add(card);
                    break;

                case CardPileType.Hand:
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
                    UpdateCardIndex();
                    break;

                case CardPileType.DiscardPile:
                    _discardPile.Add(card);
                    break;

                default:
                    Debug.Log("카드 추가 실패");
                    break;
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
                    if (_drawPile.Count == 0)
                    {
                        Debug.Log("드로우 실패");
                        return;
                    }
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
            if (!card.IsVolatile)
            {
                _discardPile.Add(card);
            }
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
            GameObject cardUIPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Card);
            GameObject cardUI = Instantiate(cardUIPrefab, _cardDeckUIParent);
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
                        
                        case MouseState.CardEvent:
                            GameManager.I.UI.UICardEvent.SelectedCard(_handCards[_selectCardIndex].CardNumber);
                            Discard(_selectCardIndex);
                            _state = CardState.Idle;
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
            StartCoroutine(GameManager.I.Player.MoveTo(num,0.4f));
        }

        private IEnumerator WarpArtifact()
        {
            yield return new WaitForSeconds(2f);
            CardUseMove(1);
        }
        private bool CardUseAction(int num, BaseEntity target=null)
        {
            if (!_canActionUse)
            {
                return false;
            }

            if(GameManager.I.Player.CheckBuffExist(BuffType.ElectricShock) && _continuousUseCount >= 2)
            {
                Debug.Log("뭐지 감전당했나?");
                return false;
            }
            if(_prevCardNumber == -1 || _prevCardNumber + 1 == num)
            {
                // [유물] 워프 부적
                if (GameManager.I.Player.PlayerInfo.CheckArtifactExist(Enums.ArtifactType.Warp)
                    && num==4)
                {
                    StartCoroutine(WarpArtifact());
                }
                _prevCardNumber = num;

                StartCoroutine(GameManager.I.Player.CardAction(num, target));
                _continuousUseCount++;
                Debug.Log(_continuousUseCount);
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
