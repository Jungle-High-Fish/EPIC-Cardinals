using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Board;
using Cardinals.Enums;
using Cardinals.UI.Description;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Cardinals.UI {
    public class UILevelUpMagicSlot: MonoBehaviour {
        private ComponentGetter<Image> _magicImage 
            = new ComponentGetter<Image>(TypeOfGetter.This);

        private ComponentGetter<Button> _button 
            = new ComponentGetter<Button>(TypeOfGetter.This);

        private ComponentGetter<MagicDescription> _magicDescription 
            = new ComponentGetter<MagicDescription>(TypeOfGetter.This);

        [Button]
        public void Init(TileMagicType tileMagicType, Action<TileMagicType> onClick) {
            var tileMagicDataSO = TileMagic.Data(tileMagicType);
            _magicImage.Get(gameObject).sprite = tileMagicDataSO.sprite;
            _button.Get(gameObject).onClick.AddListener(() => onClick(tileMagicType));

            _magicDescription.Get(gameObject).Init(tileMagicType);

            _button.Get(gameObject).interactable = false;

            (transform as RectTransform).localScale = Vector3.zero;
            (transform as RectTransform).DOScale(Vector3.one, 0.6f)
            .SetEase(Ease.OutBack).OnComplete(() => {
                _button.Get(gameObject).interactable = true;
            });
        }
    }
}

