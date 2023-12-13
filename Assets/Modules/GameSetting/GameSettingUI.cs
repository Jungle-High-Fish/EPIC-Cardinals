using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    public class GameSettingUI: MonoBehaviour {
        public bool IsActive => _isActive;

        private ComponentGetter<TextMeshProUGUI> _titleText
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Title Text");

        private ComponentGetter<RectTransform> _settingContainer
            = new ComponentGetter<RectTransform>(TypeOfGetter.ChildByName, "Setting Container");

        private ComponentGetter<Button> _closeButton
            = new ComponentGetter<Button>(TypeOfGetter.ChildByName, "Close Button Panel/Close Button");
        
        private ComponentGetter<Button> _exitGameButton
            = new ComponentGetter<Button>(TypeOfGetter.ChildByName, "Close Button Panel/Exit Game Button");

        private bool _isActive = false;

        public void Init(List<SettingDataForUI> settingDatas) {
            _titleText.Get(gameObject).text = "게임 설정";

            foreach (var settingData in settingDatas) {
                AddOption(settingData);
            }

            _closeButton.Get(gameObject).onClick.AddListener(() => {
                Hide();
            });

            _exitGameButton.Get(gameObject).onClick.AddListener(() => {
                Application.Quit();
            });
        }

        public void Show() {
            _isActive = true;
            gameObject.SetActive(true);
        }

        public void Hide() {
            _isActive = false;
            gameObject.SetActive(false);
        }

        private void AddOption(SettingDataForUI settingData) {
            if (settingData is CheckboxDataForUI) {
                AddCheckbox(settingData as CheckboxDataForUI);
            } else if (settingData is DropdownDataForUI) {
                AddDropdown(settingData as DropdownDataForUI);
            } else if (settingData is SliderDataForUI) {
                AddSlider(settingData as SliderDataForUI);
            }
        }

        private void AddCheckbox(CheckboxDataForUI checkboxData) {
            var checkboxPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_CheckboxOption);
            var checkbox = Instantiate(checkboxPrefab, _settingContainer.Get(gameObject));
            checkbox.GetComponent<CheckboxOption>().Init(
                checkboxData.Title, 
                checkboxData.DefaultValue, 
                checkboxData.OnValueChanged
            );
        }

        private void AddDropdown(DropdownDataForUI dropdownData) {
            var dropdownPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_DropdownOption);
            var dropdown = Instantiate(dropdownPrefab, _settingContainer.Get(gameObject));
            dropdown.GetComponent<DropdownOption>().Init(
                dropdownData.Title, 
                dropdownData.Options, 
                dropdownData.OnValueChanged, 
                dropdownData.DefaultValue
            );
        }

        private void AddSlider(SliderDataForUI sliderData) {
            var sliderPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_SliderOption);
            var slider = Instantiate(sliderPrefab, _settingContainer.Get(gameObject));
            slider.GetComponent<SliderOption>().Init(
                sliderData.Title, 
                sliderData.MinValue, 
                sliderData.MaxValue, 
                sliderData.OnValueChanged, 
                sliderData.DefaultValue, 
                sliderData.IsInt
            );
        }

        public class SettingDataForUI {
            public string Title;
        }

        public class CheckboxDataForUI: SettingDataForUI {
            public bool DefaultValue;
            public Action<bool> OnValueChanged;

            public CheckboxDataForUI(string title, bool defaultValue, Action<bool> onValueChanged) {
                Title = title;
                DefaultValue = defaultValue;
                OnValueChanged = onValueChanged;
            }
        }

        public class DropdownDataForUI: SettingDataForUI {
            public List<string> Options;
            public IList RealValues;
            public int DefaultValue;
            public Action<int> OnValueChanged;

            public DropdownDataForUI(string title, List<string> options, IList realValues, int defaultValue, Action<int> onValueChanged) {
                Title = title;
                Options = options;
                RealValues = realValues;
                DefaultValue = defaultValue;
                OnValueChanged = onValueChanged;
            }
        }

        public class SliderDataForUI: SettingDataForUI {
            public float MinValue;
            public float MaxValue;
            public bool IsInt;
            public float DefaultValue;
            public Action<float> OnValueChanged;

            public SliderDataForUI(string title, float minValue, float maxValue, bool isInt, float defaultValue, Action<float> onValueChanged) {
                Title = title;
                MinValue = minValue;
                MaxValue = maxValue;
                IsInt = isInt;
                DefaultValue = defaultValue;
                OnValueChanged = onValueChanged;
            }
        }
    }
}