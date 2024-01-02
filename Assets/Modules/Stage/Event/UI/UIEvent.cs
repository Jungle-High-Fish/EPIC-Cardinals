using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.Game
{
    /// <summary>
    /// 스테이지 정보 내, 하나의 사건을 표시하는 UI 스크립트
    /// </summary>
    public class UIEvent : MonoBehaviour
    {
        private UIStageMap _map;
        [SerializeField] private Image _iconImg;
        [SerializeField] private GameObject _clearObj;
        
        public void Init(UIStageMap map, BaseEvent evt)
        {
            _map = map;
            evt.UIEvent = this;

            _iconImg.sprite = ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_MapIcons + evt.Type);

            _clearObj.SetActive(false);
        }

        public IEnumerator On(bool isStartEvent)
        {
            yield return _map.MovePlayerIcon(this, isStartEvent);
        }

        public void ImmediateOn(bool isStartEvent) {
            _map.SetPlayerImmediate(this, isStartEvent);
        }
        
        public IEnumerator Clear()
        {
            bool completeDO = false;
            _clearObj.SetActive(true);
            _clearObj.transform.localScale = Vector3.one;
            _clearObj.transform.DOPunchScale(Vector3.one, 0.5f, 2, 1)
                .OnComplete(() => { completeDO = true; });

            yield return new WaitUntil(() => completeDO );
        }

        public void ImmediateClear()
        {
            _clearObj.transform.DOComplete();
            _clearObj.SetActive(true);
            _clearObj.transform.localScale = Vector3.one;
        }
    }
}
