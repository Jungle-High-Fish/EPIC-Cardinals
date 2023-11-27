using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using UnityEngine;

namespace Cardinals.UI {
    public class UIMouseDetector: MonoBehaviour {
        public MouseDetectorType MouseDetectorType => _mouseDetectorType;
        
        [SerializeField] private MouseDetectorType _mouseDetectorType;
    }
}

