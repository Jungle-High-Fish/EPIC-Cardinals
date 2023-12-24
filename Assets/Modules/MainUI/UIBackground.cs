using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;
using DG.Tweening;

namespace Cardinals.UI {
    public class UIBackground : MonoBehaviour {
        private ComponentGetter<Image> _image
            = new ComponentGetter<Image>(TypeOfGetter.This);

        public void Init() {
            gameObject.SetActive(false);
        }

        public float Show(float duration = 0.5f) {
            gameObject.SetActive(true);
            _image.Get(gameObject).DOFade(0, 0);
            _image.Get(gameObject).DOFade(0.8f, duration);
            return duration;
        }

        public float Hide(float duration = 0f) {
            gameObject.SetActive(false);
            _image.Get(gameObject).DOFade(0, duration);
            return duration;
        }
    }
}