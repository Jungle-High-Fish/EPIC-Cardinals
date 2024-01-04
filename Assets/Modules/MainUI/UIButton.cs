using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util;
using DG.Tweening;

namespace Cardinals.UI {
    public class UIButton: MonoBehaviour {
        private ComponentGetter<Image> _outerImage
            = new(TypeOfGetter.This);
        private ComponentGetter<Image> _innerImage
            = new(TypeOfGetter.ChildByName, "Inner");
        private ComponentGetter<Button> _button
            = new(TypeOfGetter.This);
        private ComponentGetter<TMP_Text> _text
            = new(TypeOfGetter.ChildByName, "Text");

        private EventTrigger _eventTrigger;
        private Color _mainColor, _subColor;

        public void Init(Action onClick, bool isBlack=false) {
            _eventTrigger = gameObject.AddComponent<EventTrigger>();
            
            if (isBlack) {
                _mainColor = Constants.Common.Colors.CardinalsBlack;
                _subColor = Constants.Common.Colors.CardinalsWhite;
            } else {
                _mainColor = Constants.Common.Colors.CardinalsWhite;
                _subColor = Constants.Common.Colors.CardinalsBlack;
            }

            _button.Get(gameObject).onClick.RemoveAllListeners();
            _button.Get(gameObject).onClick.AddListener(() => {
                onClick?.Invoke();
            });

            SetColor();

            _eventTrigger.triggers.Add(OnMouseEnterTrigger());
            _eventTrigger.triggers.Add(OnMouseExitTrigger());
        }
        
        private void SetColor() {
            _outerImage.Get(gameObject).color = _subColor;
            _innerImage.Get(gameObject).color = _mainColor;
            _text.Get(gameObject).color = _subColor;
        }

        private EventTrigger.Entry OnMouseEnterTrigger() {
            var entry = new EventTrigger.Entry {
                eventID = EventTriggerType.PointerEnter
            };

            void Callback() {
                _innerImage.Get(gameObject).DOColor(_subColor, 0.1f);
                _text.Get(gameObject).DOColor(_mainColor, 0.1f);
            }

            entry.callback.AddListener((data) => {
                Callback();
            });

            return entry;
        }

        private EventTrigger.Entry OnMouseExitTrigger() {
            var entry = new EventTrigger.Entry {
                eventID = EventTriggerType.PointerExit
            };

            void Callback() {
                _innerImage.Get(gameObject).DOColor(_mainColor, 0.1f);
                _text.Get(gameObject).DOColor(_subColor, 0.1f);
            }

            entry.callback.AddListener((data) => {
                Callback();
            });

            return entry;
        }
    }
}