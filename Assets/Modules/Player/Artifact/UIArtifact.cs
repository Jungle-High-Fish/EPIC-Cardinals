using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Cardinals.UI;
namespace Cardinals
{

    public class UIArtifact: MonoBehaviour, IDescription
    {
        [SerializeField] private Image _artifactIcon;
        private Sprite _artifactSprite;
        private string _name;

        public string Name => _name;

        public string Description => "";

        public Sprite IconSprite => _artifactSprite;

        public Transform InstTr => transform;

        public void Init(string name)
        {
            _name = name;
            _artifactSprite = ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_Artifact + name);
            _artifactIcon.sprite = _artifactSprite;
        }
    }
}
