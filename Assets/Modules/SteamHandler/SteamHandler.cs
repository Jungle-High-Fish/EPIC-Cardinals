using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

            var fileList = new List<string>();
            var fileCount = SteamRemoteStorage.Files;

            return fileList;
        }

        public List<DateTime> GetModifiedTime(List<string> fileNameList) {
            if (!_isSteamAvailable) {
                return new List<DateTime>();
            }

            var modifiedTimeList = new List<DateTime>();

            for (int i = 0; i < fileNameList.Count; i++) {
                var fileName = fileNameList[i];
                if (!SteamRemoteStorage.FileExists(fileName)) {
                    continue;
                }
                var fileTime = SteamRemoteStorage.FileTime(fileName);

                modifiedTimeList.Add(fileTime);
            }

            return modifiedTimeList;
        }
    }
}

