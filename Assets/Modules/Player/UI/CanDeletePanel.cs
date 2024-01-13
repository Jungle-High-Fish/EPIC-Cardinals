using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI.Description
{
    public class CanDeletePanel : MonoBehaviour
    {
        private UIPotion _connectedPotion;
        private ComponentGetter<Button> _useBTN = new (TypeOfGetter.ChildByName, "UseBTN");
        private ComponentGetter<Button> _deleteBTN = new (TypeOfGetter.ChildByName, "DeleteBTN");
        
        public void Init()
        {
            gameObject.SetActive(false);
            
            _useBTN.Get(gameObject).onClick.AddListener(() =>
            {
                if (_connectedPotion != null)
                {
                    _connectedPotion.Use();
                }
            });
            
            
            _deleteBTN.Get(gameObject).onClick.AddListener(() =>
            {
                if (_connectedPotion != null)
                {
                    _connectedPotion.Delete();
                }
            });
            
            _useBTN.Get(gameObject).GetComponentInChildren<TextMeshProUGUI>().text
                = GameManager.I.Localization[LocalizationEnum.UI_ITEM_USE];
            _deleteBTN.Get(gameObject).GetComponentInChildren<TextMeshProUGUI>().text 
                = GameManager.I.Localization[LocalizationEnum.UI_ITEM_REMOVE];
        }

        public void SetPanelState(bool state, UIPotion uiPotion = null)
        {
            gameObject.SetActive(state);
            
            if (state)
            {
                transform.position = Input.mousePosition + new Vector3(0, -130, 0);
                _connectedPotion = uiPotion;
                _connectedPotion.GetComponent<DescriptionConnector>().DescriptionArea.CloseAction += ClosePanel;
            }
            else
            {
                _connectedPotion.GetComponent<DescriptionConnector>().DescriptionArea.CloseAction -= ClosePanel;
                _connectedPotion = null;
            }
        }

        
        void ClosePanel()
        {
            gameObject.SetActive(false);
        }
    }
}