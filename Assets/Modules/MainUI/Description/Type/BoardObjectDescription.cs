using System.Collections;
using System.Collections.Generic;
using Cardinals.BoardEvent;
using Cardinals.Enums;
using UnityEngine;
using Util;


namespace Cardinals.UI.Description
{
    public class BoardObjectDescription : MonoBehaviour, IDescription
    {
        private NewBoardObjectDataSO _data;
        public void Init(NewBoardObjectDataSO data)
        {
            _data = data;
        }

        public string Name => _data.eventName;
        public string Description => _data.description;
        public Sprite IconSprite => null;
        public Color Color => default;
        public string Key => $"boardEvent_{_data.evtType}_{_data.objType}";
    }
}