using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using Util;
using Cardinals.Enums;
using UnityEngine.UI;
using Unity.VisualScripting;
using Cardinals.Board;
using Cardinals.Tutorial;

namespace Cardinals
{
    public class CardManager : MonoBehaviour
    {
        private int _prevCardNumber=-1;
        private int _selectCardIndex;
        private bool _canActionUse;
        private bool _lastCardUsedForAction;
        private bool _isMouseOnCardDeck;
        private int _continuousUseCount;
        private int _cardUsedCountOnThisTurn;

        #region Tutorial
        private bool _isTutorial;
        #endregion

        private CardState _state;
        private MouseState _mouseState = MouseState.Cancel;

        [SerializeField] private bool _newCardUseMod;
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
            _newCardUseMod = true;
            AddCard(1, false, CardPileType.DrawPile);
            AddCard(1, false, CardPileType.DrawPile);
            AddCard(1, false, CardPileType.DrawPile);
            AddCard(1, false, CardPileType.DrawPile);
            AddCard(2, false, CardPileType.DrawPile);
            AddCard(2, false, CardPileType.DrawPile);
            AddCard(2, false, CardPileType.DrawPile);
            AddCard(3, false, CardPileType.DrawPile);
            AddCard(3, false, CardPileType.DrawPile);
            AddCard(3, false, CardPileType.DrawPile);
            AddCard(4, false, CardPileType.DrawPile);
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
            if (!_lastCardUsedForAction&&_newCardUseMod)
            {
                _canActionUse = true;
            }
            
            _cardUsedCountOnThisTurn = 0;
            _continuousUseCount = 0;
            UpdateCardState(-1, true);
        }

        [Button]
        public void EndTurn()
        {
            StartCoroutine(DiscardAll(0, CardAnimationType.TurnEnd));
        }

        public void OnBattle(bool isTutorial=false)
        {
            _lastCardUsedForAction = false;
            _prevCardNumber = -1;

            _isTutorial = isTutorial;
            Debug.Log("배틀 시작");
        }

        public void EndBattle()
        {
            EndTurn();
            Debug.Log("배틀 끝");
        }

        public void DrawHandDecksForTutorial(int[] cardNumbers) {
            _lastCardUsedForAction = false;
            _prevCardNumber = -1;
            
            for (int i = 0; i < cardNumbers.Length; i++) {
                AddCard(cardNumbers[i], true, CardPileType.Hand);
            }

            _canActionUse = false;
            if (!_lastCardUsedForAction&&_newCardUseMod)
            {
                _canActionUse = true;
            }
            
            _cardUsedCountOnThisTurn = 0;
            _continuousUseCount = 0;

            UpdateCardState(-1, true);
        }

        public void SetCardSelectable(bool isSelectable)
        {
            foreach(CardUI c in _handcardsUI)
            {
                c.IsSelectable = isSelectable;
            }
        }

