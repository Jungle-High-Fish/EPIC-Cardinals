using Cardinals.UI;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;
using TMPro;

namespace Cardinals.Title {
    public class TitleTutorialCheck : MonoBehaviour {
        private ComponentGetter<RectTransform> _tutorialCheckPanelTr
            = new(TypeOfGetter.ChildByName, "Tutorial Check Panel");
        private ComponentGetter<UIButton> _confirmButton
            = new(TypeOfGetter.ChildByName, "Tutorial Check Panel/Button Area/Confirm Button");
        private ComponentGetter<UIButton> _denyButton
            = new(TypeOfGetter.ChildByName, "Tutorial Check Panel/Button Area/Deny Button");
        private ComponentGetter<TextMeshProUGUI> _tutorialCheckTitleTMP
            = new(TypeOfGetter.ChildByName, "Tutorial Check Panel/Title");

        private Action _onConfirm, _onDeny;

        public void Init() {
            _confirmButton.Get(gameObject).Init(OnConfirm, true);
            _denyButton.Get(gameObject).Init(OnDeny, true);
            _tutorialCheckTitleTMP.Get(gameObject).text = GameManager.I.Localization[LocalizationEnum.UI_TUTORIAL_SKIP];
            gameObject.SetActive(false);
        }

        public void Show(Action onConfirm, Action onDeny) {
            gameObject.SetActive(true);
            _onConfirm = onConfirm;
            _onDeny = onDeny;

            _tutorialCheckPanelTr.Get(gameObject).localScale = Vector3.zero;
            _tutorialCheckPanelTr.Get(gameObject).gameObject.SetActive(true);

            _tutorialCheckPanelTr.Get(gameObject).DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        private void OnConfirm() {
            _onConfirm?.Invoke();
        }

        private void OnDeny() {
            _onDeny?.Invoke();
        }
    }
}
