using System.Collections;
using System.Collections.Generic;
using Cardinals.Game;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cardinals.Game
{
    public class UIStage : MonoBehaviour
    {
        [Header("사건 정보")]
        [SerializeField] private Transform _evtInstTr;
        [SerializeField] private GameObject _evtPrefab;

        public void Init(Stage stage)
        {
            // 제거
            for (int i = _evtInstTr.childCount - 1; i >= 0; i--)
            {
                Destroy(_evtInstTr.GetChild(i).gameObject);
            }

            // 추가
            foreach (var evt in stage.Events)
            {
                var obj = Instantiate(_evtPrefab, _evtInstTr);
                obj.GetComponent<UIEvent>().Init(evt.Type);
            }
        }
    }
}