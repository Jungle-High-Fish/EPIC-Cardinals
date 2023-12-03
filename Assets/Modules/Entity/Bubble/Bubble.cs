using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using TMPro;
using UnityEngine;

namespace Cardinals.Entity.UI
{
    public class Bubble : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textTMP;
        [SerializeField] private TypewriterByCharacter _typewriter;
        [SerializeField] private GridSizeUpdator _resizer;

        private IEnumerator coroutine;

        private bool _isInit;

        private void Init()
        {
            if (!_isInit)
            {
                _typewriter.onTextShowed.AddListener(TextEnd);
                _isInit = true;
            }
        }

        [Button]
        public void SetBubble(string text)
        {
            Init();
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            
            gameObject.SetActive(true);
            _textTMP.text = text;
            _resizer.Resizing();
            
            // 신규 텍스트 설정 및 재생
            _typewriter.StartShowingText(true);
        }
        
        private void TextEnd()
        {
            coroutine = Wait();
            StartCoroutine(coroutine);
        }

        private IEnumerator Wait()
        {
            float waitTime = 1.5f;
            do
            {
                yield return new WaitForSeconds(.1f);
                waitTime -= .1f;
            } while (waitTime  > 0);

            if (coroutine != null)
            {
                gameObject.SetActive(false);
                coroutine = null;
            }
        }
    }
}
