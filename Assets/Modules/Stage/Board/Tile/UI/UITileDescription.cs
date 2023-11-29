using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    public class UITileDescription: MonoBehaviour, ILayoutSelfController {
        public ComponentGetter<Image> _outline
            = new ComponentGetter<Image>(TypeOfGetter.ChildByName, "Outline");

        public ComponentGetter<RectTransform> _contentsTransform
            = new ComponentGetter<RectTransform>(TypeOfGetter.ChildByName, "Contents");

        public ComponentGetter<RectTransform> _titleHeaderTransform
            = new ComponentGetter<RectTransform>(TypeOfGetter.ChildByName, "Contents/TitleHeader");

        public ComponentGetter<Image> _icon
            = new ComponentGetter<Image>(TypeOfGetter.ChildByName, "Contents/TitleHeader/Icon");

        public ComponentGetter<TextMeshProUGUI> _titleText
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Contents/TitleHeader/Title");

        public ComponentGetter<TextMeshProUGUI> _descriptionText
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Contents/Description");

        [Button]
        public void ShowDescription(string title, string description, Sprite icon=null, Color? color=null) {
            _contentsTransform.Get(gameObject).gameObject.SetActive(false);

            if (color != null) {
                _outline.Get(gameObject).gameObject.SetActive(true);
                _outline.Get(gameObject).color = (Color)color;
            } else {
                _outline.Get(gameObject).gameObject.SetActive(false);
            }

            _titleText.Get(gameObject).text = title;

            TextMeshProUGUI tmp = _descriptionText.Get(gameObject);
            tmp.text = description;
            float descriptionHeight = tmp.GetStringHeight(description);

            if (icon != null) {
                _icon.Get(gameObject).gameObject.SetActive(true);
                _icon.Get(gameObject).sprite = icon;
            } else {
                _icon.Get(gameObject).gameObject.SetActive(false);
            }

            float contentsHeight = 
                _titleHeaderTransform.Get(gameObject).sizeDelta.y +
                descriptionHeight;

            (transform as RectTransform).sizeDelta = new Vector2(
                (transform as RectTransform).sizeDelta.x,
                contentsHeight
            );

            _contentsTransform.Get(gameObject).gameObject.SetActive(true);
        }

        public void SetLayoutHorizontal()
        {
            
        }

        public void SetLayoutVertical()
        {
            
        }

        public void Update() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                Debug.Log(_descriptionText.Get(gameObject).textInfo.lineInfo[0].lineHeight);
            }
 
            if (Input.GetKeyDown(KeyCode.Escape)) {
                ShowDescription("가나다", "aasdfasdfass");
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                ShowDescription("가나다", "asdfasdfasdfasdfasdfdasfaasdfasdfasdfasdfasdfasdfsadfasdfasdfasdfasdfasdfasdf");
            }
        }
    }
}

