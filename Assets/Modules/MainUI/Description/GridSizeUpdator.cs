using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Modules.Utils;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class GridSizeUpdator : MonoBehaviour
{
    public void Resizing()
    {
        var fitters = GetComponentsInChildren<ContentSizeFitter>();
        var horizontalLayout = GetComponentsInChildren<HorizontalLayoutGroup>();
        var verticalLayout = GetComponentsInChildren<VerticalLayoutGroup>();

        horizontalLayout.ForEach(g => g.enabled = true);
        verticalLayout.ForEach(g => g.enabled = true);
        fitters.ForEach(f => f.enabled = true);
        
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
