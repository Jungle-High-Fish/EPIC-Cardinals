using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cardinals;
using Cardinals.Enums;
using TMPro;
using UnityEngine;

namespace Util {
    public static class TMPUtils {
        public enum CustomTag {
            Level,
            Debuff,
            Element,
            CheckBless,
            CheckElement
        }

        static readonly Dictionary<CustomTag, string> CustomTags = new Dictionary<CustomTag, string>() {
            { CustomTag.Level, "level=" },
            { CustomTag.Debuff, "debuff=" },
            { CustomTag.Element, "element=" },
            { CustomTag.CheckBless, "checkBless=" },
            { CustomTag.CheckElement, "checkElement=" }
        };

        public static string GetTextWithLevel(string parsedText, int level, Color emphasisColor) {
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
        
        public static void SetTextWithLevel(this TextMeshProUGUI textMeshProUGUI, string parsedText, int level, Color emphasisColor) {
            textMeshProUGUI.text = GetTextWithLevel(parsedText, level, emphasisColor);
        }
        
        public static string GetTextWithBless(string parsedText, Dictionary<BlessType, (string text, Color color)> blessTexts) {
            string result = parsedText;

            string checkBlessRegex = @"@bless\s*\((\s*\w+\s*)\)";
            var checkBlessMatches = Regex.Matches(result, checkBlessRegex);

            if (checkBlessMatches.Count <= 0) {
                return result;
            }

            for (int i = 0; i < checkBlessMatches.Count; i++) {
                var checkBlessMatch = checkBlessMatches[i];
                var rawCheckBlessText = checkBlessMatch.Value;
                var checkBlessText = Regex.Replace(checkBlessMatch.Value, checkBlessRegex, @"$1");
                var checkBlessType = (BlessType)System.Enum.Parse(typeof(BlessType), checkBlessText);

                if (!blessTexts.ContainsKey(checkBlessType)) {
                    result = result.Replace(rawCheckBlessText, "");
                    continue;
                }

                var blessText = blessTexts[checkBlessType].text;
                var blessColor = blessTexts[checkBlessType].color;

                string replacement = 
                    $"<color=#{ColorUtility.ToHtmlStringRGB(blessColor)}>{blessText}</color>" +
                    $"<sprite=\"UI_TMP_SpriteSheet\" name=\"UI_Bless_{checkBlessType}\">";
                result = result.Replace(rawCheckBlessText, replacement);
            }

            return result;
        }

        public static void SetTextWithBless(
            this TextMeshProUGUI textMeshProUGUI, 
            string parsedText, 
            Dictionary<BlessType, (string text, Color color)> blessTexts
        ) {
            textMeshProUGUI.text = GetTextWithBless(parsedText, blessTexts);
        }

        public static string GetTextWithElement(string parsedText, Dictionary<TileMagicType, (string text, Color color)> elementTexts) {
            string result = parsedText;

            string checkElementRegex = @"@element\s*\((\s*\w+\s*)\)";
            var checkElementMatches = Regex.Matches(result, checkElementRegex);

            if (checkElementMatches.Count <= 0) {
                return result;
            }

            for (int i = 0; i < checkElementMatches.Count; i++) {
                var checkElementMatch = checkElementMatches[i];
                var rawCheckElementText = checkElementMatch.Value;
                var checkElementText = Regex.Replace(checkElementMatch.Value, checkElementRegex, @"$1");
                var checkElementType = (TileMagicType)System.Enum.Parse(typeof(TileMagicType), checkElementText);

                if (!elementTexts.ContainsKey(checkElementType)) {
                    result = result.Replace(rawCheckElementText, "");
                    continue;
                }

                var elementText = elementTexts[checkElementType].text;
                var elementColor = elementTexts[checkElementType].color;

                string replacement = 
                    $"<color=#{ColorUtility.ToHtmlStringRGB(elementColor)}>{elementText}</color>" +
                    $"<sprite=\"UI_TMP_SpriteSheet\" name=\"UI_Magic_{checkElementType}\">";
                result = result.Replace(rawCheckElementText, replacement);
            }

            return result;
        }

        public static void SetTextWithElement(
            this TextMeshProUGUI textMeshProUGUI, 
            string parsedText, 
            Dictionary<TileMagicType, (string text, Color color)> elementTexts
        ) {
            textMeshProUGUI.text = GetTextWithElement(parsedText, elementTexts);
        }

        public static void SetLocalizedText(this TextMeshProUGUI textMeshProUGUI, string id) {
            textMeshProUGUI.text = LocalizedText(id);
        }

        public static string LocalizedText(string id) {
            LocalizationEnum localizationEnum = (LocalizationEnum)System.Enum.Parse(typeof(LocalizationEnum), id);
            return GameManager.I.Localization[localizationEnum];
        }

        public static string CustomParse(string text, bool applyLocalization=false) {
            if (applyLocalization) {
                text = LocalizedText(text);
            }

            string result = "";

            string[] substrings = text.Split('<', '>');

            //int tagFlag = text[0] == '<' ? 0 : 1;
            int tagFlag = 1;
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
                    return $"<sprite=\"UI_TMP_SpriteSheet\" name=\"UI_Buff_{debuffType}\">";
                }

                if (tag.StartsWith(CustomTags[CustomTag.Element])) {
                    var splitString = tag.Split('=');
                    var tagStr = splitString[0];
                    var elementType = splitString[1];
                    return $"<sprite=\"UI_TMP_SpriteSheet\" name=\"UI_Magic_{elementType}\">";
                }

                if (tag.StartsWith(CustomTags[CustomTag.CheckBless])) {
                    var splitString = tag.Split('=');
                    var tagStr = splitString[0];
                    var blessType = splitString[1];
                    return $"@bless({blessType})";
                }

                if (tag.StartsWith(CustomTags[CustomTag.CheckElement])) {
                    var splitString = tag.Split('=');
                    var tagStr = splitString[0];
                    var elementType = splitString[1];
                    return $"@element({elementType})";
                }

                return "";
            }
        }
    }
}
