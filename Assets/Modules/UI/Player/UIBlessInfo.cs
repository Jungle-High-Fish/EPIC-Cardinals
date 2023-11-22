using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    
    public class UIBlessInfo: MonoBehaviour {
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
    }

}

