using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cardinals.Enums;
using Sirenix.OdinInspector;
using Cardinals.Board;
using Cardinals.Tutorial;
using Cardinals.Buff;
using UnityEngine.UI;
using Util;
using System.Linq;

namespace Cardinals
{
    public class DiceManager : MonoBehaviour
    {
        public List<Dice> Dices => _dices;
        public List<DiceUI> DiceUis => _dicesUI;

        private int _prevDiceNumber = -1;
        private int _selectDiceIndex;
        private bool _canActionUse;
        private bool _lastDiceUsedForAction;
        private bool _isMouseOnDiceDeck;
        private int _continuousUseCount;
        private int _diceUsedCountOnThisTurn;

        #region Tutorial
        private bool _isTutorial;
        #endregion

        private CardState _state;
        private MouseState _mouseState = MouseState.Cancel;

        [SerializeField] private bool _newDiceUseMod;
        [ShowInInspector] private List<Dice> _dices;
        private List<DiceUI> _dicesUI;

        //public IEnumerable<Card> HandCards => _handCards;

        [SerializeField] private Transform _diceDeckUIParent;

        public bool IsElectricShock { get; set; }

        private int _selectedNumber;

        public int SelectCardIndex
        {
            set => _selectDiceIndex = value;
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
        public void Init(List<(int[], DiceType)> initialDiceList = null)
        {
            _dices = new();
            _dicesUI = new();
            _newDiceUseMod = true;
            
            if (initialDiceList != null) {
                foreach ((int[], DiceType) dice in initialDiceList) {
                    AddDice(dice.Item1.ToList(), dice.Item2);
                }
            } else {
                AddDice(new List<int>() { 1,1,2,2,3,3 }, DiceType.Normal);
                AddDice(new List<int>() { 1,1,2,2,3,3 }, DiceType.Fire);
                AddDice(new List<int>() { 1,1,2,2,3,3 }, DiceType.Normal);
                AddDice(new List<int>() { 3,3,4,4,5,5 }, DiceType.Water);
                AddDice(new List<int>() { 3,3,4,4,5,5 }, DiceType.Normal);
            }
            
            foreach(DiceUI d in _dicesUI)
            {
                d.gameObject.SetActive(false);
            }
        }

        public void SetDiceDeckUIParent(Transform parent)
        {
            _diceDeckUIParent = parent;
        }

        [Button]
        public IEnumerator OnTurn()
        {

            foreach(DiceUI d in _dicesUI)
            {
                d.EnableCardUI();
            }
            SetDiceSelectable(true);
            RollAllDice();
            _canActionUse = false;
            if (!_lastDiceUsedForAction && _newDiceUseMod)
            {
                _canActionUse = true;
            }

            _diceUsedCountOnThisTurn = 0;
            _continuousUseCount = 0;
            _state = CardState.Idle;
            UpdateDiceState(-1, true);
            yield break;
        }

        [Button]
        public IEnumerator EndTurn()
        {
            SetDiceSelectable(false);
            //StartCoroutine(DiscardAll(0, CardAnimationType.TurnEnd));
            for(int i = 0; i < _dicesUI.Count; i++)
            {
                if (_dicesUI[i].IsDiscard)
                    continue;

                StartCoroutine(Discard(i, DiceAnimationType.UseMove, () => { }));
            }
            yield return new WaitForSeconds(0.5f);
            yield break;
        }


        public void OnBattle(bool isTutorial = false)
        {
            _lastDiceUsedForAction = false;
            _prevDiceNumber = -1;

            _isTutorial = isTutorial;
        }

        public IEnumerator EndBattle()
        {
            yield return EndTurn();
        }
        [Button]
        public void TutorialRoll(int[] diceNumbers)
        {
            for(int i = 0; i < diceNumbers.Length; i++)
            {
                if (diceNumbers[i] == -1)
                {
                    StartCoroutine(Discard(i, DiceAnimationType.Empty, () => { }));
                }

                else
                {
                    int resultIndex = _dices[i].DiceNumbers.FindIndex(n => n==diceNumbers[i]);
                    int rollResult = _dices[i].DiceNumbers[resultIndex];
                    _dices[i].RollResultIndex = resultIndex;
                    _dices[i].RollResultNumber = rollResult;
                    _dicesUI[i].RollDiceUI(rollResult);
                    _dicesUI[i].DiceDescription.SetDescriptionUIRestored();
                }
            }
        }

        public void DrawHandDecksForTutorial(int[] cardNumbers)
        {
           /* for (int i = 0; i < cardNumbers.Length; i++)
            {
                AddCard(cardNumbers[i], true, CardPileType.Hand);
            }

            _canActionUse = false;
            if (!_lastCardUsedForAction && _newCardUseMod)
            {
                _canActionUse = true;
            }

            _cardUsedCountOnThisTurn = 0;
            _continuousUseCount = 0;

            UpdateCardState(-1, true);

            var cardSequenceCheck = (GameManager.I.Stage.CurEvent as TutorialEvent).CheckIfHasCardSequence();

            if (cardSequenceCheck.hasSequence)
            {
                SetCardUnselectableExcept(cardSequenceCheck.targetSequence.CardNumber);
            }*/
        }
        public void SetDiceSelectable(bool isSelectable)
        {
            /*if (isSelectable == true && _isTutorial)
            {
                var cardSequenceCheck = (GameManager.I.Stage.CurEvent as TutorialEvent).CheckIfHasCardSequence();
                if (cardSequenceCheck.hasSequence)
                {
                    SetCardUnselectableExcept(cardSequenceCheck.targetSequence.CardNumber);
                    return;
                }
            }*/

            foreach (DiceUI d in _dicesUI)
            {
                d.IsSelectable = isSelectable;
            }
        }
        public void SetDiceMouseState(bool isOnDiceDeck)
        {
            _isMouseOnDiceDeck = isOnDiceDeck;
        }

        [Button]
        public void AddDice(List<int> numbers, DiceType type)
        {
            Dice dice = new Dice(numbers, type);
            _dices.Add(dice);

            GameObject diceUI = Instantiate(ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Dice),_diceDeckUIParent);
            Vector3 UIPos = Vector3.zero;
            if (_dicesUI.Count == 0)
            {
                UIPos = new Vector3(100f, 100f, 0);
            }
            else
            {
                UIPos = _dicesUI[_dicesUI.Count - 1].GetComponent<RectTransform>().anchoredPosition;
                UIPos.x += 140f;
            }

            diceUI.GetComponent<RectTransform>().anchoredPosition = UIPos;
            diceUI.GetComponent<DiceUI>().Init(dice, _dicesUI.Count,this);
            diceUI.GetComponent<DiceUI>().DiceDescription.Init(numbers, type);
            _dicesUI.Add(diceUI.GetComponent<DiceUI>());
        }

       
        public void ChangeDice(int index, Dice dice)
        {
            bool isDiscard = _dicesUI[index].IsDiscard;
            int resultIndex = UnityEngine.Random.Range(0, dice.DiceNumbers.Count);
            int rollResult = dice.DiceNumbers[resultIndex];
            dice.RollResultIndex = resultIndex;
            dice.RollResultNumber = rollResult;

            _dices.RemoveAt(index);
            _dices.Insert(index,dice);
            _dicesUI[index].UpdateDiceUI(dice);
            _dicesUI[index].DiceDescription.UpdateDiceDescription(dice);

            if (isDiscard)
            {
                _dicesUI[index].IsDiscard = true;
                _dicesUI[index].IsSelect = false;
                _dicesUI[index].gameObject.SetActive(false);
            }
        }

