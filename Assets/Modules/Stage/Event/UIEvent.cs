using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Cardinals.Game
{
    /// <summary>
    /// 스테이지 정보 내, 하나의 사건을 표시하는 UI 스크립트
    /// </summary>
    public class UIEvent : MonoBehaviour
    {
        [SerializeField] private Image _iconImg;
        [SerializeField] private GameObject _clearObj;
        [SerializeField] private GameObject _curEvtObj;
        
        public void Init(BaseEvent evt)
        {
            evt.UIEvent = this;
            
            // [TODO] Resource Manager에서 Icon 정보 읽어서 설정하기
            // _iconImg.sprite = null; evt.Type

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
