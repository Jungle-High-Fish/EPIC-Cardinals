using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.Test {
    public class UIDebugInputField : MonoBehaviour, IDebugComponent
    {
        private ComponentGetter<TMP_InputField> _inputField = new(TypeOfGetter.Child);
        private ComponentGetter<Button> _button = new(TypeOfGetter.Child);
        private ComponentGetter<TMP_Text> _title = new(TypeOfGetter.ChildByName, "Text");

        public void Init(string title, Action<string> action) {
            _title.Get(gameObject).text = title;

            _button.Get(gameObject).onClick.RemoveAllListeners();
            _button.Get(gameObject).onClick.AddListener(() => {
                action(_inputField.Get(gameObject).text);
            });
        }
    }
}