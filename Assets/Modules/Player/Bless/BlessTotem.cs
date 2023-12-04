using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.UI;
using Cardinals.UI.Description;
using UnityEngine;
using Util;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using Cardinals.Tutorial;

namespace Cardinals.Board
{
    public class BlessTotem : MonoBehaviour
    {
        private BlessType _baseBless;
        
        [Header("Component")]
        private SpriteRenderer _totemRenderer;
        
        public void Awake()
        {
            _totemRenderer = GetComponentInChildren<SpriteRenderer>();
        } 

        public void Init(BlessType bless)
        {   
            LookAtCamera(Camera.main.transform.position);

            Vector3 pos = transform.position + transform.up * 0.8f;
            transform.position = pos + transform.up * 10f;
            transform.localScale = Vector3.zero;
            transform.DOMove(pos, 0.5f).SetEase(Ease.InQuint);
            transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutCubic);

            _baseBless = bless;

            var data = ResourceLoader.LoadSO<BlessDataSO>(Constants.FilePath.Resources.SO_BlessData + bless);
            _totemRenderer.sprite = data.totemSprite;

            transform.AddComponent<BlessDescription>().Init(bless);
            GetComponent<DescriptionConnector>().Init();
        }

        public IEnumerator Dismiss(BlessType bless, int idx, Action<int> onDismissed) {
            if (_baseBless != bless) {
                _totemRenderer.DOFade(0f, 0.3f).SetEase(Ease.InQuint);
                yield return new WaitForSeconds(0.3f);
                onDismissed?.Invoke(idx);
                yield break;
            }

            Vector3 pos = transform.position + transform.up * 10f;
            Vector3 focusPos = transform.position + transform.up * 0.3f;
            transform.DOMove(focusPos, 0.3f).SetEase(Ease.OutCubic);
            transform.DOScale(Vector3.one * 1.1f, 0.3f).SetEase(Ease.OutCubic);

            transform.DOMove(pos, 0.5f).SetEase(Ease.InQuint).SetDelay(1f);

            yield return new WaitForSeconds(1.5f);
            onDismissed?.Invoke(idx);
        }

        public void OnMouseDown()
        {
            GameManager.I.Stage.SelectedBless = _baseBless;
            if (GameManager.I.Stage.CurEvent is TutorialEvent tutorialEvent) {
                tutorialEvent.CheckBlessSelectQuest();
            }
        }

        private void LookAtCamera(Vector3 target)
        {
            transform.LookAt(target, Vector3.up);
            transform.Rotate(0, 180, 0);
        }
    }
}
