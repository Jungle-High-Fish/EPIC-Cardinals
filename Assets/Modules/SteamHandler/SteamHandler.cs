using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Steamworks;
using UnityEngine;
using Cardinals.Enums;
using Cardinals.Game;
using Steamworks.Data;

namespace Cardinals {
    // public class SteamHandler {
    //     public bool IsSteamAvailable => _isSteamAvailable;

    //     private bool _isSteamAvailable;
        
    //     public bool Init() {
    //         if (SteamClient.IsValid) {
    //             _isSteamAvailable = true;
    //             return true;
    //         }

    //         try {
    //             SteamClient.Init(2735620);
    //         } catch (Exception e) {
    //             Debug.Log(e);
    //             _isSteamAvailable = false;
    //             return false;
    //         }

    //         _isSteamAvailable = true;
    //         return true;
    //     }

    //     public void EveryFrame() {
    //         if (_isSteamAvailable) {
    //             SteamClient.RunCallbacks();
    //         }

    //         // if (Input.GetKeyDown(KeyCode.F1)) {
    //         //     foreach ( var friend in SteamFriends.GetFriends() )
    //         //     {
    //         //         if (friend.Name == "Mintti") {
    //         //             friend.SendMessage("와 샌즈!");
    //         //         }

    //         //         if (friend.Name == "a55208806") {
    //         //             friend.SendMessage("와 샌즈!");
    //         //         }

    //         //         if (friend.Name == "ytw0113") {
    //         //             friend.SendMessage("발더스를해야만해..");
    //         //             friend.SendMessage("와 샌즈!");
    //         //         }
    //         //     }
    //         // }
    //     }

    //     public void SetLobbyStateDisplay() {
    //         if (!_isSteamAvailable) {
    //             return;
    //         }

    //         SteamFriends.SetRichPresence(
    //             "steam_display", 
    //             "#StatusLobby"
    //         );
    //     }

    //     public void SetBattleStateDisplay(int eventIdx, List<EnemyType> enemyType) {
    //         if (!_isSteamAvailable) {
    //             return;
    //         }

    //         string GetOrdinalNumber(int n) {
    //             switch (n % 10) {
    //                 case 1:
    //                     return "1";
    //                 case 2:
    //                     return "2";
    //                 case 3:
    //                     return "3";
    //                 default:
    //                     return "4";
    //             }
    //         }

    //         string eventIdxOrdinalStr = GetOrdinalNumber(eventIdx + 1);
    //         SteamFriends.SetRichPresence(
    //             "eventidxordinal", 
    //             eventIdxOrdinalStr
    //         );

    //         string eventIdxStr = $"{eventIdx + 1}";

    //         string enemiesStr = "";
    //         for (int i = 0; i < enemyType.Count; i++) {
    //             enemiesStr += GameManager.I.Localization.Get(
    //                 (LocalizationEnum)Enum.Parse(typeof(LocalizationEnum), EnemyDataSO.Data(enemyType[i]).enemyName)
    //             );
    //             if (i != enemyType.Count - 1) {
    //                 enemiesStr += "/";
    //             }
    //         }

    //         SteamFriends.SetRichPresence(
    //             "eventidx", 
    //             eventIdxStr
    //         );

    //         SteamFriends.SetRichPresence(
    //             "enemies", 
    //             enemiesStr
    //         );

    //         SteamFriends.SetRichPresence(
    //             "steam_display", 
    //             "#StatusBattle"
    //         );
    //     }

    //     public bool TriggerAchievement(string key) {
    //         Debug.Log($"Achievement: <{key}> Triggered");

    //         if (!_isSteamAvailable) {
    //             return false;
    //         }

    //         string achieveName = key;
    //         var ach = new Achievement(achieveName);
    //         ach.Trigger();

    //         return true;
    //     }

    //     public bool IsCloudSaveAvailable() {
    //         if (!_isSteamAvailable) {
    //             return false;
    //         }

    //         return SteamRemoteStorage.IsCloudEnabled;
    //     }

    //     public List<string> GetSteamCloudFiles() {
    //         if (!_isSteamAvailable) {
    //             return new List<string>();
    //         }

    //         var fileList = SteamRemoteStorage.Files;
    //         fileList.ToList().RemoveAll((f) => !f.StartsWith(SteamClient.SteamId + "/"));
    //         fileList = fileList.ToList().ConvertAll((f) => f.Replace(SteamClient.SteamId + "/", ""));

    //         return fileList.ToList();
    //     }

    //     public bool IsSteamCloudFileExists(string fileName) {
    //         if (!IsCloudSaveAvailable()) {
    //             return false;
    //         }

    //         return SteamRemoteStorage.FileExists(GetUserSteamCloudSavePath(fileName));
    //     }

    //     public bool SaveSteamCloudFile(string fileName, string data) {
    //         if (!IsCloudSaveAvailable()) {
    //             return false;
    //         }

    //         return SteamRemoteStorage.FileWrite(GetUserSteamCloudSavePath(fileName), System.Text.Encoding.UTF8.GetBytes(data));
    //     }

    //     public string LoadSteamCloudFile(string fileName) {
    //         if (!IsCloudSaveAvailable()) {
    //             return null;
    //         }

    //         if (!SteamRemoteStorage.FileExists(GetUserSteamCloudSavePath(fileName))) {
    //             return null;
    //         }

    //         var fileData = SteamRemoteStorage.FileRead(GetUserSteamCloudSavePath(fileName));
    //         return System.Text.Encoding.UTF8.GetString(fileData);
    //     }
        
    //     public bool DeleteSteamCloudFile(string fileName) {
    //         if (!IsCloudSaveAvailable()) {
    //             return false;
    //         }

    //         if (!SteamRemoteStorage.FileExists(GetUserSteamCloudSavePath(fileName))) {
    //             return false;
    //         }

    //         return SteamRemoteStorage.FileDelete(GetUserSteamCloudSavePath(fileName));
    //     }

    //     public List<DateTime> GetModifiedTime(List<string> fileNameList) {
    //         if (!_isSteamAvailable) {
    //             return new List<DateTime>();
    //         }

    //         var modifiedTimeList = new List<DateTime>();

    //         for (int i = 0; i < fileNameList.Count; i++) {
    //             var fileName = fileNameList[i];
    //             if (!SteamRemoteStorage.FileExists(GetUserSteamCloudSavePath(fileName))) {
    //                 continue;
    //             }
    //             var fileTime = SteamRemoteStorage.FileTime(GetUserSteamCloudSavePath(fileName));
    //             fileTime = fileTime.ToLocalTime();

    //             modifiedTimeList.Add(fileTime);
    //         }

    //         return modifiedTimeList;
    //     }

    //     private string GetUserSteamCloudSavePath(string fileName) {
    //         if (!_isSteamAvailable) {
    //             return null;
    //         }

    //         return SteamClient.SteamId + "/" + fileName;
    //     }
    // }
}

