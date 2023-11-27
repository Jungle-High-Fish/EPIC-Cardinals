using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Cardinals.Board {
    [CreateAssetMenu(fileName = "TileCurseUIData", menuName = "Cardinals/Tile Curse UI Data")]
    public class TileCurseUIDataSO : ScriptableObject {
        public TileCurseType curseType;
        public string curseName;
        [Multiline]
        public string curseDescription;
        [AssetSelector(Paths = "Assets/Resources/Sprites/Tile/Curses")]
        [PreviewField(Height = 100, Alignment = ObjectFieldAlignment.Left)]
        public Sprite sprite;
    }
}