        [Button]
        public void Roll(int index)
        {
            int resultIndex = UnityEngine.Random.Range(0, _dices[index].DiceNumbers.Count);
            int rollResult = _dices[index].DiceNumbers[resultIndex];
            _dices[index].RollResultIndex = resultIndex;
            _dices[index].RollResultNumber = rollResult;
            _dicesUI[index].RollDiceUI(rollResult);
            _dicesUI[index].DiceDescription.SetDescriptionUIRestored();
        }

       

        [Button]
        public void SortDices()
        {
            _dices.Sort((p1, p2) => p1.RollResultNumber.CompareTo(p2.RollResultNumber));
            for(int i = 0; i < _dicesUI.Count; i++)
            {
                _dicesUI[i].UpdateDiceUI(_dices[i]);
                _dicesUI[i].DiceDescription.UpdateDiceDescription(_dices[i]);
            }

        }

        [Button]
        public void RollAllDice()
        {
            for(int i = 0; i < _dices.Count; i++)
            {
                Roll(i);
            }
        }

        private IEnumerator Discard(int index, DiceAnimationType animationType, System.Action changeDiscardState)
        {
            _dicesUI[index].IsDiscard = true;
            _dicesUI[index].IsSelect = false;

            GameManager.I.Sound.CardUse();

            yield return _dicesUI[index].DiceAnimation.Play(animationType);
            _dicesUI[index].gameObject.SetActive(false);

            changeDiscardState?.Invoke();
        }


