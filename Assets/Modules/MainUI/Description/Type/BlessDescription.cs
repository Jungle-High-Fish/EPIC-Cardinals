using System.Collections;
using System.Collections.Generic;
using Cardinals;
using Cardinals.Enums;
using Cardinals.UI;
using Unity.VisualScripting;
using UnityEngine;
using Util;

namespace Cardinals.UI.Description
{
    public class BlessDescription : MonoBehaviour, IDescription
    {
        private BlessDataSO _data;
        public string Name => _data.blessName;
        public string Description => _data.description;
        public Sprite IconSprite => null;
        public Color Color { get; }
        
        public string Key => _data.blessType.ToString();

        public void Init(BlessType blessType)
        {
            _data = ResourceLoader.LoadSO<BlessDataSO>(Constants.FilePath.Resources.SO_BlessData + blessType);

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