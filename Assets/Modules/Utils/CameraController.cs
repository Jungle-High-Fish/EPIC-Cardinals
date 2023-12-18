using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Util;

namespace Cardinals.Util {
    public class CameraController : MonoBehaviour
    {
        ComponentGetter<CinemachineVirtualCamera> _virtualCamera
            = new ComponentGetter<CinemachineVirtualCamera>(TypeOfGetter.This);

        private CinemachineBasicMultiChannelPerlin _perlin;

        private void Awake()
        {
            _perlin = _virtualCamera.Get(gameObject).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            _perlin.m_AmplitudeGain = 0;
            _perlin.m_FrequencyGain = 0;
        }

        public void ShakeCamera(float duration, float amplitudeGain, float frequencyGain)
        {
            _perlin.m_AmplitudeGain = amplitudeGain;
            _perlin.m_FrequencyGain = frequencyGain;
            StartCoroutine(ShakeCameraCoroutine(duration));
        }

        private IEnumerator ShakeCameraCoroutine(float duration)
        {
            yield return new WaitForSeconds(duration);
            _perlin.m_AmplitudeGain = 0;
            _perlin.m_FrequencyGain = 0;
        }

        private bool _zoomInMode;
        private float _speed = 1f;
        float _zoomInValue = 2.2f;
        public IEnumerator PlayerZoomIn()
        {
            bool wait = false;
            var destPos = GameManager.I.Player.transform.position + new Vector3(2, 3, -2);
            _zoomInMode = true;
            transform.DOMove(destPos, _speed)
                .OnComplete(() =>
                {
                    _zoomInMode = false;
                    wait = true;
                });
            
            yield return new WaitUntil(() => wait);
            yield return new WaitForSeconds(.5f);
        }

        public void LateUpdate()
        {
            if (_zoomInMode)
            {
                _virtualCamera.Get().m_Lens.OrthographicSize = Mathf.Lerp(_virtualCamera.Get().m_Lens.OrthographicSize,_zoomInValue, .02f);
            }
        }
    }
}
