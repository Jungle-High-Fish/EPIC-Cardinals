using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.UI
{
    public class UIArtifact : MonoBehaviour, IDescription
    {
        [Header("Info")]
        [SerializeField] private Transform _descriptionTr;
        public void Init()
        {
            
        }

        public string Name => "유물 이름";
        public string Description => "유물 설명";
        public Sprite IconSprite => null;
        public Transform InstTr => _descriptionTr;
    } 
}
