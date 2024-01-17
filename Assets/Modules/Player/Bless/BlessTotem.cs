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
using IDescription = Cardinals.UI.IDescription;

namespace Cardinals.Board
{
    public class BlessTotem : MonoBehaviour
    {
        private BlessType _baseBless;
        
        [Header("Component")]
        private SpriteRenderer _totemRenderer;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private DescriptionArea _descriptionAreaL;
        [SerializeField] private DescriptionArea _descriptionAreaR;

        public Action SelectedEvent;
        
        public void Awake()
        {
            _totemRenderer = GetComponentInChildren<SpriteRenderer>();
        } 

        public void Init(BlessType bless, bool isLeft)
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
            // var dc = GetComponent<DescriptionConnector>();
            var descs = GetComponents<IDescription>();

            if (isLeft)
            {
                _descriptionAreaL.OnPanelFix(Anchor.Left , descriptions: descs);
            }
            else
            {
                _descriptionAreaR.OnPanelFix(Anchor.Right , descriptions: descs);
            }
        }

        public IEnumerator Dismiss(BlessType bless, int idx, Action<int> onDismissed) {
            _descriptionAreaL.OffPanel();
            _descriptionAreaR.OffPanel();
            
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
            if (GameManager.I.Stage.SelectedBless != default) return;
            SelectedEvent?.Invoke();
            
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

        public void SetDescription(bool state)
        {
            if (state)
            {
                
            }
            else
            {
                
            }
        }
    }
}
