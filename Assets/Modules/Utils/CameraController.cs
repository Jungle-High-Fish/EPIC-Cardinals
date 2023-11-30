using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Util;

namespace Cardinals.Util {
    public class CameraController : MonoBehaviour {
        ComponentGetter<CinemachineVirtualCamera> _virtualCamera
            = new ComponentGetter<CinemachineVirtualCamera>(TypeOfGetter.This);
        
        private CinemachineBasicMultiChannelPerlin _perlin;

        private void Awake() {
            _perlin = _virtualCamera.Get(gameObject).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            _perlin.m_AmplitudeGain = 0;
            _perlin.m_FrequencyGain = 0;
        }

        public void ShakeCamera(float duration, float amplitudeGain, float frequencyGain) {
            _perlin.m_AmplitudeGain = amplitudeGain;
            _perlin.m_FrequencyGain = frequencyGain;
            StartCoroutine(ShakeCameraCoroutine(duration));
        }

        private IEnumerator ShakeCameraCoroutine(float duration) {
            yield return new WaitForSeconds(duration);
            _perlin.m_AmplitudeGain = 0;
            _perlin.m_FrequencyGain = 0;
        }
    }
}
