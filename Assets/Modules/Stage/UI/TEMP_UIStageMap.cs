using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Cardinals.Game {
    public class TEMP_UIStageMap: MonoBehaviour 
    {
        private ObjectGetter _eventListObj
            = new ObjectGetter(TypeOfGetter.ChildByName, "Events");

        [SerializeField] private List<Transform> _events = new();

        public void Init()
        {
            foreach (var evt in _events)
            {
                evt.GetChild(0).gameObject.SetActive(false);
                evt.GetChild(1).gameObject.SetActive(false);
            }
        }

        public void StartEvent(int index)
        {
            _events[index].GetChild(0).gameObject.SetActive(true);
        }
        
        public void ClearEvent(int index)
        {
            _events[index].GetChild(0).gameObject.SetActive(false);
            _events[index].GetChild(1).gameObject.SetActive(true);
        }

        public void On()
        {
            gameObject.SetActive(true);
        }
    }
}

