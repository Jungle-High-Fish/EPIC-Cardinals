using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    public class DropdownOption: MonoBehaviour {
        private ComponentGetter<TextMeshProUGUI> _text
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Text");

        private ComponentGetter<TMP_Dropdown> _dropdown
            = new ComponentGetter<TMP_Dropdown>(TypeOfGetter.ChildByName, "Dropdown");

        public void Init(string text, List<string> options, Action<int> onValueChanged, int defaultValue=0) {
            _text.Get(gameObject).text = text;
            
            _dropdown.Get(gameObject).AddOptions(options);
            _dropdown.Get(gameObject).value = defaultValue;

            _dropdown.Get(gameObject).onValueChanged.AddListener((value) => {
                onValueChanged(value);
            });
        }

        public void SetValue(int value) {
            _dropdown.Get(gameObject).value = value;
        }
    }

}