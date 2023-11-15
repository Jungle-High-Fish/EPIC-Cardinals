using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CardManager : MonoBehaviour
{
    private int _prevCardNumber;
    private int _selectCardIndex;
    private CardState _state;
    [SerializeField] private MouseState _mouseState= MouseState.Cancel;

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
    public void Draw(int count)
    {
        for(int i = 0; i < count; i++)
        {
            int randIndex = Random.Range(0, _drawPile.Count);
            Card card = _drawPile[randIndex];

            if (_handCards.Count == 0)
            {
                _handCards.Add(card);
                UpdateCardUI(card, 0);
                _drawPile.Remove(card);
                continue;
            }
            
            for(int j = 0; j < _handCards.Count; j++)
            {

                if(_handCards[j].CardNumber >= card.CardNumber)
                {
                    _handCards.Insert(j, card);
                    UpdateCardUI(card, j);
                    break;
                }

                if (j == _handCards.Count - 1)
                {
                    _handCards.Add(card);
                    UpdateCardUI(card, j+1);
                    break;
                }
            }
            _drawPile.Remove(card);

            UpdateCardIndex();
        }
        
    }
    [Button]
    public void ListLog()
    {
        foreach(Card c in _handCards)
        {
            Debug.Log(c.CardNumber);
        }
        foreach (CardUI c in _handcardsUI)
        {
            Debug.Log(c.Card.CardNumber);
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
        for (int i = 0; i <count; i++)
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
        _handcardsUI.Insert(index,cardUI.GetComponent<CardUI>());
       
    }

    public IEnumerator Dragging()
    {
        while(_state== CardState.Select)
        {
            if (Input.GetMouseButtonUp(0))
            {
                switch (_mouseState)
                {
                    case MouseState.Action:
                        Debug.Log(_handCards[_selectCardIndex].CardNumber);
                        Debug.Log("카드 액션에 사용");
                        Discard(_selectCardIndex);
                        _state = CardState.Idle;
                        yield break;

                    case MouseState.Move:
                        Debug.Log(_handCards[_selectCardIndex].CardNumber);
                        Debug.Log("카드 이동에 사용");
                        Discard(_selectCardIndex);
                        _state = CardState.Idle;
                        yield break;

                    case MouseState.Cancel:
                        _handcardsUI[_selectCardIndex].IsSelect = false;
                        _handcardsUI[_selectCardIndex].gameObject.SetActive(false);
                        _handcardsUI[_selectCardIndex].gameObject.SetActive(true);
                        _state = CardState.Idle;
                        yield break;
                }
               
            }
            yield return null;
        }
    }

    private void UpdateCardIndex()
    {
        for(int i = 0; i < _handCards.Count; i++)
        {
            _handcardsUI[i].CardIndex = i;
        }
    }
}
