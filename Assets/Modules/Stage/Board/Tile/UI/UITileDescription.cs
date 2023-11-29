using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Sirenix.OdinInspector;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    public class UITileDescription: MonoBehaviour{
        public float Height => _height;

        private ComponentGetter<Image> _panel
            = new ComponentGetter<Image>(TypeOfGetter.This);

        private ComponentGetter<Image> _outline
            = new ComponentGetter<Image>(TypeOfGetter.ChildByName, "Outline");

        private ComponentGetter<RectTransform> _contentsTransform
            = new ComponentGetter<RectTransform>(TypeOfGetter.ChildByName, "Contents");

        private ComponentGetter<RectTransform> _titleHeaderTransform
            = new ComponentGetter<RectTransform>(TypeOfGetter.ChildByName, "Contents/TitleHeader");

        private ComponentGetter<Image> _icon
            = new ComponentGetter<Image>(TypeOfGetter.ChildByName, "Contents/TitleHeader/Icon");

        private ComponentGetter<TextMeshProUGUI> _titleText
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Contents/TitleHeader/Title");

        private ComponentGetter<TextMeshProUGUI> _descriptionText
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Contents/Description");
        
        private Color _panelColor;
        private Color _textColor;
        private Color _outlineColor;

        private float _height;

        [Button]
        public void SetDescription(string title, string description, Sprite icon=null, bool isWhite=true, Color? outlineColor=null) {
            SetPanelColor(isWhite, outlineColor);
            SetDescription(title, description, icon);

            gameObject.SetActive(false);
        }

        public void Show(float posY) {
            gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
            CalculateAndSetHeight();
            (transform as RectTransform).anchoredPosition = new Vector2(
                (transform as RectTransform).anchoredPosition.x,
                posY
            );
        }

        public void Show(float startPosY, float gap, float duration) {
            gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
            CalculateAndSetHeight();
            (transform as RectTransform).anchoredPosition = new Vector2(
                (transform as RectTransform).anchoredPosition.x,
                startPosY
            );
            (transform as RectTransform).DOAnchorPosY(startPosY - gap, duration).SetEase(Ease.OutCubic);
        }

        private void SetPanelColor(bool isWhite, Color? outlineColor) {
            if (isWhite) {
                _panelColor = Constants.Common.Colors.CardinalsWhite;
                _textColor = Constants.Common.Colors.CardinalsBlack;
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
            _descriptionText.Get(gameObject).color = Color.black;
        }

        private void SetDescription(string title, string description, Sprite icon=null) {
            _titleText.Get(gameObject).text = title;

            TextMeshProUGUI tmp = _descriptionText.Get(gameObject);
            tmp.text = description;

            if (icon != null) {
                _icon.Get(gameObject).gameObject.SetActive(true);
                _icon.Get(gameObject).sprite = icon;
            } else {
                _icon.Get(gameObject).gameObject.SetActive(false);
            }
        }

        public float CalculateAndSetHeight() {
            float descriptionHeight 
                = _descriptionText.Get(gameObject).GetStringHeight(_descriptionText.Get(gameObject).text);
            
            float contentsHeight = 
                _titleHeaderTransform.Get(gameObject).sizeDelta.y +
                descriptionHeight;

            (transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentsHeight);

            _height = contentsHeight;

            return contentsHeight;
        }
    }
}

