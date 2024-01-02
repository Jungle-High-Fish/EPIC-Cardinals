using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    public class CheckboxOption: MonoBehaviour {
        private ComponentGetter<TextMeshProUGUI> _text
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Text");
        private ComponentGetter<Toggle> _toggle
            = new ComponentGetter<Toggle>(TypeOfGetter.ChildByName, "Toggle");

        public void Init(string text, bool defaultValue, System.Action<bool> onValueChanged) {
            _text.Get(gameObject).text = text;
            _toggle.Get(gameObject).isOn = defaultValue;
            _toggle.Get(gameObject).onValueChanged.AddListener((value) => {
                GameManager.I.Sound.TitleButtonClick();
                onValueChanged(value);
            });
        }

        public void SetValue(bool value) {
            _toggle.Get(gameObject).isOn = value;
        }
    }
}

