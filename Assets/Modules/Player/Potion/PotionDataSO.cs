using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

using Cardinals.Enums;

namespace Cardinals.Game
{
    [CreateAssetMenu(fileName = "PotionData", menuName = "Cardinals/Potion Data")]
    public class PotionDataSO : ScriptableObject
    {
        public PotionType potionType;
        public string potionName;
        [Multiline]
        public string description;
        [AssetSelector(Paths = "Assets/Resources/Sprites/UI/Icons/Potions")]
        [PreviewField(Height = 100, Alignment = ObjectFieldAlignment.Left)]
        public Sprite sprite;
        public int price;
    }
}