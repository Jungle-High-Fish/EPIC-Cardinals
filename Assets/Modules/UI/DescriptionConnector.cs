using UnityEngine;
using UnityEngine.EventSystems;
using Util;

namespace Cardinals.UI
{
    [Tooltip("마우스 호버 시, 설명 창을 출력")]
    public class DescriptionConnector : MonoBehaviour
    {
        private IDescription _description;
        private GameObject _instObject;
        
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
            if (_instObject == null)
            {
                _description ??= GetComponent<IDescription>();
                if (_description != null)
                {
                    var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Description);
                    _instObject = Instantiate(prefab, _description.InstTr);
                    _instObject.GetComponent<UIDescription>().Init(_description);
                }
            }
            
            _instObject?.SetActive(true);
        }

        void OnTriggerExitCallback()
        {
            _instObject?.SetActive(false);
        }
    }
}
