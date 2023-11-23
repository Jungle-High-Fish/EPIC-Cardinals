using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cardinals.Enums;
using Sirenix.OdinInspector;

namespace Cardinals
{
    [CreateAssetMenu(fileName = "BlessData", menuName = "Cardinals/Bless Data")]
    public class BlessDataSO : ScriptableObject
    {
        public string blessName;
        public string description;
        [PreviewField(Height = 100, Alignment = ObjectFieldAlignment.Left)]
        public Sprite totemSprite;
        [AssetSelector(Paths = "Assets/Resources/Sprites/UI/Icons/Bless")]
        [PreviewField(Height = 100, Alignment = ObjectFieldAlignment.Left)]
        public Sprite patternSprite;
        public BlessType blessType;
    }
}
