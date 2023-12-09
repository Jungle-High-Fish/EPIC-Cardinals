using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Util;

namespace Cardinals.Game {
    public class UIStageEnterAlert: MonoBehaviour {
        private ComponentGetter<TextMeshProUGUI> _stageNameText
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Name");
        
        public IEnumerator Init(Stage stage) {
            _stageNameText.Get(gameObject).SetText(string.Empty);
            gameObject.SetActive(true);
            _stageNameText.Get(gameObject).SetText(stage.Name);
            yield return new WaitForSeconds(1f);
            

            yield return new WaitForSeconds(3f);
        }
    }
}

