using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Cardinals.UI
{
    public class UILoading : MonoBehaviour {
        private ComponentGetter<Animator> _animation
            = new ComponentGetter<Animator>(TypeOfGetter.This);

        [Button]
        public void Play() {
            _animation.Get(gameObject).Play("Roll");
        }
    }
}
