using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

using Cardinals.Enums;

namespace Cardinals.Game
{
    [CreateAssetMenu(fileName = "ArtifactData", menuName = "Cardinals/Artifact Data")]
    public class ArtifactDataSO : ScriptableObject
    {
        public ArtifactType artifactType;
        public string artifactName;
        [Multiline]
        public string description;
        [AssetSelector(Paths = "Assets/Resources/Sprites/UI/Icons/Artifacts")]
        [PreviewField(Height = 100, Alignment = ObjectFieldAlignment.Left)]
        public Sprite sprite;
        public int price;
    }
}