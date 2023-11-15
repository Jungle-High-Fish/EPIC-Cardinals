using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IPointerDownHandler
{
    private int _cardIndex;
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
    public void Init(Card card, int index,CardManager cardManager)
    {
        _card = card;
        _cardIndex = index;
        _cardManager = cardManager;
        Image image = GetComponent<Image>();

        switch (card.CardNumber)
        {
            case 1:
                image.color = Color.red;
                break;
            case 2:
                image.color = Color.yellow;
                break;
            case 3:
                image.color = Color.green;
                break;
            case 4:
                image.color = Color.blue;
                break;
            case 5:
                image.color = Color.black;
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
public enum CardState
{
    Idle,
    Select
}

public enum MouseState
{
    Action,
    Move,
    Cancel
}