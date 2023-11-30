using System.Collections.Generic;
using Modules.Utils;
using Sirenix.Utilities;
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
            
            gameObject.SetActive(true);
            
            // 업데이트
            GetComponent<VerticalLayoutGroup>().Update();
            GetComponent<HorizontalLayoutGroup>().Update();
            GetComponent<GridLayoutGroup>().Update();
        }

        public void OffPanel()
        {
            UIDescriptionsDict.Values.ForEach(desc => desc.gameObject.SetActive(false));
            // foreach (var desc in _descriptions)
            // {
            //     UIDescriptionsDict[desc.Key].gameObject.SetActive(false);
            // }
        }
    }
}