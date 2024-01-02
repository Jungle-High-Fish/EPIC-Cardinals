using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals;
using Cardinals.UI;
using TMPro;
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

        public static void MatchWidthUpper(this RectTransform rectTransform, RectTransform parent=null) {
            if (parent != null) {
                rectTransform.SetParent(parent);
            }
            
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            rectTransform.pivot = new Vector2(0.5f, 1);
        }

        public static void MatchHeigthRightSide(this RectTransform rectTransform, RectTransform parent=null) {
            if (parent != null) {
                rectTransform.SetParent(parent);
            }
            
            rectTransform.anchorMin = new Vector2(1, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            rectTransform.pivot = new Vector2(1, 0f);
        }

        public static void SetUILeftBottom(this RectTransform rectTransform, RectTransform parent=null) {
            if (parent != null) {
                rectTransform.SetParent(parent);
            }

            var origianlPosition = rectTransform.position;

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.zero;

            rectTransform.position = origianlPosition;
        }

        public static void SetUICenter(this RectTransform rectTransform, RectTransform parent=null) {
            if (parent != null) {
                rectTransform.SetParent(parent);
            }

            var origianlPosition = rectTransform.position;
            
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

            rectTransform.position = origianlPosition;
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

        public static float GetStringHeight(this TextMeshProUGUI tmp, string text) {
            TMP_TextInfo textInfo = tmp.GetTextInfo(text);
            float textHeight = textInfo.lineInfo[0].lineHeight * textInfo.lineCount;
            float lineSpacing = tmp.lineSpacing * (textInfo.lineCount - 1) * 0.01f * tmp.fontSize;

            //Debug.Log($"textHeight: {textHeight}, lineSpacing: {lineSpacing}, margin: {tmp.margin.y}, {tmp.margin.w}, lineCount: {textInfo.lineCount}, fontSize: {tmp.fontSize}, lineHeight: {textInfo.lineInfo[0].lineHeight}");
            return textHeight + lineSpacing + tmp.margin.y + tmp.margin.w;
        }

        public static void DOStrikethrough(this TextMeshProUGUI tmp, float t) {
            string targetText = tmp.text;
            int targetLength = targetText.Length;

            targetText = targetText.Insert(0, "<s>");

            string newText = "";
            IEnumerator Coroutine() {
                for (int i = 2; i < targetLength; i++) {
                    if (targetText[i] == '\n') {
                        continue;
                    }
                    
                    newText = targetText.Insert(i, "</s>");
                    tmp.text = newText;
                    yield return new WaitForSeconds(t / targetLength);
                }
                tmp.text = targetText + "</s>";
            }

            tmp.StartCoroutine(Coroutine());
        }

        public static Canvas GetCanvas(this RectTransform rectTransform) {
            return rectTransform.GetComponentInParent<Canvas>();
        }

        public static Canvas GetCanvas(this GameObject gameObject) {
            if (gameObject.transform is not RectTransform) {
                throw new Exception($"GameObject [{gameObject.name}] is not a UI Object");
            }

            return gameObject.GetComponentInParent<Canvas>();
        }

        public static UIBackground Background(this GameObject gameObject) {
            return GameManager.I.UI.Background(gameObject.GetCanvas());
        }

        public static UIBackground Background(this RectTransform rectTransfrom) {
            return GameManager.I.UI.Background(rectTransfrom.GetCanvas());
        }
    }
}

