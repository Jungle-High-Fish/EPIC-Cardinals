using System.Collections;
using System.Collections.Generic;
using Cardinals.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Cardinals.UI
{
    public class UIIcon : MonoBehaviour
    {
        [SerializeField] public Image _iconImg;

        public void Init()
        {
            
        }

        public void Init(Sprite sprite)
        {
            _iconImg.sprite = sprite;
            GetComponent<DescriptionConnector>().Init();
        }
    }
}