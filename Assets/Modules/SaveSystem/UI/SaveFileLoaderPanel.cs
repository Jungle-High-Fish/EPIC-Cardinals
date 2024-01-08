using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    public class SaveFileLoaderPanel : MonoBehaviour {
        public bool IsActive => _isActive;

        private ComponentGetter<TMP_Text> _titleText
            = new ComponentGetter<TMP_Text>(TypeOfGetter.ChildByName, "Main Panel/Title");
        private ComponentGetter<RectTransform> _scrollContentTransform
            = new ComponentGetter<RectTransform>(TypeOfGetter.ChildByName, "Main Panel/File List Panel/Scroll View/Viewport/Content");
        private ComponentGetter<Button> _closeButton
            = new ComponentGetter<Button>(TypeOfGetter.ChildByName, "Main Panel/Button Area/Close Button");

        private ComponentGetter<RectTransform> _mainPanelTransform
            = new ComponentGetter<RectTransform>(TypeOfGetter.ChildByName, "Main Panel");

        private List<SaveFileDataPanel> _saveFileDataPanelList = new List<SaveFileDataPanel>();
        private bool _isActive = false;

        public void Init() {
            Hide();

            _closeButton.Get(gameObject).onClick.AddListener(() => {
                GameManager.I.Sound.TitleButtonClick();
                Hide();
            });
        }

        public void Show(List<(string, DateTime)> localSaveFileList, List<(string, DateTime)> cloudSaveFileList, bool showAnimation=true) {
            //
            Clear();
            gameObject.SetActive(true);
            _isActive = true;

            _titleText.Get(gameObject).text = "불러오기";
            _closeButton.Get(gameObject).GetComponentInChildren<TMP_Text>().text = "닫기";

            if (showAnimation) {
                _mainPanelTransform.Get(gameObject).localScale = Vector3.zero;
                _mainPanelTransform.Get(gameObject).DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);
            }

            localSaveFileList.Sort((a, b) => b.Item2.CompareTo(a.Item2));
            foreach (var (fileName, modifiedTime) in localSaveFileList) {
                var saveFileDataPanel = Instantiate(
                    ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_SaveFileDataPanel), 
                    _scrollContentTransform.Get(gameObject)
                ).GetComponent<SaveFileDataPanel>();

                saveFileDataPanel.Init(
                    Path.GetFileNameWithoutExtension(fileName), 
                    $"{modifiedTime:yy-MM-dd HH:mm}", 
                    false, 
                    () => {
                        bool result = GameManager.I.SaveSystem.Load(Path.GetFileNameWithoutExtension(fileName), false);
                        GameManager.I.GameStart(GameManager.I.SaveSystem.CurrentSaveFileData);
                        Hide();
                        // TODO: 로드 실패 시 처리
                    },
                    () => {
                        bool result = GameManager.I.SaveSystem.Delete(Path.GetFileNameWithoutExtension(fileName), false);
                        if (result) {
                            //
                            Show(GameManager.I.SaveSystem.GetLocalSaveFileList(), GameManager.I.SaveSystem.GetCloudSaveFileList(), false);
                        }
                    }
                );

                _saveFileDataPanelList.Add(saveFileDataPanel);
            }
            
            if (cloudSaveFileList == null) {
                return;
            }

            cloudSaveFileList.Sort((a, b) => b.Item2.CompareTo(a.Item2));
            foreach (var (fileName, modifiedTime) in cloudSaveFileList) {
                var saveFileDataPanel = Instantiate(
                    ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_SaveFileDataPanel), 
                    _scrollContentTransform.Get(gameObject)
                ).GetComponent<SaveFileDataPanel>();

                saveFileDataPanel.Init(
                    Path.GetFileNameWithoutExtension(fileName), 
                    $"{modifiedTime:yy-MM-dd HH:mm}", 
                    true, 
                    () => {
                        bool result = GameManager.I.SaveSystem.Load(Path.GetFileNameWithoutExtension(fileName), true);
                        GameManager.I.GameStart(GameManager.I.SaveSystem.CurrentSaveFileData);
                        Hide();
                        // TODO: 로드 실패 시 처리
                    },
                    () => {
                        bool result = GameManager.I.SaveSystem.Delete(Path.GetFileNameWithoutExtension(fileName), true);
                        if (result) {
                            //
                            Show(GameManager.I.SaveSystem.GetLocalSaveFileList(), GameManager.I.SaveSystem.GetCloudSaveFileList(), false);
                        }
                    }
                );

                _saveFileDataPanelList.Add(saveFileDataPanel);
            }
        }

        public void Hide() {
            gameObject.SetActive(false);
            _isActive = false;
        }

        private void Clear() {
            foreach (var saveFileDataPanel in _saveFileDataPanelList) {
                Destroy(saveFileDataPanel.gameObject);
            }

            _saveFileDataPanelList.Clear();
        }
    }
}

