using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    public class GameSetting {
        public event Action OnSoundSettingChanged;

        public string Language => _language;
        public Resolution Resolution => _resolution;
        public bool IsFullScreen => _isFullScreen;
        public float BgmVolume => _bgmVolume;
        public float SfxVolume => _sfxVolume;

        private string _language;
        private Resolution _resolution;
        private bool _isFullScreen;
        private int _bgmVolume;
        private int _sfxVolume;

        public void Init() {
            GetLanguage();
            GetResolution();
            GetFullScreen();
            GetBgmVolume();
            GetSfxVolume();
        }

        public void InitUI(GameSettingUI gameSettingUI) {
            int languageIndex = GameManager.I.Localization.LanguageList.FindIndex((language) => {
                return language == _language;
            });

            GameSettingUI.DropdownDataForUI languages = new GameSettingUI.DropdownDataForUI(
                GameManager.I.Localization[LocalizationEnum.UI_GAMESETTING_LANGUAGE],
                GameManager.I.Localization.LanguageNameList,
                GameManager.I.Localization.LanguageList,
                0,
                (value) => {
                    _language = GameManager.I.Localization.LanguageList[value];
                    PlayerPrefs.SetString("Language", GameManager.I.Localization.LanguageList[value]);
                    PlayerPrefs.Save();

                    GameManager.I.Localization.LoadData(_language);
                }
            );

            GameSettingUI.CheckboxDataForUI isFullScreenData = new GameSettingUI.CheckboxDataForUI(
                GameManager.I.Localization[LocalizationEnum.UI_GAMESETTING_FULLSCREEN],
                _isFullScreen,
                (value) => {
                    _isFullScreen = value;
                    Screen.fullScreen = value;
                    PlayerPrefs.SetInt("IsFullScreen", value ? 1 : 0);
                    PlayerPrefs.Save();
                }
            );
            
            List<Resolution> resolutions = new List<Resolution>(Screen.resolutions);
            List<string> resolutionStrings = resolutions.ConvertAll<string>((resolution) => {
                return resolution.width + "x" + resolution.height;
            });
            // find current resolution index
            int currentResolutionIndex = resolutions.FindIndex((resolution) => {
                return resolution.width == _resolution.width && resolution.height == _resolution.height;
            });

            GameSettingUI.DropdownDataForUI resolutionData = new GameSettingUI.DropdownDataForUI(
                GameManager.I.Localization[LocalizationEnum.UI_GAMESETTING_RESOLUTION],
                resolutionStrings,
                resolutions,
                currentResolutionIndex,
                (value) => {
                    var targetResolution = resolutions[value];
                    _resolution.width = targetResolution.width;
                    _resolution.height = targetResolution.height;

                    Screen.SetResolution(_resolution.width, _resolution.height, _isFullScreen);
                    PlayerPrefs.SetInt("ResolutionWidth", _resolution.width);
                    PlayerPrefs.SetInt("ResolutionHeight", _resolution.height);
                    PlayerPrefs.Save();
                }
            );

            GameSettingUI.SliderDataForUI bgmVolumeData = new GameSettingUI.SliderDataForUI(
                GameManager.I.Localization[LocalizationEnum.UI_GAMESETTING_BGM],
                0,
                100,
                true,
                _bgmVolume,
                (value) => {
                    _bgmVolume = (int) value;
                    PlayerPrefs.SetInt("BgmVolume", _bgmVolume);
                    PlayerPrefs.Save();
                    OnSoundSettingChanged?.Invoke();
                }
            );

            GameSettingUI.SliderDataForUI sfxVolumeData = new GameSettingUI.SliderDataForUI(
                GameManager.I.Localization[LocalizationEnum.UI_GAMESETTING_SFX],
                0,
                100,
                true,
                _sfxVolume,
                (value) => {
                    _sfxVolume = (int) value;
                    PlayerPrefs.SetInt("SfxVolume", _sfxVolume);
                    PlayerPrefs.Save();
                    OnSoundSettingChanged?.Invoke();
                }
            );

            gameSettingUI.Init(new List<GameSettingUI.SettingDataForUI>() {
                languages,
                isFullScreenData,
                resolutionData,
                bgmVolumeData,
                sfxVolumeData
            });
        } 

        private void GetLanguage() {
            if (PlayerPrefs.HasKey("Language")) {
                _language = PlayerPrefs.GetString("Language");
            } else {
                _language = GameManager.I.Localization.LanguageList[0];
            }
        }

        private void GetResolution() {
            if (PlayerPrefs.HasKey("ResolutionWidth") && PlayerPrefs.HasKey("ResolutionHeight")) {
                _resolution.width = PlayerPrefs.GetInt("ResolutionWidth");
                _resolution.height = PlayerPrefs.GetInt("ResolutionHeight");
                Screen.SetResolution(_resolution.width, _resolution.height, _isFullScreen);
            } else {
                _resolution = Screen.currentResolution;
            }
        }

        private void GetFullScreen() {
            if (PlayerPrefs.HasKey("IsFullScreen")) {
                _isFullScreen = PlayerPrefs.GetInt("IsFullScreen") == 1;
                Screen.fullScreen = _isFullScreen;
            } else {
                _isFullScreen = Screen.fullScreen;
            }
        }

        private void GetBgmVolume() {
            if (PlayerPrefs.HasKey("BgmVolume")) {
                _bgmVolume = PlayerPrefs.GetInt("BgmVolume");
                OnSoundSettingChanged?.Invoke();
            } else {
                _bgmVolume = 50;
            }
        }

        private void GetSfxVolume() {
            if (PlayerPrefs.HasKey("SfxVolume")) {
                _sfxVolume = PlayerPrefs.GetInt("SfxVolume");
                OnSoundSettingChanged?.Invoke();
            } else {
                _sfxVolume = 50;
            }
        }
    }
}