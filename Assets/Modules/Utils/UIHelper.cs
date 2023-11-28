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
        
        public static void DestroyChildren(this Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                MonoBehaviour.Destroy(parent.GetChild(i).gameObject);
                
            }
        }

        public static void SetMaterialTransparent(this Material material) {
            material.SetFloat("_Surface", 1f);
            material.SetFloat("_Blend", 3.0f);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
        }
    }
}

