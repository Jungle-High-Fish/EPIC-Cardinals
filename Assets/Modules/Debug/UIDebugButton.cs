using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.Test {
    public class UIDebugButton : MonoBehaviour, IDebugComponent
    {
        private ComponentGetter<Button> _button = new(TypeOfGetter.Child);
        private ComponentGetter<TMP_Text> _title = new(TypeOfGetter.ChildByName, "Button/Text");

        public void Init(string title, Action action) {
            _title.Get(gameObject).text = title;

            _button.Get(gameObject).onClick.RemoveAllListeners();
            _button.Get(gameObject).onClick.AddListener(() => {
                action();
            });
        }
    }
}