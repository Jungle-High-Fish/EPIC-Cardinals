using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.Test {
    public class UIDebugDropdown : MonoBehaviour, IDebugComponent
    {
        private ComponentGetter<TMP_Dropdown> _dropdown = new(TypeOfGetter.Child);
        private ComponentGetter<Button> _button = new(TypeOfGetter.Child);
        private ComponentGetter<TMP_Text> _title = new(TypeOfGetter.ChildByName, "Text");

        public void Init(string title, Type type, Action<Enum> action) {
            _title.Get(gameObject).text = title;

            TMP_Dropdown dropdown = _dropdown.Get(gameObject);
            dropdown.ClearOptions();
            dropdown.AddOptions(Enum.GetNames(type).ToList());

            _button.Get(gameObject).onClick.RemoveAllListeners();
            _button.Get(gameObject).onClick.AddListener(() => {
                action((Enum) Enum.Parse(type, dropdown.options[dropdown.value].text));
            });
        }
    }
}