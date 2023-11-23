using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cardinals.Enums;
using Util;

namespace Cardinals.Game {
    public class Artifact
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
    }
}
