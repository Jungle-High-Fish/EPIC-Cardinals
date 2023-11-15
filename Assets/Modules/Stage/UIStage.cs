using System.Collections;
using System.Collections.Generic;
using Cardinals.Game;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cardinals.Game
{
    /// <summary>
    /// 스테이지 정보를 표시하는 UI 스크립트
    /// </summary>
    public class UIStage : MonoBehaviour
    {
        [Header("스테이지 정보")]
        [SerializeField] private GameObject _stageNameTxtObj;
        
        [Header("사건 정보")]
        [SerializeField] private Transform _evtInstTr;
        [SerializeField] private GameObject _evtPrefab;

        public void Init(Stage stage)
        {
            // 기존 항목 제거
            for (int i = _evtInstTr.childCount - 1; i >= 0; i--)
            {
                Destroy(_evtInstTr.GetChild(i).gameObject);
            }

            // 현재 스테이지 정보 설정
            _stageNameTxtObj.GetComponent<TextMeshProUGUI>()?.SetText(stage.Name);
            
            // 추가
            foreach (var evt in stage.Events)
            {
                var obj = Instantiate(_evtPrefab, _evtInstTr);
                obj.GetComponent<UIEvent>().Init(evt.Type);
            }
        }

        public void Visit()
        {
            StartCoroutine(ShowStageName());
        }

        IEnumerator ShowStageName()
        {
            _stageNameTxtObj.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            _stageNameTxtObj.SetActive(false);
        }
    }
}