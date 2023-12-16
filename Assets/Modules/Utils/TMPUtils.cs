using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Util {
    public static class TMPUtils {
        static readonly string[] CustomTags = new string[] {
            "level=",
        };

        public static void SetTextWithCustomTag(this TextMeshProUGUI textMeshProUGUI, string text, Color emphasisColor) {
            
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
                        
                    }
                 } else {
                    result += substrings[i];
                 }
            }

            bool IsCustomTag(string tag) {
                foreach (var customTag in CustomTags) {
                    if (tag.StartsWith(customTag)) {
                        return true;
                    }
                }

                return false; 
            }
        }
    }
}
