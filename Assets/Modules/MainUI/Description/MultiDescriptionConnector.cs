using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;

namespace Cardinals.UI
{
    [Tooltip("마우스 호버 시, 설명 창을 출력")]
    public class MultiDescriptionConnector : MonoBehaviour
    {
        [SerializeField][ReadOnly] private Transform _instTr;
        
        public void Awake()
        {
            var trigger = GetComponent<EventTrigger>();

            if (trigger == null)
            {
                trigger = gameObject.AddComponent<EventTrigger>();
            
                var entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener( (eventData) => { OnPointEnterCallback(); } );
                trigger.triggers.Add(entry);
                
                entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerExit;
                entry.callback.AddListener( (eventData) => { OnTriggerExitCallback(); } );
                trigger.triggers.Add(entry);
            }
        }

        void OnPointEnterCallback()
        {
            if (_instTr != null)
            {
                if (_instTr.childCount > 0)
                {
                    for (int i = _instTr.childCount - 1; i >= 0; i--)
                    {
                        Destroy(_instTr.GetChild(i).gameObject);
                    }
                }
            }
            
            var descs = GetComponentsInChildren<IDescription>();
                
            // 생성할 객체 위치 설정
            // var descIstTrInfo = GetComponentInParent<IDescriptionInstTrInfo>();
            // _instTr = descIstTrInfo != null ? descIstTrInfo.DescriptionInstTr : descs.FirstOrDefault().InstTr;
            //
            // foreach (var desc in descs)
            // {
            //     var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Description);
            //
            //  
            //     var obj = Instantiate(prefab, _instTr.transform);
            //     obj.GetComponent<UIDescription>().Init(desc);
            // }
            
            _instTr.gameObject.SetActive(true);
        }

        void OnTriggerExitCallback()
        {
            _instTr.gameObject.SetActive(false);
        }
    }
}
