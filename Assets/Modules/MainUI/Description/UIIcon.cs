using System.Collections;
using System.Collections.Generic;
using Cardinals.UI;
using UnityEngine;
using UnityEngine.UI;
using Util;
using DG.Tweening;

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

        public void Init()
        {
            
        }

        public void Init(Sprite sprite, Color? innerColor=null)
        {
            transform.localScale = Vector3.one * 1.5f;
            transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);

            _background.Get(gameObject).color = innerColor ?? Constants.Common.Colors.CardinalsWhite;
            _icon.Get(gameObject).sprite = sprite;

            if (sprite == null) {
                _icon.Get(gameObject).color = Color.clear;
            } else {
                _icon.Get(gameObject).color = Color.white;
            }

            GetComponent<DescriptionConnector>().Init();
        }
    }
}