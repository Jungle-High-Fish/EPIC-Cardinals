using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Util {
    public static class TMPUtils {
        public enum CustomTag {
            Level
        }

        static readonly Dictionary<CustomTag, string> CustomTags = new Dictionary<CustomTag, string>() {
            { CustomTag.Level, "level=" }
        };
        
        public static void SetTextWithLevel(this TextMeshProUGUI textMeshProUGUI, string text, Color emphasisColor) {
            
        }

        private static void Parse(string text) {
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
                    return $"<color=@emphasisColor>@level({level})</color>";
                }

                return "";
            }
        }
    }
}
