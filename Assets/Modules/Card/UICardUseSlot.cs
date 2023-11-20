using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cardinals
{
    public class UICardUseSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // ���߿� ����
        [SerializeField] private CardManager _cardManager;
        [SerializeField] private MouseState _slotState;
        public void OnPointerEnter(PointerEventData eventData)
        {
            _cardManager.MouseState = _slotState;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _cardManager.MouseState = MouseState.Cancel;
        }


    }

}
