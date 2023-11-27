using System.Collections;
using System.Collections.Generic;
using Cardinals.Board;
using Cardinals.Enums;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    public class UITileCurse: MonoBehaviour {
        private ComponentGetter<Image> _curseEmblem = new ComponentGetter<Image>(
            TypeOfGetter.ChildByName, 
            "Curse Emblem"
        );
        
        public void Set(TileCurseType type) {
            _curseEmblem.Get(gameObject).sprite
                = TileCurseData.Data(type).sprite;
        }
    }
}
