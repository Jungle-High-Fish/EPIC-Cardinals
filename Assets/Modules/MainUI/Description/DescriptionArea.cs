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


        public void InitCanvas()
        {
            gameObject.name = "Global Description Area";
            _canvasMode = true;
            
            var group = transform.AddComponent<VerticalLayoutGroup>();
            group.padding = new RectOffset(5, 5, 5, 5);
            group.childAlignment = TextAnchor.UpperLeft;
            group.childControlWidth = true;
            group.childForceExpandWidth = true;
            group.childForceExpandHeight = true;
            
            var fitter = transform.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            var rect = GetComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.pivot = new Vector2(0, 1);
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
        }

        public void Update()
        {
            if (_canvasMode && _isActive)
            {
                transform.position = Input.mousePosition;
            }
        }

        public void UpdatePanel(params IDescription[] descriptions)
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
                        
            uiDescription.Init(description);
            uiDescription.gameObject.SetActive(false);
            UIDescriptionsDict.Add(description.Key, uiDescription);
        }
        
        public void OnPanel(params IDescription[] descriptions )
        {
            if (_canvasMode)
            {   
                Vector2 mousePos = Input.mousePosition;
                transform.position = mousePos;
            }

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
            
            // 업데이트
            GetComponent<VerticalLayoutGroup>().Update();
            GetComponent<HorizontalLayoutGroup>().Update();
            GetComponent<GridLayoutGroup>().Update();
        }

        public void OffPanel()
        {
            _isActive = false;
            UIDescriptionsDict.Values.ForEach(desc => desc.gameObject.SetActive(false));
        }
    }
}