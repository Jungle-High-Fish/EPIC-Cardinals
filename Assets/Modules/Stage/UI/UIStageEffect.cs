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
            yield return new WaitForSeconds(2f);
            //yield return CurtainOFF();
        }
        
        public IEnumerator CurtainOFF()
        {
            yield return new WaitForSeconds(.5f);
        }
    }
}