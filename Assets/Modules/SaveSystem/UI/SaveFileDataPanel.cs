using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    public class SaveFileDataPanel : MonoBehaviour {
        private ComponentGetter<TextMeshProUGUI> _fileNameText
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Text Area/File Name Text");
        private ComponentGetter<TextMeshProUGUI> _modifiedTimeText
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Text Area/File DateTime Text");

        private ComponentGetter<Button> _loadButton
            = new ComponentGetter<Button>(TypeOfGetter.ChildByName, "Button Area/Load Button");

        public void Init(string fileName, string modifiedTime, Action loadAction) {
            _fileNameText.Get().text = fileName;
            _modifiedTimeText.Get().text = modifiedTime;

            _loadButton.Get().onClick.AddListener(() => loadAction());
        }
    }
}

