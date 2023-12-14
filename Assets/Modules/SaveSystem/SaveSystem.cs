using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace Cardinals {
    public class SaveSystem {
        private readonly string LOCAL_SAVE_PATH = Path.Join(Application.persistentDataPath, "SaveData/");

        private SaveFileData _currentSaveFileData;
        private bool _isCloudAvailable;

        private List<(string, DateTime)> GetSaveFileData() {
            // check if steam cloud save exists
            _isCloudAvailable = 
                GameManager.I.SteamHandler.IsSteamAvailable && 
                GameManager.I.SteamHandler.IsCloudSaveAvailable();

            if (_isCloudAvailable) {
                return GetCloudSaveFileList();
            } else {
                return GetLocalSaveFileList();
            }
        }

        private List<(string, DateTime)> GetLocalSaveFileList() {
            if (!Directory.Exists(LOCAL_SAVE_PATH)) {
                Directory.CreateDirectory(LOCAL_SAVE_PATH);
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(LOCAL_SAVE_PATH);
            var saveFileList = directoryInfo.GetFiles("*.sav");

            return saveFileList.ToList().ConvertAll((f) => (f.Name, f.LastWriteTime));
        }

        private List<(string, DateTime)> GetCloudSaveFileList() {
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
    }
}