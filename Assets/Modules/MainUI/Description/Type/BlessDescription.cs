using System.Collections;
using System.Collections.Generic;
using Cardinals;
using Cardinals.Enums;
using Cardinals.Game;
using Cardinals.UI;
using Unity.VisualScripting;
using UnityEngine;
using Util;

namespace Cardinals.UI.Description
{
    public class BlessDescription : BaseDescription, IDescription
    {
        private BlessDataSO _data;
        public string Name => TMPUtils.LocalizedText(_data.blessName);
        public string Description => TMPUtils.CustomParse(_data.description,true) ;
        public Sprite IconSprite => null;
        public Color Color { get; set; }

        public string Key => $"bless_{_data.blessType.ToString()}";

        public void Init(BlessType blessType)
        {
            _data = ResourceLoader.LoadSO<BlessDataSO>(Constants.FilePath.Resources.SO_BlessData + blessType);
            Color = ResourceLoader
                .LoadSO<TileMagicDataSO>(Constants.FilePath.Resources.SO_MagicData + _data.relatedMagicType)
                .elementColor;

            switch (blessType)
            {
                case BlessType.BlessFire1 : 
                case BlessType.BlessFire2 : 
                    transform.AddComponent<BuffDescription>().Init(BuffType.Burn);
                    break;
                default: break;
            }
        }
    }

}