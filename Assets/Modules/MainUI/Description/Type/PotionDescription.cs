using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.Game;
using UnityEngine;
using Util;

namespace Cardinals.UI.Description
{
    public class PotionDescription : BaseDescription, IDescription
    {
        private PotionDataSO _data;
        public void Init(PotionType type)
        {
            _data = ResourceLoader.LoadSO<PotionDataSO>(Constants.FilePath.Resources.SO_PotionData + type);
        }

        public string Name => TMPUtils.LocalizedText(_data.potionName);
        public string Description => TMPUtils.LocalizedText(_data.description);
        public Sprite IconSprite { get; }
        public Color Color { get; }
        public string Key => _data.potionType.ToString();
    }
}