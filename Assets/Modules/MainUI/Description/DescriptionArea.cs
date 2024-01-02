using System;
using System.Collections.Generic;
using Modules.Utils;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI
{
    public class DescriptionArea : MonoBehaviour
    {
        private IDescription[] _descriptions;
        private Dictionary<string, UIDescription> UIDescriptionsDict = new();
        
        [Header("Mode")]
        [SerializeField] private bool _canvasMode;

        private bool _isActive;
        private Anchor _curAnchor;


        public void Update()
        {
            TraceCursor();
        }
        
        #region Canvas Setting
        public void InitCanvas()
        {
            gameObject.name = "Global Description Area";
            _canvasMode = true;
            
            var group = transform.AddComponent<VerticalLayoutGroup>();
            group.padding = new RectOffset(5, 5, 5, 5);
            group.childAlignment = TextAnchor.UpperLeft;
            group.childControlWidth = false;
            group.childControlHeight = false;
            group.childForceExpandWidth = true;
            group.childForceExpandHeight = true;
            group.spacing = 10;
            
            var fitter = transform.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

            var rect = GetComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.pivot = new Vector2(0, 1);
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
        }

        void TraceCursor()
        {
            if (_canvasMode && _isActive)
            {
                transform.position = Input.mousePosition + (_curAnchor is Anchor.Right ? new Vector3(50, 0, 0) : Vector2.zero);
            }
        }
        #endregion
        

        public void UpdatePanel(Anchor anchor, params IDescription[] descriptions)
        {
            if (descriptions != null)
            {
                foreach (var desc in descriptions)
                {
                    if (!UIDescriptionsDict.ContainsKey(desc.Key))
                    {
                        InstantiateItem(desc);
                    }
                }
            }
            
            _descriptions = descriptions;
        }

        private void InstantiateItem(IDescription description)
        {
            var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Description);
            var uiDescription = Instantiate(prefab, transform).GetComponent<UIDescription>();

            if (description.Key.StartsWith("bless_"))
            {
                uiDescription.transform.SetSiblingIndex(0);
            }
            uiDescription.Init(description);
            uiDescription.gameObject.SetActive(false);
            UIDescriptionsDict.Add(description.Key, uiDescription);
        }
        
        public void OnPanel(Anchor anchor = Anchor.Right, params IDescription[] descriptions)
        {
            _curAnchor = anchor;
            // if (_canvasMode)
            // {
            //     Vector2 mousePos = Input.mousePosition;
            //     transform.position = mousePos + (anchor is Anchor.Right ? new Vector3(50, 0, 0) : Vector2.zero);
            // }

            if (descriptions != null)
            {
                _descriptions = descriptions;
            }
            
            if (_descriptions != null)
            {
                foreach (var desc in _descriptions)
                {
                    if (!UIDescriptionsDict.ContainsKey(desc.Key))
                    {
                        InstantiateItem(desc);
                    }
                    UIDescriptionsDict[desc.Key].gameObject.SetActive(true);
                }
            }

            _isActive = true;
            gameObject.SetActive(true);

            UIUpdate(anchor);
        }

        void UIUpdate(Anchor anchor)
        {
            var rect = GetComponent<RectTransform>();

            rect.pivot = anchor switch
            {
                Anchor.Left => new Vector2(1, 1),
                Anchor.Right => new Vector2(0, 1),
                _ => Vector2.right
            };
            
            // 업데이트
            GetComponent<VerticalLayoutGroup>().Update();;
            // GetComponent<HorizontalLayoutGroup>().Update();
            GetComponent<GridLayoutGroup>().Update();
            GetComponent<ContentSizeFitter>().Update();
        }

        public void OffPanel()
        {
            _isActive = false;
            UIDescriptionsDict.Values.ForEach(desc => desc.gameObject.SetActive(false));
        }
    }
}