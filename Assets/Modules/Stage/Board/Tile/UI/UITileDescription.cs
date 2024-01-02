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
using UnityEngine.EventSystems;

namespace Cardinals.UI {
    public class UITileDescription: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{
        public float Height => _height;

        protected ComponentGetter<Image> _panel
            = new ComponentGetter<Image>(TypeOfGetter.This);

        protected ComponentGetter<Image> _outline
            = new ComponentGetter<Image>(TypeOfGetter.ChildByName, "Outline");

        protected ComponentGetter<RectTransform> _titleHeaderTransform
            = new ComponentGetter<RectTransform>(TypeOfGetter.ChildByName, "Contents/TitleHeader");

        protected ComponentGetter<Image> _icon
            = new ComponentGetter<Image>(TypeOfGetter.ChildByName, "Contents/TitleHeader/Icon");

        protected ComponentGetter<TextMeshProUGUI> _titleText
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Contents/TitleHeader/Title");

        protected ComponentGetter<TextMeshProUGUI> _descriptionText
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Contents/Description");
        
        protected Color _panelColor;
        protected Color _textColor;
        protected Color _outlineColor;

        protected float _height;

        public virtual void OnPointerEnter(PointerEventData eventData) {
        }

        public virtual void OnPointerExit(PointerEventData eventData) {
        }

        [Button]
        public virtual void SetDescription(string title, string description, Sprite icon=null, bool isWhite=true, Color? outlineColor=null) {
            SetPanelColor(isWhite, outlineColor);
            SetDescriptionText(title, description, icon);

            gameObject.SetActive(false);
        }

        public virtual void Show(float posY) {
            gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
            CalculateAndSetHeight();
            (transform as RectTransform).anchoredPosition = new Vector2(
                (transform as RectTransform).anchoredPosition.x,
                posY
            );
        }

        public virtual void Show(float startPosY, float gap, float duration) {
            gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
            CalculateAndSetHeight();
            (transform as RectTransform).anchoredPosition = new Vector2(
                (transform as RectTransform).anchoredPosition.x,
                startPosY
            );
            (transform as RectTransform).DOAnchorPosY(startPosY - gap, duration).SetEase(Ease.OutCubic);
        }

        protected virtual void SetPanelColor(bool isWhite, Color? outlineColor) {
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
            _descriptionText.Get(gameObject).color = isWhite ? Color.black : Color.white;
        }

        private void SetDescriptionText(string title, string description, Sprite icon=null) {
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

        public virtual float CalculateAndSetHeight() {
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

