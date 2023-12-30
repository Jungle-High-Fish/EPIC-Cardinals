using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Util;

namespace Cardinals.UI
{
    public class UINotiBoardEventByTurn : MonoBehaviour
    {
        private ObjectGetter _turnNotiBubbleObj = new(TypeOfGetter.ChildByName, "Turn Noti");
        private ComponentGetter<TextMeshProUGUI> _turnNotiBubbleTMP = new(TypeOfGetter.ChildByName, "Turn Noti/TMP");
        private Dictionary<string, Func<string>> _turnNotiFuncDict = new();

        public void Init()
        {
            _turnNotiBubbleObj.Get(gameObject).SetActive(false);
        }
        
        public void AddTurnNoti(Func<string> func)
        {
            var key = func.Method.Name;
            var newlineKey = $"New Line({key})";
            
            if (!_turnNotiFuncDict.ContainsKey(key))
            {
                if (_turnNotiFuncDict.Any())
                {
                    _turnNotiFuncDict.Add(newlineKey, () => "\n");
                }
                _turnNotiFuncDict.Add(key, func);
            }
            else
            {
                // 기존 항목 제거 후 재 등록
                _turnNotiFuncDict.Remove(key);
                if (_turnNotiFuncDict.ContainsKey(newlineKey))
                {
                    _turnNotiFuncDict.Remove(newlineKey);
                }
                AddTurnNoti(func);
            }
        }
        
        public void PrintTurnNoti()
        {
            if (_turnNotiFuncDict.Any())
            {
                // 텍스트 추출
                var text = string.Empty;
                _turnNotiFuncDict.Values.ToList().ForEach(f => text += f());
                
                // 오브젝트 설정 
                var obj = _turnNotiBubbleObj.Get(gameObject);
                var tmp = _turnNotiBubbleTMP.Get(gameObject);
                
                obj.SetActive(true);
                tmp.text = text;
                obj.GetComponent<GridSizeUpdator>().Resizing();

                obj.transform.DOKill();
                obj.transform.localScale = Vector3.zero;

                var seq = DOTween.Sequence();
                seq.Append(obj.transform.DOLocalRotate(new Vector3(0, 0, -10f), .3f)
                    .OnComplete(() => obj.transform.DOLocalRotate(Vector3.zero, .5f).SetEase(Ease.OutBounce)));
                seq.Join(obj.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutElastic));
                seq.Append(obj.transform.DOScale(Vector3.one, 2f));
                seq.OnComplete(() => obj.SetActive(false));
                seq.Play();
            }
        }
    }
}