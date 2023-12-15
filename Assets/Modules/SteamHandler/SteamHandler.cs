using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Steamworks;
using UnityEngine;

namespace Cardinals {
    public class SteamHandler {
        public bool IsSteamAvailable => _isSteamAvailable;

        private bool _isSteamAvailable;
        
        public bool Init() {
            try {
                Steamworks.SteamClient.Init(2735620);
            } catch {
                _isSteamAvailable = false;
                return false;
            }

            _isSteamAvailable = true;
            return true;
        }

        public void EveryFrame() {
            if (_isSteamAvailable) {
                SteamClient.RunCallbacks();
            }

            if (Input.GetKeyDown(KeyCode.F1)) {
                foreach ( var friend in SteamFriends.GetFriends() )
                {
                    if (friend.Name == "Mintti") {
                        friend.SendMessage("와 샌즈!");
                    }
                }
            }
        }

        public bool IsCloudSaveAvailable() {
            if (!_isSteamAvailable) {
                return false;
            }

            return SteamRemoteStorage.IsCloudEnabled;
        }

        public List<string> GetSteamCloudFiles() {
            if (!_isSteamAvailable) {
                return new List<string>();
            }

            var fileList = SteamRemoteStorage.Files;
            fileList.ToList().RemoveAll((f) => !f.StartsWith(SteamClient.SteamId + "/"));
            fileList = fileList.ToList().ConvertAll((f) => f.Replace(SteamClient.SteamId + "/", ""));

            return fileList.ToList();
        }

        public bool IsSteamCloudFileExists(string fileName) {
            if (!IsCloudSaveAvailable()) {
                return false;
            }

            return SteamRemoteStorage.FileExists(GetUserSteamCloudSavePath(fileName));
        }

        public bool SaveSteamCloudFile(string fileName, string data) {
            if (!IsCloudSaveAvailable()) {
                return false;
            }

            return SteamRemoteStorage.FileWrite(GetUserSteamCloudSavePath(fileName), System.Text.Encoding.UTF8.GetBytes(data));
        }

        public string LoadSteamCloudFile(string fileName) {
            if (!IsCloudSaveAvailable()) {
                return null;
            }

            if (!SteamRemoteStorage.FileExists(GetUserSteamCloudSavePath(fileName))) {
                return null;
            }

            var fileData = SteamRemoteStorage.FileRead(GetUserSteamCloudSavePath(fileName));
            return System.Text.Encoding.UTF8.GetString(fileData);
        }
        
        public bool DeleteSteamCloudFile(string fileName) {
            if (!IsCloudSaveAvailable()) {
                return false;
            }

            if (!SteamRemoteStorage.FileExists(GetUserSteamCloudSavePath(fileName))) {
                return false;
            }

            return SteamRemoteStorage.FileDelete(GetUserSteamCloudSavePath(fileName));
        }

        public List<DateTime> GetModifiedTime(List<string> fileNameList) {
            if (!_isSteamAvailable) {
                return new List<DateTime>();
            }

            var modifiedTimeList = new List<DateTime>();

            for (int i = 0; i < fileNameList.Count; i++) {
                var fileName = fileNameList[i];
                if (!SteamRemoteStorage.FileExists(GetUserSteamCloudSavePath(fileName))) {
                    continue;
                }
                var fileTime = SteamRemoteStorage.FileTime(GetUserSteamCloudSavePath(fileName));

                modifiedTimeList.Add(fileTime);
            }

            return modifiedTimeList;
        }

        private string GetUserSteamCloudSavePath(string fileName) {
            if (!_isSteamAvailable) {
                return null;
            }

            return SteamClient.SteamId + "/" + fileName;
        }
    }
}

