using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Sirenix.OdinInspector;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Cardinals.Board;
using UnityEngine.EventSystems;

namespace Cardinals.UI {
    public class UIOnTileDescription: UITileDescription {
        private string _description;
        private bool _isHovering = false;
        private float _targetHeight = 0;

        private Sequence _descriptionShowSequence;
        private Sequence _descriptionHideSequence;

        public override void OnPointerEnter(PointerEventData eventData) {
            _isHovering = true;

            if (_description == "") return;
            Canvas.ForceUpdateCanvases();
            _descriptionHideSequence.Complete();
            _descriptionShowSequence.Restart();
        }

        public override void OnPointerExit(PointerEventData eventData) {
            _isHovering = false;

            if (_description == "") return;
            Canvas.ForceUpdateCanvases();
            _descriptionShowSequence.Complete();
            _descriptionHideSequence.Restart();
        }

        [Button]
        public override void SetDescription(string title, string description, Sprite icon=null, bool isWhite=true, Color? outlineColor=null) {
            SetPanelColor(isWhite, outlineColor);

            _titleText.Get(gameObject).text = title;

            if (icon != null) {
                _icon.Get(gameObject).gameObject.SetActive(true);
                _icon.Get(gameObject).sprite = icon;
            } else {
                _icon.Get(gameObject).gameObject.SetActive(false);
            }

            Canvas.ForceUpdateCanvases();
            (transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 57);
            _height = 57;

            _description = description;
            _descriptionText.Get(gameObject).text = "";

            gameObject.SetActive(false);
            
            _targetHeight = _descriptionText.Get(gameObject).GetStringHeight(_description);

            _descriptionShowSequence = DOTween.Sequence();
            _descriptionShowSequence.Append(
                (transform as RectTransform).DOSizeDelta(
                new Vector2(
                    (transform as RectTransform).sizeDelta.x,
                    _targetHeight + 57
                ),
                0.3f
                ).SetEase(Ease.OutCubic)
            ).Insert(0.15f, _descriptionText.Get(gameObject).DOText(_description, 0.01f))
            .SetAutoKill(false).Pause();

            _descriptionHideSequence = DOTween.Sequence();
            _descriptionHideSequence.Append(
                (transform as RectTransform).DOSizeDelta(
                new Vector2(
                    (transform as RectTransform).sizeDelta.x,
                    57
                ),
                0.3f
                ).SetEase(Ease.OutCubic)
            ).Insert(0.05f, _descriptionText.Get(gameObject).DOText("", 0.01f))
            .SetAutoKill(false).Pause();

            _descriptionText.Get(gameObject).text = "";
        }

        public override void Show(float posY) {
            gameObject.SetActive(true);
        }

        public override void Show(float startPosY, float gap, float duration) {
            gameObject.SetActive(true);
        }

        protected override void SetPanelColor(bool isWhite, Color? outlineColor) {
            if (isWhite) {
                _panelColor = Constants.Common.Colors.CardinalsWhite;
                _textColor = Constants.Common.Colors.CardinalsBlack;
            } else {
                _panelColor = Constants.Common.Colors.CardinalsBlack;
                _textColor = Constants.Common.Colors.CardinalsWhite;
            }

            if (outlineColor == null) {
                if (isWhite) {
                    _outlineColor = Constants.Common.Colors.CardinalsBlack;
                } else {
                    _outlineColor = Constants.Common.Colors.CardinalsWhite;
                }
            } else {
                _outlineColor = (Color)outlineColor;
                _textColor = (Color)outlineColor;
            }

            _panel.Get(gameObject).color = _panelColor;
            _outline.Get(gameObject).color = _outlineColor;
            _titleText.Get(gameObject).color = _textColor;
        }

        public override float CalculateAndSetHeight() {
            return _height;
        }
    }
}

