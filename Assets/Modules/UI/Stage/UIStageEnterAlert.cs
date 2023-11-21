using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Util;

namespace Cardinals.Game {
    public class UIStageEnterAlert: MonoBehaviour {
        private ComponentGetter<TextMeshProUGUI> _stageNameText
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Name");
        
        public void Init(Stage stage) {
            _stageNameText.Get(gameObject).SetText(stage.Name);
        }
    }
}