        public void SetCardMouseState(bool isOnCardDeck) {
            _isMouseOnCardDeck = isOnCardDeck;
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

        private IEnumerator DiscardAll(int index, CardAnimationType animationType)
        {
            yield return new WaitForSeconds(0.6f);
            for (int i = 0; i < _handCards.Count; i++)
            {
                StartCoroutine(_handcardsUI[i].CardAnim.Play(animationType));
            }
                
            yield return new WaitForSeconds(0.4f);
            int count = _handCards.Count;
            for (int i=0;i< count; i++)
            {
                Destroy(_handcardsUI[0].gameObject);
                _handcardsUI.RemoveAt(0);

                Card card = _handCards[0];
                if (!card.IsVolatile)
                {
                    _discardPile.Add(card);
                }
                _handCards.Remove(card);
                UpdateCardIndex();
            }
           
        }

        private IEnumerator Discard(int index, CardAnimationType animationType)
        {
            _handcardsUI[index].IsDiscard = true;
            _handcardsUI[index].IsSelect = false;
            yield return _handcardsUI[index].CardAnim.Play(animationType);
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
            cardUI.transform.SetSiblingIndex(index);
            Canvas.ForceUpdateCanvases();
            cardUI.GetComponent<CardUI>().Init(card, index, this);
            
            _handcardsUI.Insert(index, cardUI.GetComponent<CardUI>());

        }

        public IEnumerator Dragging()
        {
            foreach (CardUI c in _handcardsUI)
            {
                if (c == null)
                    continue;
                c.StartDraggingState();
            }

            while (_state == CardState.Select)
            { 
                if (Input.GetMouseButtonUp(0))
                {
                    BoardInputHandler boardInputHandler = GameManager.I.Stage.Board.BoardInputHandler;
                    if (boardInputHandler.IsMouseHoverUI) {
                        if (boardInputHandler.HoveredMouseDetectorType == MouseDetectorType.CardPile) {
                            _mouseState = MouseState.Cancel;
                        } else {
                            _mouseState = MouseState.CardEvent;
                        }
                    } else {
                        if (boardInputHandler.IsMouseHover) {
                            if (boardInputHandler.HoveredIdx >= 0) {
                                _mouseState = MouseState.Action;
                            } else {
                                _mouseState = MouseState.Move;
                            }
                        } else {
                            _mouseState = MouseState.Cancel;
                        }
                    }

                    int useNumber = _handCards[_selectCardIndex].CardNumber;

                    if (_isTutorial) {
                        var cardValidCheck = (GameManager.I.Stage.CurEvent as TutorialEvent).CheckIfHasCardSequence();
                        if (cardValidCheck.hasSequence && cardValidCheck.targetSequence.CardNumber != useNumber) {
                            goto DismissCards;
                        }

                        if (cardValidCheck.hasSequence && cardValidCheck.targetSequence.HowToUse != MouseState.Action) {
                            goto DismissCards;
                        }
                    }

                    switch (_mouseState)
                    {
                        case MouseState.Action:
                            var target = GameManager.I.Stage.Enemies[boardInputHandler.HoveredIdx];

                            if (!CardUseAction(useNumber, target))
                            {
                                break;
                            }

                            _cardUsedCountOnThisTurn++;
                            if (_isTutorial) {
                                CheckTutorialStateForCard(useNumber);
                            }
                            yield break;

                        case MouseState.Move:
                            CardUseMove(useNumber);

                            _cardUsedCountOnThisTurn++;
                            if (_isTutorial) {
                                CheckTutorialStateForCard(useNumber);
                            }
                            yield break;
                        
                        case MouseState.CardEvent:
                            GameManager.I.UI.UICardEvent.SelectedCard(useNumber);
                            _state = CardState.Idle;
                            UpdateCardState(useNumber, false);
                            DismissAllCards();
                            yield return Discard(_selectCardIndex, CardAnimationType.TurnEnd);

                            _cardUsedCountOnThisTurn++;
                            if (_isTutorial) {
                                CheckTutorialStateForCard(useNumber);
                            }
                            yield break;
                    }

                    DismissCards:
                    _handcardsUI[_selectCardIndex].IsSelect = false;
                    _handcardsUI[_selectCardIndex].transform.localScale=new Vector3(1, 1, 1);
                    _cardDeckUIParent.GetComponent<HorizontalLayoutGroup>().SetLayoutHorizontal();
                    _cardDeckUIParent.GetComponent<HorizontalLayoutGroup>().SetLayoutVertical();
                    DismissAllCards();

                    _state = CardState.Idle;
                    yield break;
                }
                yield return null;
            }
        }

        private void DismissAllCards() {
            foreach (CardUI c in _handcardsUI)
            {
                if (c == null)
                    continue;
                c.ClickDismiss();
            }
        }

        private void SetCardUnselectableExcept(int cardNumber) {
            foreach(CardUI c in _handcardsUI)
            {
                if (c.Card.CardNumber == cardNumber) {
                    c.IsSelectable = true;
                } else {
                    c.IsSelectable = false;
                }
            }
        }

        private void CheckTutorialStateForCard(int useNumber) {
            (GameManager.I.Stage.CurEvent as TutorialEvent).CheckCardQuest(useNumber, MouseState.Move);
            var cardSequenceCheck = (GameManager.I.Stage.CurEvent as TutorialEvent).CheckIfHasCardSequence();

            if (cardSequenceCheck.hasSequence) {
                SetCardUnselectableExcept(cardSequenceCheck.targetSequence.CardNumber);
            }
        }

        public void CardUseMove(int num)
        {
            StartCoroutine(GameManager.I.Player.MoveTo(num,0.4f));

            StartCoroutine(Discard(_selectCardIndex, CardAnimationType.UseMove));
            _state = CardState.Idle;
            _prevCardNumber = -1;
            _continuousUseCount = 0;
            _canActionUse = true;
            _lastCardUsedForAction = false;
            DismissAllCards();
        }

        public void CardUsePrevMove(int num)
        {
            StartCoroutine(GameManager.I.Player.PrevMoveTo(num, 0.4f));
            _state = CardState.Idle;
            _prevCardNumber = -1;
            _canActionUse = true;
            _lastCardUsedForAction = false;
            StartCoroutine(Discard(_selectCardIndex, CardAnimationType.UseMove));
            DismissAllCards();
        }

        public void WarpArtifact()
        {
            CardUseMove(1);
        }

        public bool CheckUseCardOnAction()
        {
            bool result = true;
            if(GameManager.I.Player.OnTile.Type==TileType.Start||
               GameManager.I.Player.OnTile.Type == TileType.Blank)
            {
                result = false;
            }
            
            if (!_canActionUse)
            {
                result = false;
            }
            
            // [디버프] 감전
            if(GameManager.I.Player.CheckBuffExist(BuffType.ElectricShock) && _continuousUseCount >= 2)
            {
                Debug.Log("뭐지 감전당했나?");
                result = false;
            }
            
            return result;
        }
        private bool CardUseAction(int num, BaseEntity target=null)
        {
            if (!CheckUseCardOnAction()) return false;
           
            if(_prevCardNumber == -1 || _prevCardNumber + 1 == num)
            {
               
                _prevCardNumber = num;
                switch (GameManager.I.Player.OnTile.Type)
                {
                    case TileType.Attack:
                        StartCoroutine(Discard(_selectCardIndex, CardAnimationType.UseAttack));
                        break;
                    case TileType.Defence:
                        StartCoroutine(Discard(_selectCardIndex, CardAnimationType.UseDefense));
                        break;
                    default:
                        StartCoroutine(Discard(_selectCardIndex, CardAnimationType.UseMove));
                        break;
                }

                // [디버프] 슬로우
                if (GameManager.I.Player.CheckBuffExist(BuffType.Slow)&& _continuousUseCount == 0)
                {
                    Debug.Log("슬로우 때문에 행동 무시");
                }
                else
                {
                    StartCoroutine(GameManager.I.Player.CardAction(num, target));
                }

                _continuousUseCount++;
                _state = CardState.Idle;
                _lastCardUsedForAction = true;
                UpdateCardState(num, false);
                DismissAllCards();
                if (GameManager.I.Stage.Enemies.Count == 0)
                {
                    EndBattle();
                }
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

        public void UpdateCardState(int usedCardNumber, bool isMove)
        {
            if (!_canActionUse)
            {
                foreach(CardUI card in _handcardsUI)
                {
                    card.CanMove = true;
                    card.CanAction = false;
                    
                }
                return;
            }

            if (isMove)
            {
                for (int i = 0; i < _handcardsUI.Count; i++)
                {
                    _handcardsUI[i].CanMove = true;
                    _handcardsUI[i].CanAction = true;
                }
                return;
            }

            foreach (CardUI card in _handcardsUI)
            {
                card.CanAction = false;
                card.CanMove = true;
            }

            int prevNum = usedCardNumber+1;
            for(int i = 0; i < _handcardsUI.Count; i++)
            {
                // [디버프] 감전
                if(GameManager.I.Player.CheckBuffExist(BuffType.ElectricShock) && _continuousUseCount >= 2)
                {
                    return;
                }

                if (_handcardsUI[i].Card.CardNumber == prevNum)
                {
                    _handcardsUI[i].CanAction = true;
                }
            }

            
        }
    }

}
