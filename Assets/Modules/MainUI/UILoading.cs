using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI
{
    public class UILoading : MonoBehaviour {
        private ComponentGetter<Image> _background
            = new ComponentGetter<Image>(TypeOfGetter.ChildByName, "Loading Panel");
        private ComponentGetter<RectTransform> _flavorArea
            = new ComponentGetter<RectTransform>(TypeOfGetter.ChildByName, "Loading Panel/Flavor Area");
        private ComponentGetter<RectTransform> _loadingTextArea
            = new ComponentGetter<RectTransform>(TypeOfGetter.ChildByName, "Loading Panel/Loading Text Area");
        private ComponentGetter<Animator> _animation
            = new ComponentGetter<Animator>(TypeOfGetter.ChildByName, "Loading Panel/Loading Text Area/Loading");

        public IEnumerator Show() {
            gameObject.SetActive(true);
            DontDestroyOnLoad(gameObject);
            Color clear = new Color(1f, 1f, 1f, 0f);
            Color background = _background.Get(gameObject).color;
             _loadingTextArea.Get(gameObject).gameObject.SetActive(false);

            background.a = 0f;
            _background.Get(gameObject).color = background;
            
            List<Image> flavorImageList = new List<Image>();
            flavorImageList.AddRange(_flavorArea.Get(gameObject).GetComponentsInChildren<Image>());
            foreach (var flavorImage in flavorImageList) {
                flavorImage.color = clear;
            }

            _background.Get(gameObject).DOFade(1f, 0.7f);
            yield return new WaitForSeconds(0.2f);
            foreach (var flavorImage in flavorImageList) {
                flavorImage.DOFade(1f, 0.5f);
            }
            yield return new WaitForSeconds(0.3f);
            _loadingTextArea.Get(gameObject).gameObject.SetActive(true);
            _animation.Get(gameObject).Play("Roll");
        }

        public IEnumerator Hide() {
            List<Image> flavorImageList = new List<Image>();
            flavorImageList.AddRange(_flavorArea.Get(gameObject).GetComponentsInChildren<Image>());
            foreach (var flavorImage in flavorImageList) {
                flavorImage.DOFade(0f, 0.5f);
            }
            yield return new WaitForSeconds(0.2f);
            _background.Get(gameObject).DOFade(0f, 0.7f);
            yield return new WaitForSeconds(0.3f);
            _loadingTextArea.Get(gameObject).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.4f);
            Destroy(gameObject);
            yield break;
        }
    }
}
