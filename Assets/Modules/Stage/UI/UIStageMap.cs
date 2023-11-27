using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Cardinals.Game {
    public class UIStageMap: MonoBehaviour {
        private ObjectGetter _eventListObj
            = new ObjectGetter(TypeOfGetter.ChildByName, "Events");

        private List<UIEvent> _eventNodeList = new List<UIEvent>();

        public void Init(Stage stage) {
            DestroyAllNodes();

            GameObject eventNodePrefab 
                = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_EventNode);

            Transform parent = _eventListObj.Get(gameObject).transform;
            
            foreach(var evt in stage.Events) {
                GameObject eventNode = Instantiate(eventNodePrefab, parent);
                UIEvent uiEvent = eventNode.GetComponent<UIEvent>();
                uiEvent.Init(evt);
                _eventNodeList.Add(uiEvent);
            }
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

