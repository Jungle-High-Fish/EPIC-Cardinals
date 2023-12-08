using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Game;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Util;

namespace Cardinals.Game
{
    /// <summary>
    /// 스테이지 정보를 표시하는 UI 스크립트
    /// </summary>
    public class UIStage : MonoBehaviour
    {
        private UIStageEnterAlert _stageEnterAlert;
        private UIStageMap _stageMap;

        public void Start()
        {
            _stageEnterAlert = GetComponentInChildren<UIStageEnterAlert>();
            _stageMap = GetComponentInChildren<UIStageMap>();
            
            _stageEnterAlert.gameObject.SetActive(false);
            _stageMap.gameObject.SetActive(false);
        }

        public IEnumerator Init(Stage stage)
        {
            yield return _stageEnterAlert.Init(stage);
            _stageMap.Init(stage);

            yield return null;
        }

        public IEnumerator Visit()
        {
            bool completeDO = false;
            _stageEnterAlert.gameObject.SetActive(true);
            _stageEnterAlert.transform.DOScale(Vector3.one, 1.5f)
                .OnComplete(() =>
                {
                    _stageEnterAlert.gameObject.SetActive(false);
                    completeDO = true;
                });

            yield return new WaitUntil(() => completeDO);
        }
    }
}