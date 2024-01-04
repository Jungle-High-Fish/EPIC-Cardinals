using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    public class SaveFileLoaderPanel : MonoBehaviour {
        private ComponentGetter<RectTransform> _scrollContentTransform
            = new ComponentGetter<RectTransform>(TypeOfGetter.ChildByName, "File List Panel/Scroll View/Viewport/Content");
        private ComponentGetter<Button> _closeButton
            = new ComponentGetter<Button>(TypeOfGetter.ChildByName, "Button Area/Close Button");

        private List<SaveFileDataPanel> _saveFileDataPanelList = new List<SaveFileDataPanel>();

        public void Init() {
            _closeButton.Get(gameObject).onClick.AddListener(() => {
                GameManager.I.Sound.TitleButtonClick();
                gameObject.SetActive(false);
            });
        }

        public void Show(List<(string, DateTime)> localSaveFileList, List<(string, DateTime)> cloudSaveFileList) {
            GameManager.I.Sound.TitleButtonClick();
            Clear();
            gameObject.SetActive(true);
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
                        gameObject.SetActive(false);
                        // TODO: 로드 실패 시 처리
                    },
                    () => {
                        bool result = GameManager.I.SaveSystem.Delete(Path.GetFileNameWithoutExtension(fileName), false);
                        if (result) {
                            Show(GameManager.I.SaveSystem.GetLocalSaveFileList(), GameManager.I.SaveSystem.GetCloudSaveFileList());
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
                        gameObject.SetActive(false);
                        // TODO: 로드 실패 시 처리
                    },
                    () => {
                        bool result = GameManager.I.SaveSystem.Delete(Path.GetFileNameWithoutExtension(fileName), true);
                        if (result) {
                            Show(GameManager.I.SaveSystem.GetLocalSaveFileList(), GameManager.I.SaveSystem.GetCloudSaveFileList());
                        }
                    }
                );

                _saveFileDataPanelList.Add(saveFileDataPanel);
            }
        }

        private void Clear() {
            foreach (var saveFileDataPanel in _saveFileDataPanelList) {
                Destroy(saveFileDataPanel.gameObject);
            }

            _saveFileDataPanelList.Clear();
        }
    }
}

