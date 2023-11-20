using System.Collections;
using System.Collections.Generic;
using Cardinals.Game;
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
        private ComponentGetter<UIStageEnterAlert> _stageEnterAlert
            = new ComponentGetter<UIStageEnterAlert>(TypeOfGetter.ChildByName, "Stage Enter Alert");
        
        private ComponentGetter<UIStageMap> _stageMap
            = new ComponentGetter<UIStageMap>(TypeOfGetter.ChildByName, "Stage Map");

        public void Init(Stage stage)
        {
            _stageEnterAlert.Get(gameObject).Init(stage);
            _stageMap.Get(gameObject).Init(stage);
        }

        public IEnumerator Visit()
        {
            _stageEnterAlert.Get(gameObject).gameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            _stageEnterAlert.Get(gameObject).gameObject.SetActive(false);
        }

        public IEnumerator EventIntro()
        {
            _stageMap.Get(gameObject).gameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            _stageMap.Get(gameObject).gameObject.SetActive(false);
        }
    }
}