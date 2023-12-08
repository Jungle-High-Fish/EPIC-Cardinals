using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Util;

namespace Cardinals.Game {
    public class UIStageMap: MonoBehaviour {
        private ObjectGetter _eventListObj
            = new ObjectGetter(TypeOfGetter.ChildByName, "Events");

        private List<UIEvent> _eventNodeList = new List<UIEvent>();

        [SerializeField] private Transform _playerIconTr;
        
        public void Init(Stage stage) {
            gameObject.SetActive(true);
            DestroyAllNodes();
            
            Transform parent = _eventListObj.Get(gameObject).transform;

            float interval =  (parent.localPosition.magnitude * 2) / (stage.Events.Length - 1);

            InstantiateEventItem(parent, stage.Events[0], 0);
            for (int i = 1, cnt = stage.Events.Length; i < cnt; i++)
            {
                InstantiateEventItem(parent, stage.Events[i], interval * i);
            }

            _playerIconTr.gameObject.SetActive(false);
            _playerIconTr.SetSiblingIndex(100);
        }

        /// <summary>
        /// 각 사건들에 해당하는 오브젝트를 생성하며, 지정된 x 값에 배치
        /// </summary>
        private void InstantiateEventItem(Transform parent, BaseEvent evt, float x)
        {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Map_EventNode);
            GameObject eventNode = Instantiate(prefab, parent);
            eventNode.transform.localPosition = new Vector3( x , 0, 0);
                
            UIEvent uiEvent = eventNode.GetComponent<UIEvent>();
            uiEvent.Init(this, evt);
            _eventNodeList.Add(uiEvent);
        }

        /// <summary>
        /// 플레이어를 이동
        /// </summary>
        /// <param name="curEvt"></param>
        /// <returns></returns>
        public IEnumerator MovePlayerIcon(UIEvent curEvt)
        {
            bool completeDO = false;

            Vector3 targetPos;
            var index = _eventNodeList.IndexOf(curEvt);

            // 처음 이동 시, 뿅 나타남
            if (index == 0)
            {
                targetPos = _eventNodeList[index].transform.localPosition;
                _playerIconTr.gameObject.SetActive(true);
                _playerIconTr.localPosition = targetPos;
                _playerIconTr.localScale = Vector3.zero;
                _playerIconTr.DOScale(1, 1f).SetEase(Ease.OutElastic)
                    .OnComplete(() => { completeDO = true; });
            }
            else // 다음 이벤트 이동 시, 이동 효과
            {
                targetPos = _eventNodeList[index].transform.position;
                _playerIconTr.DOJump(targetPos, 1, 1,1f).OnComplete(() => { completeDO = true;});
            }
                
            yield return new WaitUntil(() => completeDO);
        }
        
        

        public void DestroyAllNodes() {    
            if (_eventNodeList == null) return;

            foreach(var node in _eventNodeList) {
                Destroy(node.gameObject);
            }

            _eventNodeList.Clear();
        }
    }
}

