using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI
{
    [Tooltip("마우스 호버 시, 설명 창을 출력")]
    public class DescriptionConnector : MonoBehaviour, IPointerClickHandler
    {
        private bool _isOpen;
        public bool IsOpen 
        { 
            get => _isOpen;
            set => _isOpen = false; 
        }
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

        [SerializeField] private Transform _transform;
        private Transform _itemAreaTr
        {
            get
            {
                if (_transform == null)
                {
                    _transform = GetComponent<Transform>();
                    if (_transform == null)
                    {
                        _transform = GetComponent<RectTransform>();
                    }
                }

                return _transform;
            }
        }
        [SerializeField] private DescriptionArea _descriptionArea;
        [SerializeField] private HoverRenderType _hoverRenderType;
        [SerializeField] private CountType _countType;
        [SerializeField] private Anchor _anchor;
        private GameObject UIDescriptionPrefab => ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Description);

        public void Start()
        {
            if(_countType == CountType.Multi) Init();
            if (_anchor == default) _anchor = Anchor.Right;
        }

        public void Init()
        {
            // _itemAreaTr ??= this.transform;
            // _itemAreaTr ??= GetComponent<RectTransform>();
            
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

        private Action<Anchor, IDescription[]> _mouseEnterAction;
        private Action _mouseExitAction;

        private void InitSpriteSetting()
        {
            var items = _itemAreaTr.GetComponents<IDescription>();
            
            // _mouseEnterAction += _descriptionArea.OnPanel;
            // _mouseExitAction += _descriptionArea.OffPanel;
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

        // private void OnMouseEnter()
        // {
        //     _mouseEnterAction?.Invoke(_anchor, GetIDescription());
        // }
        //
        // private void OnMouseExit()
        // {
        //     _mouseExitAction?.Invoke();
        // }

        // public void OnPointerEnter(PointerEventData eventData)
        // {
        //     _mouseEnterAction?.Invoke(_anchor, GetIDescription());
        // }
        //
        // public void OnPointerExit(PointerEventData eventData)
        // {
        //     _mouseExitAction?.Invoke();
        // }

        /// <summary>
        /// 모바일용 설명창 출력
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isOpen)
            {
                _descriptionArea.OnPanel(this, _anchor, GetIDescription());
                // _mouseEnterAction?.Invoke(_anchor, GetIDescription());
            }
            else
            {
                _mouseExitAction?.Invoke();
            }

            _isOpen = !_isOpen;
        }

        private void OnDisable() {
            if (_hoverRenderType == HoverRenderType.Sprite) {
                _mouseExitAction?.Invoke();
            } else {
                if (_descriptionArea != null) {
                    _descriptionArea.OffPanel();
                }
            }
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
            }

            var entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((eventData) =>
                {
                    //OnPanel();
                    if (!_isOpen)
                                {
                                    _descriptionArea.OnPanel(this, _anchor, GetIDescription());
                                    // _mouseEnterAction?.Invoke(_anchor, GetIDescription());
                                }
                                else
                                {
                                    _mouseExitAction?.Invoke();
                                }
                    
                                _isOpen = !_isOpen;
                } );
                trigger.triggers.Add(entry);
                
                // entry = new EventTrigger.Entry();
                // entry.eventID = EventTriggerType.PointerExit;
                // entry.callback.AddListener( (eventData) => { OffPanel(); } );
                // trigger.triggers.Add(entry);
        }

        public void OnPanel()
        {
            _descriptionArea.OnPanel(this, _anchor, GetIDescription());
        }

        public void OffPanel()
        {
            _descriptionArea.OffPanel();
        }
        #endregion

        public void RequestOnDescription(IDescription description)
        {
            _descriptionArea.UpdatePanel(_anchor, description);
            _descriptionArea.OnPanel(this);
        }

        public void OnTransformChildrenChanged()
        {
            if (_countType == CountType.Multi)
            {
                _descriptionArea.UpdatePanel(_anchor, GetIDescription());
            }
        }
    }
}
