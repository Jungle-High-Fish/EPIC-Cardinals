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

        private ComponentGetter<Text> _titleText = new ComponentGetter<Text>(
            TypeOfGetter.ChildByName, 
            "Details/Title Text"
        ); 

        private ComponentGetter<Text> _descriptionText = new ComponentGetter<Text>(
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
            _titleText.Get(gameObject).text = data.name;
            _descriptionText.Get(gameObject).text = data.description;
        }
    }
}

