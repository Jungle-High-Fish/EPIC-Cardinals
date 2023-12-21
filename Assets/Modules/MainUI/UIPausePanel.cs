using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    public class UIPausePanel : MonoBehaviour {
        public bool IsActive => _isActive;

        private ComponentGetter<Button> _continueButton
            = new ComponentGetter<Button>(TypeOfGetter.ChildByName, "Options Area/Continue Button");
        private ComponentGetter<Button> _settingsButton
            = new ComponentGetter<Button>(TypeOfGetter.ChildByName, "Options Area/Setting Button");
        private ComponentGetter<Button> _mainMenuButton
            = new ComponentGetter<Button>(TypeOfGetter.ChildByName, "Options Area/Main Button");
        private ComponentGetter<Button> _quitGameButton
            = new ComponentGetter<Button>(TypeOfGetter.ChildByName, "Options Area/Quit Button");

        private bool _isActive = false;

        public void Init() {
            _continueButton.Get(gameObject).onClick.AddListener(OnContinueButtonClick);
            _settingsButton.Get(gameObject).onClick.AddListener(OnSettingsButtonClick);
            _mainMenuButton.Get(gameObject).onClick.AddListener(OnMainMenuButtonClick);
            _quitGameButton.Get(gameObject).onClick.AddListener(OnQuitGameButtonClick);

            Hide();
        }

        public void Show() {
            _isActive = true;
            gameObject.SetActive(true);
        }

        public void Hide() {
            _isActive = false;
            gameObject.SetActive(false);
        }

        private void OnContinueButtonClick() {
            Hide();
        }

        private void OnSettingsButtonClick() {
            GameManager.I.UI.GameSettingUI.Show();
        }

        private void OnMainMenuButtonClick() {
            // TODO: Go to main menu
            GameManager.I.GoToTitle();
        }

        private void OnQuitGameButtonClick() {
            Application.Quit();
        }
    }
}