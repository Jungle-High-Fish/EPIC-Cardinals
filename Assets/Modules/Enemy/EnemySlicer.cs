using Destructible2D;
using Destructible2D.Examples;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Util;

namespace Cardinals.Enemy {
    public class EnemySlicer : MonoBehaviour {
        private ComponentGetter<SpriteRenderer> _renderer 
            = new(TypeOfGetter.Child);

        [SerializeField, Range(0f, 1f)] float _slicePos = 0.5f;
        [SerializeField] float _sliceLength = 3f;
        [SerializeField] Transform _sliceEffect;
        private SpriteRenderer _slicedRenderer;

        public void Init() {
            _sliceEffect.gameObject.SetActive(false);
        }

        public IEnumerator Slice() {
            GameObject slicedObj = Instantiate(
                _renderer.Get(gameObject).gameObject, 
                transform.position, 
                Quaternion.identity,
                _renderer.Get(gameObject).transform.parent
            );

            _slicedRenderer = slicedObj.GetComponent<SpriteRenderer>();

            _renderer.Get(gameObject).material.SetFloat("_ClipUvUp", _slicePos);
            _slicedRenderer.material.SetFloat("_ClipUvDown", 1 - _slicePos);

            yield return SliceCoroutine();
        }

        IEnumerator SliceCoroutine() {
            _slicedRenderer.transform.DOMove(
                _slicedRenderer.transform.position + new Vector3(0.3f, 1f, 0f),
                0.5f
            ).SetEase(Ease.OutCubic);
            _slicedRenderer.transform.DOLocalRotate(
                new Vector3(0f, 0f, -20f),
                0.5f
            ).SetEase(Ease.OutCubic);

            _sliceEffect.gameObject.SetActive(true);
            // var effectPos = _renderer.Get(gameObject).transform.position;
            // effectPos.x -= _renderer.Get(gameObject).bounds.size.x / 2f + _sliceLength / 2f;
            // effectPos.y -= _renderer.Get(gameObject).bounds.size.y / 2f;
            // effectPos.y += (1-_slicePos) * _renderer.Get(gameObject).bounds.size.y;
            var effectPos = new Vector3(-3f, _renderer.Get(gameObject).transform.parent.localPosition.y, 0f);

            var initialScale = Vector3.one;
            initialScale.x = 0.15f;
            //float targetYScale = _renderer.Get(gameObject).bounds.size.x + _sliceLength;
            float targetYScale = 6f;

            _sliceEffect.localPosition = effectPos + new Vector3(0f, -0.7f, 0f);
            _sliceEffect.localRotation = Quaternion.Euler(0f, 0f, -90f);
            _sliceEffect.localScale = initialScale;
            _sliceEffect.DOScaleY(targetYScale, 0.2f).SetEase(Ease.OutCubic).OnComplete(() => {
                _sliceEffect.gameObject.SetActive(false);
            });

            StartCoroutine(GameManager.I.CameraController.EnemyZoomIn(transform, 0.1f));
            yield return new WaitForSeconds(0.1f);
            Time.timeScale = 0.1f;
           
            yield return new WaitForSeconds(0.1f);
            Time.timeScale = 1f;
            
            
            RendererFade(_slicedRenderer, 0.5f);
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(GameManager.I.CameraController.ZoomOut(0.5f));
            RendererFade(_renderer.Get(gameObject), 0.7f);
            yield return new WaitForSeconds(0.7f);
        }

        private void RendererFade(SpriteRenderer target, float time) {
            target.material.SetFloat("_FadeAmount", -0.1f);
            DOTween.To(
                () => target.material.GetFloat("_FadeAmount"), 
                (x) => target.material.SetFloat("_FadeAmount", x), 
                1f, 
                time
            ).SetEase(Ease.OutCubic);
        }

        [Button]
        private void SliceTest() {
            StartCoroutine(Slice());
        }
    }
}
