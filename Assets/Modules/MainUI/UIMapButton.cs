using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cardinals
{
    public class UIMapButton : MonoBehaviour
    {
        // Start is called before the first frame update
        public void Init()
        {
           GetComponentInChildren<Button>().onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            GameManager.I.UI.TEMP_UIStageMap.On();
        }

        public void On()
        {
            gameObject.SetActive(true);
        }
    }
}
