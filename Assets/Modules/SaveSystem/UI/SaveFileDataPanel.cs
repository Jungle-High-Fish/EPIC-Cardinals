using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    public class SaveFileDataPanel : MonoBehaviour {
        private ComponentGetter<TextMeshProUGUI> _fileNameText
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Text Area/File Name Text");
        private ComponentGetter<TextMeshProUGUI> _modifiedTimeText
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Text Area/File DateTime Text");
        private ComponentGetter<Image> _cloudSaveImage
            = new ComponentGetter<Image>(TypeOfGetter.ChildByName, "Cloud Image");

        private ComponentGetter<Button> _loadButton
            = new ComponentGetter<Button>(TypeOfGetter.ChildByName, "Button Area/Load Button");
        private ComponentGetter<Button> _deleteButton
            = new ComponentGetter<Button>(TypeOfGetter.ChildByName, "Button Area/Trash Button");

        public void Init(string fileName, string modifiedTime, bool isCloudSave, Action loadAction, Action deleteAction) {
            _fileNameText.Get(gameObject).text = fileName;
            _modifiedTimeText.Get(gameObject).text = modifiedTime;

            _cloudSaveImage.Get(gameObject).gameObject.SetActive(isCloudSave);

            _loadButton.Get(gameObject).GetComponentInChildren<TMP_Text>().text
                = GameManager.I.Localization.Get(LocalizationEnum.UI_LOAD_LOAD);

            _loadButton.Get(gameObject).onClick.AddListener(() => {
                GameManager.I.Sound.TitleButtonClick();
                loadAction();
            });
            _deleteButton.Get(gameObject).onClick.AddListener(() => {
                deleteAction();
            });
        }
    }
}

