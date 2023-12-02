using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Util;

namespace Cardinals.UI
{
    [Tooltip("마우스 호버 시, 설명 창을 출력")]
    public class DescriptionConnector : MonoBehaviour
    {
        private IDescription _description;
        private GameObject _instObject;
        
        enum HoverRenderType
        {
            None,
            Sprite,
            Image,
        }
        
        enum CountType
        {
            None,
            Single, //  
            Multi,  // 여러 오브젝트 정보를 한번에 띄우려 할 때 사용
        }

        [SerializeField] private Transform _itemAreaTr;
        [SerializeField] private DescriptionArea _descriptionArea;
        [SerializeField] private HoverRenderType _hoverRenderType;
        [SerializeField] private CountType _countType;
        private GameObject UIDescriptionPrefab => ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Description);

        public void Start()
        {
            if(_countType == CountType.Multi) Init();
        }

        public void Init()
        {
            _itemAreaTr ??= this.transform;
            _itemAreaTr ??= GetComponent<RectTransform>();
            
            _descriptionArea ??= GameManager.I.UI.DescCanvasDescArea;
            
            if (_hoverRenderType == HoverRenderType.Sprite)
            {
                InitSpriteSetting();
            }
            else
            {
                InitImageSetting();
            }
        }

        #region Sprite Setting

        private Action<IDescription[]> _mouseEnterAction;
        private Action _mouseExitAction;

        private void InitSpriteSetting()
        {
            var items = _itemAreaTr.GetComponents<IDescription>();
            
            _mouseEnterAction += _descriptionArea.OnPanel;
            _mouseExitAction += _descriptionArea.OffPanel;
        }

        IDescription[] GetIDescription()
        {
            return _countType switch
            {
                CountType.Single => _itemAreaTr.GetComponents<IDescription>(),
                CountType.Multi => _itemAreaTr.GetComponentsInChildren<IDescription>(),
                _ => null
            };
        }

        // public void OnPointerEnter(PointerEventData eventData)
        // {
        //     _mouseEnterAction?.Invoke(GetIDescription());
        // }
        //
        // public void OnPointerExit(PointerEventData eventData)
        // {
        //     _mouseExitAction?.Invoke();
        // }
        private void OnMouseEnter()
        {
            _mouseEnterAction?.Invoke(GetIDescription());
        }
        
        private void OnMouseExit()
        {
            _mouseExitAction?.Invoke();
        }
        #endregion

        #region Image Setting
        /// <summary>
        /// 이미지 타입 호버 시, 생성되는 설명창의 경우 사용
        /// </summary>
        private void InitImageSetting()
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
            _descriptionArea.OnPanel(GetIDescription());
        }

        void OnTriggerExitCallback()
        {
            _descriptionArea.OffPanel();
        }
        #endregion

        public void RequestOnDescription(IDescription description)
        {
            _descriptionArea.UpdatePanel(description);
            _descriptionArea.OnPanel();
        }

        public void OnTransformChildrenChanged()
        {
            if (_countType == CountType.Multi)
            {
                _descriptionArea.UpdatePanel(GetIDescription());
            }
        }
    }
}