        public IEnumerator Dragging()
        {
            foreach (DiceUI c in _dicesUI)
            {
                if (c == null)
                    continue;
                c.StartDraggingState();
            }

            bool initFirstSelect = false;
            while (_state == CardState.Select)
            {
                if (!initFirstSelect)
                {
                    GameManager.I.Player.MotionThinking();
                    initFirstSelect = true;
                }
                
                if (Input.GetMouseButtonUp(0))
                {
                    GameManager.I.Player.MotionIdle();
                    
                    IBoardInputHandler boardInputHandler = GameManager.I.Stage.Board.BoardInputHandler;
                    if (boardInputHandler.IsMouseHoverUI)
                    {
                        if (boardInputHandler.HoveredMouseDetectorType == UIMouseDetectorType.CardPile)
                        {
                            _mouseState = MouseState.Cancel;
                        }
                        else
                        {
                            _mouseState = MouseState.CardEvent;
                        }
                    }
                    else
                    {
                        if (boardInputHandler.IsMouseHover)
                        {
                            if (boardInputHandler.HoveredIdx >= 0)
                            {
                                _mouseState = MouseState.Action;
                            }
                            else
                            {
                                _mouseState = MouseState.Move;
                            }
                        }
                        else
                        {
                            _mouseState = MouseState.Cancel;
                        }
                    }

                    int useNumber = _dices[_selectDiceIndex].RollResultNumber;
                    _selectedNumber = useNumber;
                    if (_isTutorial)
                    {
                        var cardValidCheck = (GameManager.I.Stage.CurEvent as TutorialEvent).CheckIfHasCardSequence();
                        if (cardValidCheck.hasSequence && cardValidCheck.targetSequence.CardNumber != useNumber)
                        {
                            goto DismissCards;
                        }

                        if (cardValidCheck.hasSequence && cardValidCheck.targetSequence.HowToUse != _mouseState)
                        {
                            goto DismissCards;
                        }
                    }

                    switch (_mouseState)
                    {
                        case MouseState.Action:
                            var target = GameManager.I.Stage.Enemies[boardInputHandler.HoveredIdx];

                            if (!CheckUseDiceOnAction())
                            {
                                break;
                            }

                            if(GameManager.I.Player.PlayerInfo.CheckBlessExist(BlessType.BlessWind1)
                                && useNumber==1 && _prevDiceNumber == 5)
                            {
                                StartCoroutine(DiceUseAction(useNumber, _dices[_selectDiceIndex].DiceType, target));
                                yield break;
                            }

                            if (_prevDiceNumber != -1 && _prevDiceNumber + 1 != useNumber)
                            {
                                break;
                            }
                            StartCoroutine(DiceUseAction(useNumber, _dices[_selectDiceIndex].DiceType,target));
                            
                            if (_isTutorial)
                            {
                                CheckTutorialStateForCard(useNumber, MouseState.Action);
                            }
                            yield break;

                        case MouseState.Move:
                            StartCoroutine(DiceUseMove(useNumber));
                            
                            if (_isTutorial)
                            {
                                CheckTutorialStateForCard(useNumber, MouseState.Move);
                            }
                            yield break;

                        case MouseState.CardEvent:
                            yield return Discard(_selectDiceIndex, DiceAnimationType.UseMove, 
                                () =>
                                {
                                    GameManager.I.UI.UIDiceEvent.SelectedCard(useNumber);
                                    _state = CardState.Idle;
                                    UpdateDiceState(useNumber, false);
                                    DismissAllCards();
                            
                                    _diceUsedCountOnThisTurn++;
                                    if (_isTutorial)
                                    {
                                        CheckTutorialStateForCard(useNumber, MouseState.CardEvent);
                                    }
                                });
                            
                            yield break;
                    }

                DismissCards:
                    _dicesUI[_selectDiceIndex].DismissDiceUI();
                    DismissAllCards();

                    _state = CardState.Idle;
                    yield break;
                }
                yield return null;
            }
        }

