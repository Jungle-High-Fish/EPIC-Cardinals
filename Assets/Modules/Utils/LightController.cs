using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Cardinals.Util {
    public class LightController : MonoBehaviour {
        public void SetLightNight() {
            transform.DOLocalRotate(new Vector3(185, transform.localEulerAngles.y, transform.localEulerAngles.z), 0.5f, RotateMode.Fast);
        }

        public void SetLightDay() {
            transform.DOLocalRotate(new Vector3(130, transform.localEulerAngles.y, transform.localEulerAngles.z), 0.5f, RotateMode.FastBeyond360);

            GameManager.I.Stage.Enemies.Aggregate(Vector3.zero, (s, v) => s + v.transform.position);
        }
    }
}

