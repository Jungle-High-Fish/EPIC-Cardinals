using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace Util {
    public static class TMPUtils {
        public enum CustomTag {
            Level,
            Debuff
        }

        static readonly Dictionary<CustomTag, string> CustomTags = new Dictionary<CustomTag, string>() {
            { CustomTag.Level, "level=" },
            { CustomTag.Debuff, "debuff=" }
        };

        public static string GetTextWithLevel(string text, int level, Color emphasisColor) {
            string parsedText = Parse(text);
            string result = parsedText;

        #pragma warning disable 0164
        LevelMatching:
        #pragma warning restore 0164

            string levelValueRegex = @"@levelValue\s*\(((\s*\d+\s*,)*\s*\d\s*)\)";
            var levelValueMatches = Regex.Matches(result, levelValueRegex);

            if (levelValueMatches.Count <= 0) {
                goto ColorMatching;
            }

            for (int i = 0; i < levelValueMatches.Count; i++) {
                var levelMatch = levelValueMatches[i];
                var rawLevelText = levelMatch.Value;
                var levelText = Regex.Replace(levelMatch.Value, levelValueRegex, @"$1");
                var levelList = new List<int>();
                foreach (var lt in levelText.Split(',')) {
                    levelList.Add(int.Parse(lt.Replace(" ", "")));
                }

                if (level > 0) {
                    result = result.Replace(rawLevelText, $"{levelList[level - 1]}");
                } else {
                    string levelValues = "";
                    for (int j = 0; j < levelList.Count; j++) {
                        levelValues += $"{levelList[j]}<sub><b><size=100%>Lv.{j + 1}</size></b></sub>";
                        if (j != levelList.Count - 1) {
                            levelValues += "/";
                        }
                    }
                    result = result.Replace(rawLevelText, levelValues);
                }
            }

            string levelRegex = @"@level";
            result = Regex.Replace(result, levelRegex, $"Lv. {level}");
        
        ColorMatching:
            string colorRegex = @"@emphasisColor";
            result = Regex.Replace(result, colorRegex, $"#{ColorUtility.ToHtmlStringRGB(emphasisColor)}");

            return result;
        }
        
        public static void SetTextWithLevel(this TextMeshProUGUI textMeshProUGUI, string text, int level, Color emphasisColor) {
            textMeshProUGUI.text = GetTextWithLevel(text, level, emphasisColor);
        }

        private static string Parse(string text) {
            string result = "";

            string[] substrings = text.Split('<', '>');
            
            int tagFlag = text[0] == '<' ? 0 : 1;
            for (int i = 0; i < substrings.Length; i++) {
                 if (i % 2 == tagFlag) {
                    var targetTag = substrings[i].Replace(" ", "");
                    if (!IsCustomTag(targetTag)) {
                        result += substrings[i];
                    } else {
                        result += TagConvert(targetTag);
                    }
                 } else {
                    result += substrings[i];
                 }
            }

            return result;

            bool IsCustomTag(string tag) {
                foreach (var customTag in CustomTags.Values) {
                    if (tag.StartsWith(customTag)) {
                        return true;
                    }
                }

                return false; 
            }

            string TagConvert(string tag) {
                if (tag.StartsWith(CustomTags[CustomTag.Level])) {
                    var splitString = tag.Split('=');
                    var tagStr = splitString[0];
                    var level = splitString[1];
                    return $"<color=@emphasisColor>@levelValue({level})</color>";
                }

                if (tag.StartsWith(CustomTags[CustomTag.Debuff])) {
                    var splitString = tag.Split('=');
                    var tagStr = splitString[0];
                    var debuffType = splitString[1];
                    return $"<sprite=\"UI_Buffs_Sprite_Sheet\" name=\"UI_Buff_{debuffType}\">";
                }

                return "";
            }
        }
    }
}
