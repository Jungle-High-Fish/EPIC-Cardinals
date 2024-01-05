using DG.Tweening;
using Febucci.UI;
using Febucci.UI.Core;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Steamworks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;

namespace Cardinals.Title
{
    public class CreditController : MonoBehaviour
    {
        [Header("Component")]
        [SerializeField] private GameObject _mainCanvas;
        [SerializeField] private GameObject _CreditCanvas;
        [SerializeField] private GameObject _panel;
        [SerializeField] private GameObject _highFish;
        [SerializeField] private GameObject _coach;
        [SerializeField] private GameObject _tk;
        [SerializeField] private GameObject _specialThanks;
        [SerializeField] private TextMeshProUGUI[] _nameTMPs;
        
        [Header("Enemies Component")]
        [SerializeField] private RectTransform _enemiesTr;
        [SerializeField] private Image _pipiIMG;
        [SerializeField] private Image _popoIMG;
        [SerializeField] private Sprite[] _pipiSprites;
        [SerializeField] private Sprite[] _popoSprites;

        [Header("Data")] [SerializeField] private string[] _specialThanksNames;

        [Button]
        public void ShowCredit()
        {
            StartCoroutine(CreditFlow());
        }

        IEnumerator CreditFlow()
        {;
            var title = FindObjectOfType<TitleManager>();
            var player = title.PlayerControlInTitle;
            bool next;

            // 초기화
            _enemiesTr.localPosition = new Vector2(0, -200);
            _pipiIMG.sprite = _pipiSprites[0];
            _popoIMG.sprite = _popoSprites[0];
           // _panel.SetActive(false);
            _highFish.SetActive(false);
            _coach.SetActive(false);
            _tk.SetActive(false);
            _specialThanks.SetActive(false);
            
            // 액션
            _mainCanvas.SetActive(false);
            _CreditCanvas.SetActive(true);
            _enemiesTr.DOLocalMoveY(-70, 2f).OnComplete(() => { next = true;});
            next = false;
            yield return new WaitUntil(() => next);

            player.CreditMode();
            player.transform.DOPunchScale(new Vector3(.2f, .2f), .5f, 1);
            _pipiIMG.sprite = _pipiSprites[1];
            _popoIMG.sprite = _popoSprites[1];
            
            // _panel.SetActive(true);
            // _panel.transform.localScale = Vector3.zero;
            // _panel.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutElastic).OnComplete(() => { next = true;});
            // next = false;
            // yield return new WaitUntil(() => next);
            yield return new WaitForSeconds(1f);

            yield return Effect(_highFish);
            //yield return Effect(_coach);

            yield return EffectSpecialThanks();
            
            _tk.SetActive(true);
            
            var ach = new Achievement("Thank_You");
            ach.Trigger();
            
            yield return new WaitForSeconds(5f);
            _tk.GetComponentsInChildren<TypewriterByCharacter>().ForEach(tw => tw.StartDisappearingText());
            yield return new WaitForSeconds(2f);
            _tk.SetActive(false); 
            
            //_panel.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InElastic).OnComplete(() => { next = true; _panel.SetActive(false);});
            // next = false;
            // yield return new WaitUntil(() => next);
            
            _enemiesTr.DOLocalMoveY(-300, 2f).OnComplete(() => { next = true;});
            
            next = false;
            yield return new WaitUntil(() => next);
            _CreditCanvas.SetActive(false);
            _mainCanvas.SetActive(true);
            
            player.StartFlow();
        }

        private IEnumerator Effect(GameObject obj)
        {
            var img = obj.GetComponentInChildren<Image>();
            var alpha0 = Color.white;
            alpha0.a = 0;
            img.color = alpha0;
            var tmp = img.GetComponentInChildren<TextMeshProUGUI>();
            
            if (tmp != null)
            {
                Color tmpColor = tmp.color;
                tmpColor.a = 0;
                tmp.color = tmpColor;
            }
            
            obj.SetActive(true);
            img.DOFade(1,1f);
            tmp.DOFade(1,1f);
            yield return new WaitForSeconds(5f);
            
            obj.GetComponentsInChildren<TypewriterByCharacter>().ForEach(tw => tw.StartDisappearingText());
            img.DOFade(0,1f);
            tmp.DOFade(0,1f);
            yield return new WaitForSeconds(2f);
            obj.SetActive(false);
        }


        private IEnumerator EffectSpecialThanks()
        {
            if(_specialThanksNames.Length == 0) yield break;
            
            // Init
            _nameTMPs.ForEach(tmp => tmp.text = string.Empty);
            var header = _specialThanks.transform.Find("Header");
            header.gameObject.SetActive(false);

            _specialThanks.SetActive(true);
            header.gameObject.SetActive(true);

            yield return new WaitForSeconds(3f);
            header.GetComponent<TypewriterByCharacter>().StartDisappearingText();
            yield return new WaitForSeconds(2f);


            for (int i = 0; i < _specialThanksNames.Length; )
            {
                _nameTMPs.ForEach(tmp => tmp.text = string.Empty);
                for (int pivot = 0; pivot < 6 && i < _specialThanksNames.Length; pivot++, i++)
                {
                    _nameTMPs[pivot].gameObject.SetActive(false);
                    _nameTMPs[pivot].text = _specialThanksNames[i];
                    _nameTMPs[pivot].gameObject.SetActive(true);
                }
                yield return new WaitForSeconds(5f);
                _nameTMPs.ForEach(tmp => tmp.GetComponent<TypewriterByCharacter>().StartDisappearingText());
                yield return new WaitForSeconds(2f);
            }
        }
    }

}