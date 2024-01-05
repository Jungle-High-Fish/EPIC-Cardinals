using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cardinals.Game;
using Sirenix.Utilities;
using UnityEngine;

namespace Cardinals {
    public class SaveSystem {
        private readonly string LOCAL_SAVE_PATH = Path.Join(Application.persistentDataPath, "SaveData/");

        public SaveFileData CurrentSaveFileData => _currentSaveFileData;

        private SaveFileData _currentSaveFileData;
        private bool _isCloudAvailable 
            => GameManager.I.SteamHandler.IsSteamAvailable && GameManager.I.SteamHandler.IsCloudSaveAvailable();
        
        public void ClearCurrentSaveFileData() {
            _currentSaveFileData = null;
        }

        public List<(string, DateTime)> GetProperSaveFileData() {
            if (_isCloudAvailable) {
                return GetCloudSaveFileList();
            } else {
                return GetLocalSaveFileList();
            }
        }

        public List<(string, DateTime)> GetLocalSaveFileList() {
            if (!Directory.Exists(LOCAL_SAVE_PATH)) {
                Directory.CreateDirectory(LOCAL_SAVE_PATH);
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(LOCAL_SAVE_PATH);
            var saveFileList = directoryInfo.GetFiles("*.sav");

            return saveFileList.ToList().ConvertAll((f) => (f.Name, f.LastWriteTime));
        }

        public List<(string, DateTime)> GetCloudSaveFileList() {
            if (!GameManager.I.SteamHandler.IsSteamAvailable) {
                return null;
            }

            var fileList = GameManager.I.SteamHandler.GetSteamCloudFiles();
            var modifiedTimeList = GameManager.I.SteamHandler.GetModifiedTime(fileList);

            var saveFileList = new List<(string, DateTime)>();
            for (int i = 0; i < fileList.Count; i++) {
                saveFileList.Add((fileList[i], modifiedTimeList[i]));
            }

            return saveFileList;
        }

        public SaveFileData GenerateAutoSaveFile(Stage[] stageList, Player player, DiceManager diceManager) {
            var saveFileData = new SaveFileData();

            string fileName;

            if (_isCloudAvailable) {
                fileName = IndexedSaveFileName(true);
            } else {
                fileName = IndexedSaveFileName(false);
            }

            saveFileData.SetData(
                _isCloudAvailable,
                fileName, 
                player, 
                diceManager, 
                stageList, 
                GameManager.I.CurrentStageIndex
            );

            return saveFileData;
        }

        public bool Save(SaveFileData saveFileData) {
            _currentSaveFileData = saveFileData;
            if (_isCloudAvailable && saveFileData.isCloudSave) {
                return GameManager.I.SteamHandler.SaveSteamCloudFile(
                    saveFileData.fileName + ".sav", 
                    JsonUtility.ToJson(saveFileData)
                );
            } else {
                saveFileData.isCloudSave = false;
                
                if (!Directory.Exists(LOCAL_SAVE_PATH)) {
                    Directory.CreateDirectory(LOCAL_SAVE_PATH);
                }

                File.WriteAllText(
                    Path.Join(LOCAL_SAVE_PATH, saveFileData.fileName + ".sav"), 
                    JsonUtility.ToJson(saveFileData)
                );

                return true;
            }
        }

        public bool Load(string fileName, bool isCloudSave) {
            if (isCloudSave) {
                if (!_isCloudAvailable) {
                    return false;
                }

                var fileData = GameManager.I.SteamHandler.LoadSteamCloudFile(fileName + ".sav");
                if (fileData == null) {
                    return false;
                }

                _currentSaveFileData = JsonUtility.FromJson<SaveFileData>(fileData);
                return true;
            } else {
                if (!Directory.Exists(LOCAL_SAVE_PATH)) {
                    Directory.CreateDirectory(LOCAL_SAVE_PATH);
                }

                var fileData = File.ReadAllText(Path.Join(LOCAL_SAVE_PATH, fileName + ".sav"));
                if (fileData == null) {
                    return false;
                }

                _currentSaveFileData = JsonUtility.FromJson<SaveFileData>(fileData);
                return true;
            }
        }

        public bool Delete(string fileName, bool isCloudSave) {
            GameManager.I.Sound.TitleButtonClick();
            if (isCloudSave) {
                if (!_isCloudAvailable) {
                    return false;
                }

                return GameManager.I.SteamHandler.DeleteSteamCloudFile(fileName + ".sav");
            } else {
                if (!Directory.Exists(LOCAL_SAVE_PATH)) {
                    Directory.CreateDirectory(LOCAL_SAVE_PATH);
                }

                if (!File.Exists(Path.Join(LOCAL_SAVE_PATH, fileName + ".sav"))) {
                    return false;
                }

                File.Delete(Path.Join(LOCAL_SAVE_PATH, fileName + ".sav"));
                return true;
            }
        }

        public bool SaveCurrentSaveFileData() {
            if (_currentSaveFileData == null) {
                return false;
            }

            return Save(_currentSaveFileData);
        }

        private string IndexedSaveFileName(bool isCloudSave) {
            int i = 0;
            string filename = null;
            string path = null;

            if (isCloudSave) {
                do {
                    i++;
                    filename = $"AutoSave_{i}";
                } while (GameManager.I.SteamHandler.IsSteamCloudFileExists(filename + ".sav"));

                return filename;
            } else {
                do {
                    i++;
                    filename = $"AutoSave_{i}";
                    path = Path.Join(LOCAL_SAVE_PATH, filename + ".sav");
                } while (File.Exists(path));

                return filename;
            }
        }
    }
}