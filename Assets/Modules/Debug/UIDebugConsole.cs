using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.Test {
    public class UIDebugConsole : MonoBehaviour, IDebugComponent
    {
        private ComponentGetter<TMP_InputField> _inputField 
            = new(TypeOfGetter.Child);
        private ComponentGetter<TMP_Text> _logText 
            = new(TypeOfGetter.ChildByName, "Log Area/Viewport/Content/Text");
        
        private int _logStartIndex = 0;
        private int _logEndIndex = 0;

        public void Init() {
            
        }
    }
}