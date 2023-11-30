using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Modules.Utils;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Cardinals.UI.Description
{
    public class LayoutInitializer : MonoBehaviour
    {
        public void Clear()
        {
            var fitters = GetComponentsInChildren<ContentSizeFitter>();
            var horizontalLayout = GetComponentsInChildren<HorizontalLayoutGroup>();
            var verticalLayout = GetComponentsInChildren<VerticalLayoutGroup>();

            horizontalLayout.Reverse().ForEach(g => g.Update());
            verticalLayout.Reverse().ForEach(g => g.Update());
            foreach (var _fitter in fitters.Reverse())
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_fitter.transform);
            }

            horizontalLayout.ForEach(g => g.enabled = false);
            verticalLayout.ForEach(g => g.enabled = false);
            fitters.ForEach(f => f.enabled = false);
        }
    }
}