using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace Cardinals {
    public class Localization {
        public string this[LocalizationEnum localizationEnum] => Get(localizationEnum);
        public List<string> LanguageList => _languageList;
        public List<string> LanguageNameList => _languageNameList;
        public bool IsJapanese => PlayerPrefs.GetString("Language").Split("-")[0] == "JP";
        public bool IsChinese => PlayerPrefs.GetString("Language").Split("-")[0] == "CHT";

        private Dictionary<LocalizationEnum, string> _localizationData;
        private List<string> _languageList;
        private List<string> _languageNameList;

        public Localization() {
            LoadData();
        }

        public void LoadData(string languageCode = null) {
            var data = CSVReader.Read("Localization");

            _languageList = new List<string>();
            _languageNameList = new List<string>();
            data[0].ForEach((v) => {
                if (v.Key != "ID") {
                    _languageList.Add(v.Key);
                    _languageNameList.Add(v.Key.Split("-")[1]);
                }
            });
            
            string targetLanguage;
            if (languageCode == null || _languageList.Contains(languageCode) == false) {
                if (PlayerPrefs.HasKey("Language") == false) {
                    PlayerPrefs.SetString("Language", _languageList[0]);
                    PlayerPrefs.Save();
                    targetLanguage = _languageList[0];
                } else {
                    targetLanguage = PlayerPrefs.GetString("Language");
                }
            } else {
                targetLanguage = languageCode;
            }

            _localizationData = new Dictionary<LocalizationEnum, string>();

            data.ForEach((row) => {
                var localizationEnum = (LocalizationEnum) Enum.Parse(typeof(LocalizationEnum), row["ID"].ToString());
                var localizationString = row[targetLanguage].ToString();
                _localizationData.Add(localizationEnum, localizationString);
            });
        }

        public string Get(LocalizationEnum localizationEnum) {
            return _localizationData[localizationEnum];
        }
    }
}