        private void DismissAllCards()
        {
            foreach (DiceUI d in _dicesUI)
            {
                if (d == null)
                    continue;
                d.ClickDismiss();
            }
        }

        private void SetCardUnselectableExcept(int cardNumber)
        {
            /*foreach (DiceUI d in _dicesUI)
            {
                if (d.Card.CardNumber == cardNumber)
                {
                    d.IsSelectable = true;
                }
                else
                {
                    d.IsSelectable = false;
                }
            }*/
        }

        private void CheckTutorialStateForCard(int useNumber, MouseState mouseState)
        {
            /*(GameManager.I.Stage.CurEvent as TutorialEvent).CheckCardQuest(useNumber, mouseState);
            var cardSequenceCheck = (GameManager.I.Stage.CurEvent as TutorialEvent).CheckIfHasCardSequence();

            if (cardSequenceCheck.hasSequence)
            {
                SetCardUnselectableExcept(cardSequenceCheck.targetSequence.CardNumber);
            }*/
        }

        public IEnumerator DiceUseMove(int num)
        {
            _diceUsedCountOnThisTurn++;
            SetDiceSelectable(false);
            StartCoroutine(Discard(_selectDiceIndex, DiceAnimationType.UseMove, () => { }));
            if (GameManager.I.Player.CheckBuffExist(BuffType.Confusion)&&Random.Range(0,2)==1)
            {
                GameManager.I.Player.Bubble.SetBubble("혼란하다 혼란해..");
                yield return GameManager.I.Player.PrevMoveTo(num, 0.4f);
            }
            else
            {
                yield return GameManager.I.Player.MoveTo(num, 0.4f);
            }

            GameManager.I.DiceRollingCount++;

            _state = CardState.Idle;
            _prevDiceNumber = -1;
            _continuousUseCount = 0;
            _canActionUse = true;
            _lastDiceUsedForAction = false;
            DismissAllCards();
            SetDiceSelectable(true);
        }

        public void PotionUseMove(int num)
        {
            StartCoroutine(GameManager.I.Player.MoveTo(num, 0.4f));
            _prevDiceNumber = -1;
            _continuousUseCount = 0;
            _canActionUse = true;
            _lastDiceUsedForAction = false;
            DismissAllCards();
        }

        public void PotionUsePrevMove(int num)
        {
            StartCoroutine(GameManager.I.Player.PrevMoveTo(num, 0.4f));
            _prevDiceNumber = -1;
            _continuousUseCount = 0;
            _canActionUse = true;
            _lastDiceUsedForAction = false;
            DismissAllCards();
        }

        public void PotionUseAction(int num)
        {
            StartCoroutine(GameManager.I.Player.CardAction(num, GameManager.I.Stage.Enemies[Random.Range(0, GameManager.I.Stage.Enemies.Count)]));
            _state = CardState.Idle;
            DismissAllCards();
            if (GameManager.I.Stage.Enemies.Count == 0)
            {
                StartCoroutine(EndBattle());
            }
        }

        public void WarpArtifact()
        {
            StartCoroutine(GameManager.I.Player.MoveTo(1, 0.4f));
            _state = CardState.Idle;
            _prevDiceNumber = -1;
            _continuousUseCount = 0;
            _canActionUse = true;
            _lastDiceUsedForAction = false;
            DismissAllCards();
        }

