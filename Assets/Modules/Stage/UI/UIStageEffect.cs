using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Cardinals.UI
{
    public class UIStageEffect : MonoBehaviour
    {
        [Header("Component")]
        [SerializeField] private RectTransform _topCurtainTr; 
        [SerializeField] private RectTransform _bottomCurtainTr;

        [Button]
        private void Test()
        {
            StartCoroutine(CurtainON());
        }
        public IEnumerator CurtainON()
        {
            bool wait = false;
            _topCurtainTr.DOLocalMoveY(-100, .5f).SetEase(Ease.Linear);
            _bottomCurtainTr.DOLocalMoveY(100, .5f).SetEase(Ease.Linear)
                .OnComplete(() => wait = true);
            yield return new WaitUntil(() => wait);

            yield return new WaitForSeconds(1f);
            yield return CurtainOFF();
        }
        
        public IEnumerator CurtainOFF()
        {
            bool wait = false;
            
            _topCurtainTr.DOLocalMoveY(100, .5f).SetEase(Ease.Linear);
            _bottomCurtainTr.DOLocalMoveY(-100, .5f).SetEase(Ease.Linear)
                .OnComplete(() => wait = true);
            yield return new WaitUntil(() => wait);
        }
    }
}