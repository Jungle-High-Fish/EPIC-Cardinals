using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using UnityEngine;

namespace Cardinals.BoardEvent.Alchemy
{
    [CreateAssetMenu(fileName = "AlchemyEventData", menuName = "Cardinals/BoardEvent/Alchemy Event Data", order = 0)]
    public class AlchemyEventDataSO : ScriptableObject
    {
        public BoardEventAlchemyType type;
        [TextArea] public string eventDescription;
    }
}