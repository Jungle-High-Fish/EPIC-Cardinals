using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

using Util;

namespace Cardinals.Board {
    [CreateAssetMenu(fileName = "TileEventData", menuName = "Cardinals/Tile Event Data")]
    public class BoardEventDataSO : ScriptableObject {
        public BoardEventType eventType;
        public string eventName;
        [Multiline]
        public string eventDescription;
        [AssetSelector(Paths = "Assets/Resources/Sprites/BoardEvent")]
        [PreviewField(Height = 100, Alignment = ObjectFieldAlignment.Left)]
        public Sprite sprite;

        public static BoardEventDataSO Data(BoardEventType type) {
            return ResourceLoader.LoadSO<BoardEventDataSO>(
                Constants.FilePath.Resources.SO_BoardObjectData + type
            );
        }
    }
}