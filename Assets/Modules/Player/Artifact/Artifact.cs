using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cardinals.Enums;
using Cardinals.UI;
using Util;

namespace Cardinals.Game {
    public class Artifact : IProduct
    {
        private int _money;
        private string _name;
        private ArtifactType _type;

        public ArtifactType Type
        {
            get => _type;
            set
            {
                _type = value;
            }
        }

        public ArtifactDataSO Data()
        {
            return ResourceLoader.LoadSO<ArtifactDataSO>(
                Constants.FilePath.Resources.SO_ArtifactData + Type
            );
        }

        public virtual void OnEffect()
        {

        }

        #region IProduct
        public Sprite Sprite => Data().sprite;
        public string Description => Data().description;
        public int Price => Data().price;
        public string Name => Data().artifactName;
        #endregion
    }
}
