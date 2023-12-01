using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using UnityEngine;
using Util;

namespace Cardinals.UI.Description
{
    public class BuffDescription : MonoBehaviour, IDescription
    {
        private BuffDataSO _data;
        public void Init(BuffType type)
        {
            _data = ResourceLoader.LoadSO<BuffDataSO>(Constants.FilePath.Resources.SO_BuffData + type);
        }

        public string Name => _data.buffName;
        public string Description => _data.description;
        public Sprite IconSprite => _data.sprite;
        public Color Color { get; }
        public string Key => _data.type.ToString();
    }
}