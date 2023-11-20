using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util {
    public static class UIHelper{
        public static void MatchParent(this RectTransform rectTransform, RectTransform parent=null) {
            if (parent != null) {
                rectTransform.SetParent(parent);
            }
            
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}

