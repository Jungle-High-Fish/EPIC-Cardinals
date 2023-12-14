using System.Collections;
using System.Collections.Generic;
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
    }
}

