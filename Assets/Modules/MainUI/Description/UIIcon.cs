using System.Collections;
using System.Collections.Generic;
using Cardinals.UI;
using UnityEngine;
using UnityEngine.UI;
using Util;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Cardinals.UI
{
    public class UIIcon : MonoBehaviour
    {
        [SerializeField] public Image _iconImg;

        ComponentGetter<Image> _background
            = new ComponentGetter<Image>(TypeOfGetter.This);

        ComponentGetter<Image> _border
            = new ComponentGetter<Image>(TypeOfGetter.ChildByName, "Border");

        ComponentGetter<Image> _icon
            = new ComponentGetter<Image>(TypeOfGetter.ChildByName, "Icon");

        private Color _backupColor;
        private bool _isAnimating;

        public void Init()
        {

        }

        public void Init(Sprite sprite, Color? innerColor = null)
        {
            transform.localScale = Vector3.one * 1.5f;
            transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);

            _background.Get(gameObject).color = innerColor ?? Constants.Common.Colors.CardinalsWhite;
            _backupColor = _background.Get(gameObject).color;
            _icon.Get(gameObject).sprite = sprite;

            if (sprite == null)
            {
                _icon.Get(gameObject).color = Color.clear;
            }
            else
            {
                _icon.Get(gameObject).color = Color.white;
            }

            GetComponent<DescriptionConnector>().Init();
        }

        public void EffectDefault()
        {

        }

        [Button]
        public void EffectBless()
        {
            if (!_isAnimating)
            {
                _isAnimating = true;
                float duration = 0.5f;

                _icon.Get(gameObject).DOBlendableColor(_backupColor, duration);
                transform.DOPunchScale(Vector3.one, duration, 1, 0);
                _background.Get(gameObject).DOColor(Color.white, duration);
                _background.Get(gameObject).DOFade(.5f, duration)
                    .OnComplete(() =>
                    {
                        _background.Get(gameObject).color = _backupColor;
                        _icon.Get(gameObject).color = Color.white;
                        transform.localScale = Vector3.one;
                        _isAnimating = false;
                    });
            }
        }
    }
}