using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Linq;
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
        private bool _zoomOutMode;
        private float _speed = 1f;
        float _zoomInValue = 2.2f;
        float _zoomOutValue = 5.3f;
        public IEnumerator PlayerZoomIn()
        {
            bool wait = false;
            var destPos = GameManager.I.Player.transform.position + new Vector3(2, 3, -2);
            _zoomInValue = 2.2f;
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
            
            if (_zoomOutMode)
            {
                _virtualCamera.Get().m_Lens.OrthographicSize = Mathf.Lerp(_virtualCamera.Get().m_Lens.OrthographicSize,_zoomOutValue, .1f);
            }
        }

        [Button]
        private void test()
        {
            StartCoroutine(EnemyZoomIn());
        }

        private float _backup;
        Vector3 _movePos = new Vector3(-1.5f, 0, 0);
        public IEnumerator EnemyZoomIn()
        {
            float backup = _virtualCamera.Get().m_Lens.OrthographicSize;
            bool wait = false;
            var destPos = transform.position + _movePos;
            
            _zoomInValue = 3.2f;
            _zoomInMode = true;
            transform.DOMove(destPos, _speed).OnComplete(() => { wait = true; });
            
            yield return new WaitUntil(() => wait);
            yield return new WaitUntil(() => MyApproximation(_virtualCamera.Get().m_Lens.OrthographicSize, _zoomInValue));
            _zoomInMode = false;
        }
        
        public IEnumerator EnemyZoomOut()
        {
            bool wait = false;
            var destPos = transform.position - _movePos;
            _zoomOutMode = true;
            transform.DOMove(destPos, .1f).OnComplete(() => { wait = true; });
            
            yield return new WaitUntil(() => wait);
            yield return new WaitUntil(() => MyApproximation(_virtualCamera.Get().m_Lens.OrthographicSize,_zoomOutValue));
            _zoomOutMode = false;
        }
        
        private bool MyApproximation(float a, float b, float tolerance = 0.001f)
        {
            return (Mathf.Abs(a - b) < tolerance);
        }
    }
}
