using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cardinals.Title
{
    public class Mover : MonoBehaviour
    {
        private Transform _leftColliderTr;
        private Transform _rightColliderTr;
        private float _speed = 0;
        
        public void Init(Transform left, Transform right)
        {
            _leftColliderTr = left;
            _rightColliderTr = right;
            SetMover();
        }

        private void Update()
        {
            transform.position -= new Vector3(_speed, 0, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Equals(_leftColliderTr.gameObject.name))
            {
                var pos = transform.position;
                pos.x = _rightColliderTr.position.x;
                transform.position = pos;
                SetMover();
            }
        }

        private void SetMover()
        {
            _speed = Random.Range(.005f, .01f);
        }
    }
}