        public bool CheckUseDiceOnAction()
        {
            bool result = true;

            // if (!_handcardsUI[_selectCardIndex].CanAction)
            // {
            //     result = false;
            // }

            if (GameManager.I.Stage.Board.IsBoardSquare) {
                if (GameManager.I.Player.OnTile.Type == TileType.Start ||
                GameManager.I.Player.OnTile.Type == TileType.Blank)
                {
                    result = false;
                }
            }
            

            if (!_canActionUse)
            {
                result = false;
            }

            // [�����] ����
            if (GameManager.I.Player.CheckBuffExist(BuffType.ElectricShock) && _continuousUseCount >= 2)
            {
                Debug.Log("뭐지? 감전당했나?");
                GameManager.I.Player.Bubble.SetBubble("감전당해서 사용할 수 없어...");
                result = false;
            }

            return result;
        }

        private IEnumerator DiceUseAction(int num, DiceType type, BaseEntity target = null)
        {
            _diceUsedCountOnThisTurn++;
            SetDiceSelectable(false);
            _prevDiceNumber = num;

            bool hasDiscard = false;
            void ChangeDiscard()
            {
                hasDiscard = true;
            }

            switch (GameManager.I.Player.OnTile.TileMagic.Type)
            {
                case TileMagicType.Defence:
                case TileMagicType.Earth:
                case TileMagicType.Water:
                    StartCoroutine(Discard(_selectDiceIndex, DiceAnimationType.UseDefense, ChangeDiscard));
                    break;
                default:
                    StartCoroutine(Discard(_selectDiceIndex, DiceAnimationType.UseAttack, ChangeDiscard));
                    break;
            }

            //[축복] 태풍 : 3번째 행동마다 적에게 3의 데미지를 추가로 부여
            if (GameManager.I.Player.PlayerInfo.CheckBlessExist(BlessType.BlessWind2)&& _continuousUseCount == 2)
            {
                target.Hit(3);
            }

            // [�����] ���ο�
            if (GameManager.I.Player.CheckBuffExist(BuffType.Slow) && _continuousUseCount == 0)
            {
                GameManager.I.Player.Bubble.SetBubble("슬로우 때문에 첫 번째 행동이 무시되었어.");
                Debug.Log("���ο� ������ �ൿ ����");
            }
            else
            {
                yield return GameManager.I.Player.CardAction(num, target);
                DiceBuffByType(num, type, target);
                GameManager.I.DiceRollingCount++;
            }

            yield return new WaitUntil(() => hasDiscard);

            _continuousUseCount++;
            _state = CardState.Idle;
            _lastDiceUsedForAction = true;
            UpdateDiceState(num, false);
            DismissAllCards();
            if (GameManager.I.CurrentEnemies.Count() == 0)
            {
                yield return EndBattle();
            }
            SetDiceSelectable(true);

        }

        private void DiceBuffByType(int num, DiceType type, BaseEntity target = null)
        {
            if(type==DiceType.Fire 
                && GameManager.I.Player.OnTile.TileMagic.Type == TileMagicType.Fire)
            {
                target.AddBuff(new Burn(num));
            }
            else if(type == DiceType.Water
                && GameManager.I.Player.OnTile.TileMagic.Type == TileMagicType.Water)
            {
                target.AddBuff(new Weak(num));
            }

            else if(type == DiceType.Earth
                && GameManager.I.Player.OnTile.TileMagic.Type == TileMagicType.Earth)
            {
                target.AddBuff(new Powerless(num));
            }
        }

        public void UpdateDiceState(int usedDiceNumber, bool isMove)
        {
            if (!_canActionUse)
            {
                foreach (DiceUI dice in _dicesUI)
                {
                    dice.CanMove = true;
                    dice.CanAction = false;

                }
                return;
            }

            if (isMove)
            {
                for (int i = 0; i < _dicesUI.Count; i++)
                {
                    _dicesUI[i].CanMove = true;
                    _dicesUI[i].CanAction = true;
                }
                return;
            }

            foreach (DiceUI dice in _dicesUI)
            {
                dice.CanAction = false;
                dice.CanMove = true;
            }

            int prevNum = usedDiceNumber + 1;
            for (int i = 0; i < _dicesUI.Count; i++)
            {
                // [�����] ����
                if (GameManager.I.Player.CheckBuffExist(BuffType.ElectricShock) && _continuousUseCount >= 2)
                {
                    return;
                }

                if (_dicesUI[i].Dice.RollResultNumber == prevNum)
                {
                    _dicesUI[i].CanAction = true;
                }
            }


        }
    }


}