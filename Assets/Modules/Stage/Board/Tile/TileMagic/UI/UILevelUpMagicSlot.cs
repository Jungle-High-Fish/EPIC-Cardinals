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

namespace Cardinals.UI {
    public class UILevelUpMagicSlot: MonoBehaviour {
        private ComponentGetter<Image> _magicImage 
            = new ComponentGetter<Image>(TypeOfGetter.This);

        private ComponentGetter<Button> _button 
            = new ComponentGetter<Button>(TypeOfGetter.This);

        private ComponentGetter<MagicDescription> _magicDescription 
            = new ComponentGetter<MagicDescription>(TypeOfGetter.This);

        public void Init(TileMagicType tileMagicType, Action<TileMagicType> onClick) {
            var tileMagicDataSO = TileMagic.Data(tileMagicType);
            _magicImage.Get(gameObject).sprite = tileMagicDataSO.sprite;
            _button.Get(gameObject).onClick.AddListener(() => onClick(tileMagicType));

            _magicDescription.Get(gameObject).Init(tileMagicType);
        }
    }
}

