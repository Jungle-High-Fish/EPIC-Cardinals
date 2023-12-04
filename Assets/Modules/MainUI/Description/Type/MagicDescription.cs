using System.Collections;
using System.Collections.Generic;
using Cardinals;
using Cardinals.Board;
using Cardinals.Enums;
using Cardinals.Game;
using Cardinals.UI;
using Unity.VisualScripting;
using UnityEngine;
using Util;

namespace Cardinals.UI.Description
{
    public class MagicDescription : MonoBehaviour, IDescription
    {
        private TileMagicDataSO _data;
        public string Name => _data.elementName;
        public string Description => _data.mainMagicDescription;
        public Sprite IconSprite => _data.uiSprite;
        public Color Color { get; set; }

        public string Key => _data.magicType.ToString();

        public void Init(TileMagicType magicType)
        {
            _data = TileMagic.Data(magicType);
            Color = _data.elementColor;
            
            if (_data.hasBuffEffect) {
                transform.AddComponent<BuffDescription>().Init(_data.buffType);
            }
        }
    }

}