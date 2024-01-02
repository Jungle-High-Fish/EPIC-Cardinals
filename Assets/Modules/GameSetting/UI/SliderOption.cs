using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    public class SliderOption: MonoBehaviour {
        private ComponentGetter<TextMeshProUGUI> _text
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Text");

        private ComponentGetter<Slider> _slider
            = new ComponentGetter<Slider>(TypeOfGetter.ChildByName, "Slider");

        private ComponentGetter<TextMeshProUGUI> _valueText
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Slider/Handle Slide Area/Handle/Slider Value");

        public void Init(string text, float min, float max, Action<float> onValueChanged, float defaultValue=0, bool isInt=true) {
            _text.Get(gameObject).text = text;
            _slider.Get(gameObject).minValue = min;
            _slider.Get(gameObject).maxValue = max;
            _slider.Get(gameObject).value = defaultValue;
            _slider.Get(gameObject).wholeNumbers = isInt;

            _slider.Get(gameObject).onValueChanged.AddListener((value) => {
                onValueChanged(value);
                _valueText.Get(gameObject).text = isInt ? ((int)value).ToString() : value.ToString();
            });

            _valueText.Get(gameObject).text = isInt ? ((int)defaultValue).ToString() : defaultValue.ToString();
        }

        public void SetValue(float value) {
            _slider.Get(gameObject).value = value;
            _valueText.Get(gameObject).text = _slider.Get(gameObject).wholeNumbers ? ((int)value).ToString() : value.ToString();
        }
    }

}