using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals
{
    public class UIBless : MonoBehaviour
    {
        private BlessType _baseBless;

        [Header("Component")]
        [SerializeField] private Image _iconImg;
        [SerializeField] private TextMeshProUGUI _nameTMP;
        [SerializeField] private TextMeshProUGUI _descipritonTMP;
        
        public void Init(BlessType bless)
        {
            _baseBless = bless;

            var data = ResourceLoader.LoadSO<BlessDataSO>(Constants.FilePath.Resources.SO_BlessData + bless);

            _iconImg.sprite = data.patternSprite;
            _nameTMP.text = data.name;
            _descipritonTMP.text = data.description;
        }
    }
}

