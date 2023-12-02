using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.Game;
using UnityEngine;
using Util;

namespace Cardinals.UI.Description
{
    public class ArtifactDescription : MonoBehaviour, IDescription
    {
        private ArtifactDataSO _data;
        public void Init(ArtifactType type)
        {
            _data = ResourceLoader.LoadSO<ArtifactDataSO>(Constants.FilePath.Resources.SO_ArtifactData + type);
        }

        public string Name => _data.artifactName;
        public string Description => _data.description;
        public Sprite IconSprite { get; }
        public Color Color { get; }
        public string Key => _data.artifactType.ToString();
    }
}