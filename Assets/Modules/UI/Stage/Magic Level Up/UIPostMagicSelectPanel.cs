using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    public class UIPostMagicSelectPanel: MonoBehaviour {
        private ComponentGetter<Image> _levelUpMagicImage = new ComponentGetter<Image>(
            TypeOfGetter.ChildByName,
            "Magic Slot List/Level Up Slot/Magic Slot"
        );

        private ComponentGetter<TextMeshProUGUI> _levelUpMagicTargetLevelText = new ComponentGetter<TextMeshProUGUI>(
            TypeOfGetter.ChildByName,
            "Magic Slot List/Level Up Slot/Magic Slot/Level Text"
        );

        private ComponentGetter<Image> _originalMagicImage = new ComponentGetter<Image>(
            TypeOfGetter.ChildByName,
            "Magic Slot List/Change Slot/Prev Magic Slot"
        );

        private ComponentGetter<TextMeshProUGUI> _originalLevelText = new ComponentGetter<TextMeshProUGUI>(
            TypeOfGetter.ChildByName,
            "Magic Slot List/Change Slot/Prev Magic Slot/Level Text"
        );

        private ComponentGetter<Button> _levelUpButton = new ComponentGetter<Button>(
            TypeOfGetter.ChildByName,
            "Button Panel/Level Up Button"
        );

        private ComponentGetter<Button> _changeButton = new ComponentGetter<Button>(
            TypeOfGetter.ChildByName,
            "Button Panel/Change Button"
        );

        private TileMagicType _originalMagicType;

        public void Init() {
            gameObject.SetActive(false);

            
        }

        public void Set(
            int originalLevel,
            TileMagicType originalMagicType,
            Action<TileMagicType> onLevelUp,
            Action onChange
        ) {
            _levelUpButton.Get(gameObject).onClick.RemoveAllListeners();
            _changeButton.Get(gameObject).onClick.RemoveAllListeners();
            
            _levelUpButton.Get(gameObject).onClick.AddListener(() => {
                gameObject.SetActive(false);
                onLevelUp(_originalMagicType);
            });

            _changeButton.Get(gameObject).onClick.AddListener(() => {
                gameObject.SetActive(false);
                onChange();
            });

            _originalMagicType = originalMagicType;

            _levelUpMagicTargetLevelText.Get(gameObject).text = (originalLevel + 1).ToString();
            
            _originalLevelText.Get(gameObject).text = originalLevel.ToString();
        }
    }
}

