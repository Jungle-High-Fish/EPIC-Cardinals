using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
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

        public void Init(TileMagicType tileMagicType, Action<TileMagicType> onClick) {
            _button.Get(gameObject).onClick.AddListener(() => onClick(tileMagicType));
        }
    }
}

