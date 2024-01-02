using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.Game;
using UnityEngine;
using Util;

namespace Cardinals
{
    public abstract class Potion: IProduct
    {
        public PotionType PotionType => _potionType;
        protected PotionType _potionType;

        public virtual bool UsePotion()
        {
            return true;
        }

        #region IProduct
        public Sprite Sprite => Data().sprite;
        public string Description => Data().description;
        public int Price => Data().price;
        public string Name => Data().potionName;
        #endregion

        private PotionDataSO Data() {
            return ResourceLoader.LoadSO<PotionDataSO>(
                Constants.FilePath.Resources.SO_PotionData + _potionType
            );
        }
    }
}
