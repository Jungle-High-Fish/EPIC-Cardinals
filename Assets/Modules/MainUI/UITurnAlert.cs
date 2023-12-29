using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Util;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Cardinals.UI {
    public class UITurnAlert: MonoBehaviour {
        private ComponentGetter<Image> _image
            = new ComponentGetter<Image>(TypeOfGetter.Child);

        [SerializeField] private float targetPositionY;

        public IEnumerator Show(Sprite sprite) {
            gameObject.SetActive(true);
            _image.Get(gameObject).sprite = sprite;

            RectTransform rectTransform = _image.Get(gameObject).transform as RectTransform;
            var initialPosition = new Vector2(0, 50);
            var targetPosition = new Vector2(0, -(rectTransform.rect.height + targetPositionY));

            rectTransform.anchoredPosition = initialPosition;
            _image.Get(gameObject).color = new Color(1f, 1f, 1f, 0f);
            _image.Get(gameObject).DOFade(1f, 0.5f);
            rectTransform.DOAnchorPos(targetPosition, 0.5f).SetEase(Ease.OutBack);
            //rectTransform.DOPunchScale(Vector3.one * 0.2f, 0.5f, 1, 0f);

            yield return new WaitForSeconds(0.5f);
        }

        public IEnumerator Hide() {
            _image.Get(gameObject).DOFade(0f, 0.5f).OnComplete(() => {
                gameObject.SetActive(false);
            });

            yield return new WaitForSeconds(0.5f);
        }

        [Button]
        private void TestShow() {
            gameObject.SetActive(true);
            StartCoroutine(Show(null));
        }

        [Button]
        private void TestHide() {
            StartCoroutine(Hide());
        }
    }
}
