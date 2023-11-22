using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals
{
    [CreateAssetMenu(fileName = "BlessData", menuName = "Cardinals/Bless Data")]
    public class BlessDataSO : ScriptableObject
    {
        public string blessName;
        public string description;
        public Sprite totemSprite;
        public Sprite patternSprite;
    }
}
