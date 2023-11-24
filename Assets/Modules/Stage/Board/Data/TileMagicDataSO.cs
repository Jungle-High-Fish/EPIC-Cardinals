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
		[AssetSelector(Paths = "Assets/Resources/Sprites/UI/Icons/Elements")]
        [PreviewField(Height = 100, Alignment = ObjectFieldAlignment.Left)]
        public Sprite sprite;
		public string mainMagicName;
        [Multiline]
        public string mainMagicDescription;
		public bool hasSubMagic;
		[ShowIf("hasSubMagic")]
		public string subMagicName;
		[Multiline, ShowIf("hasSubMagic")]
		public string subMagicDescription;
    }
}