using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

using Cardinals.Enums;

namespace Cardinals.Game
{
    [CreateAssetMenu(fileName = "MagicData", menuName = "Cardinals/Magic Data")]
    public class TileMagicDataSO : ScriptableObject
    {
        public TileMagicType magicType;
        public string elementName;
		public Color elementColor;
        public Color particleColor1;
        public Color particleColor2;
		[AssetSelector(Paths = "Assets/Resources/Materials/Board/Tiles")]
        [PreviewField(Height = 100, Alignment = ObjectFieldAlignment.Left)]
        public Material tileMaterial;
        [AssetSelector(Paths = "Assets/Resources/Sprites/UI/Icons/Elements")]
        [PreviewField(Height = 100, Alignment = ObjectFieldAlignment.Left)]
        public Sprite sprite;
        [AssetSelector(Paths = "Assets/Resources/Sprites/UI/Icons/Elements")]
        [PreviewField(Height = 100, Alignment = ObjectFieldAlignment.Left)]
        public Sprite uiSprite;
        [AssetSelector(Paths = "Assets/Resources/Sprites/Tile/TileLevelEffect")]
        [PreviewField(Height = 100, Alignment = ObjectFieldAlignment.Left)]
        public Sprite[] tileLevelEffectSprite = new Sprite[3];
		public string mainMagicName;
        [Multiline]
        public string mainMagicDescription;
		public bool hasBuffEffect;
		[ShowIf("hasBuffEffect")]
		public BuffType buffType;
    }
}