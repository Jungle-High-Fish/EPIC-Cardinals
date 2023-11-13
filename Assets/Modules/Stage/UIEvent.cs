using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cardinals.Game
{
    public class UIEvent : MonoBehaviour
    {
        [SerializeField] private Image _iconImg;
        [SerializeField] private GameObject _clearObj;
        [SerializeField] private GameObject _curEvtObj;
        
        public void Init(EventType type)
        {
            // [TODO] Resource Manager에서 Icon 정보 읽어서 설정하기
            _iconImg.sprite = null;

            _clearObj.SetActive(false);
            _curEvtObj.SetActive(false);
        }

        public void On()
        {
            _curEvtObj.SetActive(true); 
        }
        
        public void Clear()
        {
            _clearObj.SetActive(true);
            _curEvtObj.SetActive(false);
        }
    }
}
