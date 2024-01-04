using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Board;
using Cardinals.Enums;
using Cardinals.UI.Description;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
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

        private ComponentGetter<RectTransform> _levelUpSlot = new ComponentGetter<RectTransform>(
            TypeOfGetter.ChildByName,
            "Magic Slot List/Level Up Slot"
        );

        private ComponentGetter<RectTransform> _changeSlot = new ComponentGetter<RectTransform>(
            TypeOfGetter.ChildByName,
            "Magic Slot List/Change Slot"
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
            TransitionAnimation();
            
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

            _levelUpMagicImage.Get(gameObject).sprite = TileMagic.Data(originalMagicType).sprite;
            _originalMagicImage.Get(gameObject).sprite = TileMagic.Data(originalMagicType).sprite;

            _levelUpMagicTargetLevelText.Get(gameObject).text = $"Lv.{originalLevel + 1}";
            _originalLevelText.Get(gameObject).text = $"Lv.{originalLevel}";
            
            // 설명 창 추가
            var tr = _levelUpMagicImage.Get(gameObject).transform;
            
            //기존 설명 제거
            var descs = tr.GetComponents<BaseDescription>();
            for (int i = descs.Length - 1; i >= 0; i--)
            {
                descs[i].Delete();
            }
            
            tr.AddComponent<MagicDescription>().Init(originalMagicType);
        }

        public void TransitionAnimation() {
            _levelUpSlot.Get(gameObject).localScale = Vector3.zero;
            _changeSlot.Get(gameObject).localScale = Vector3.zero;
            _levelUpButton.Get(gameObject).transform.localScale = Vector3.zero;
            _changeButton.Get(gameObject).transform.localScale = Vector3.zero;
            gameObject.SetActive(true);

            _levelUpSlot.Get(gameObject).DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutCubic);
            _changeSlot.Get(gameObject).DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutCubic).SetDelay(0.2f);
            _levelUpButton.Get(gameObject).transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutCubic);
            _changeButton.Get(gameObject).transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutCubic).SetDelay(0.2f);
        }
    }
}

