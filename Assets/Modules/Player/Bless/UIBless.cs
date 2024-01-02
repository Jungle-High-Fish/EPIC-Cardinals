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
        private ComponentGetter<Image> _image = new ComponentGetter<Image>(
            TypeOfGetter.ChildByName, 
            "Image"
        );

        private ComponentGetter<TextMeshProUGUI> _titleText = new ComponentGetter<TextMeshProUGUI>(
            TypeOfGetter.ChildByName, 
            "Details/Title Text"
        ); 

        private ComponentGetter<TextMeshProUGUI> _descriptionText = new ComponentGetter<TextMeshProUGUI>(
            TypeOfGetter.ChildByName, 
            "Details/Description Text"
        );

        public void Activate() {
            gameObject.SetActive(true);
        }
        
        public void Init(BlessType bless)
        {
            var data = ResourceLoader.LoadSO<BlessDataSO>(Constants.FilePath.Resources.SO_BlessData + bless);

            _image.Get(gameObject).sprite = data.patternSprite;
            _titleText.Get(gameObject).text = data.blessName;
            _descriptionText.Get(gameObject).text = data.description;
        }
    }
}

