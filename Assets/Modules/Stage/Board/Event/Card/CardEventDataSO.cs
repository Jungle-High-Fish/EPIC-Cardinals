using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using UnityEngine;

namespace Cardinals.BoardEvent.Card
{
    [CreateAssetMenu(fileName = "CardEventData", menuName = "Cardinals/BoardEvent/Card Event Data", order = 0)]
    public class CardEventDataSO : ScriptableObject
    {
        public BoardEventCardType type;
        public string description;
        public string endText;
    }
}