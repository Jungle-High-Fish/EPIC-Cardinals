using System.Collections;
using System.Collections.Generic;
using Cardinals.Board;
using Cardinals.Enums;
using Cardinals.Game;
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

        public string Name => TMPUtils.LocalizedText(_data.buffName);
        public string Description => TMPUtils.LocalizedText(_data.Description);

        public Sprite IconSprite => _data.sprite;
        public Color Color { get; }
        public string Key => _data.type.ToString();
    }
}
