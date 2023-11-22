using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

using Cardinals.Game;
using Cardinals.Enums;

namespace Cardinals.UI
{
    public class UIArtifact: MonoBehaviour, IDescription
    {
        public string Name => _artifactDataSO.artifactName;
        public string Description => _artifactDataSO.description;
        public Sprite IconSprite => _artifactDataSO.sprite;
        public Transform InstTr => transform;

        private ArtifactDataSO _artifactDataSO;

        private ComponentGetter<Image> _icon = new ComponentGetter<Image>(
            TypeOfGetter.ChildByName,
            "Icon"
        );

        public void Init(ArtifactType artifactType)
        {
            _artifactDataSO = ResourceLoader.LoadSO<ArtifactDataSO>(
                Constants.FilePath.Resources.SO_ArtifactData + artifactType
            );

            _icon.Get(gameObject).sprite = _artifactDataSO.sprite;
        }
    }
}
