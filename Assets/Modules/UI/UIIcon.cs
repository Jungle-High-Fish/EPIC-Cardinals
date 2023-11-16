using System.Collections;
using System.Collections.Generic;
using Cardinals.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Cardinals.UI
{
    /// <summary>
    /// 변경이 될 수 잇음
    /// </summary>
    public class UIIcon : MonoBehaviour, IDescription
    {
        private IDescription _baseInfo;
        
        [SerializeField] public Image _iconImg;

        public void Init(IDescription info)
        {
            _baseInfo = info;
        }
        
        public string Name => "[임시]UIIcon";
        public string Description => "[임시]Description";
        public Sprite IconSprite => null; //_baseInfo.IconSprite;
        public Transform InstTr => null;//_baseInfo.InstTr;
    }
}