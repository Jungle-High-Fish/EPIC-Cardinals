using UnityEngine;
using UnityEngine.UI;

namespace Modules.Utils
{
    public static class Extensions
    {
        public static void Update(this HorizontalLayoutGroup layoutGroup)
        {
            if (layoutGroup != null)
            {
                layoutGroup.CalculateLayoutInputHorizontal();
                layoutGroup.CalculateLayoutInputVertical();
                layoutGroup.SetLayoutHorizontal();
                layoutGroup.SetLayoutVertical();
            }
        }
        
        public static void Update(this VerticalLayoutGroup layoutGroup)
        {
            if (layoutGroup != null)
            {
                layoutGroup.CalculateLayoutInputHorizontal();
                layoutGroup.CalculateLayoutInputVertical();
                layoutGroup.SetLayoutHorizontal();
                layoutGroup.SetLayoutVertical();
            }
        }
        
        public static void Update(this GridLayoutGroup layoutGroup)
        {
            if (layoutGroup != null)
            {
                layoutGroup.CalculateLayoutInputHorizontal();
                layoutGroup.CalculateLayoutInputVertical();
                layoutGroup.SetLayoutHorizontal();
                layoutGroup.SetLayoutVertical();
            }
        }
        
    }
}