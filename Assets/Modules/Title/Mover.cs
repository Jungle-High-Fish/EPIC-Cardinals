using DG.Tweening;
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
        [SerializeField] private float _speed = -2;
        public void Init(Transform left, Transform right)
        {
            _leftColliderTr = left;
            _rightColliderTr = right;
            // SetMover();
        }

        private void Update()
        {
            //transform.position -= new Vector3(_speed, 0, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Equals(_leftColliderTr.gameObject.name))
            {
                transform.DOKill();
                var value = 2 + (_backup - transform.position.x) + Random.Range(0, 1f);

                var pos = transform.position + new Vector3(-value, 0, 0);
                pos.x = _rightColliderTr.position.x;
                transform.position = pos;
                //SetMover();
            }
        }

        private void SetMover()
        {
            _speed = Random.Range(.005f, .01f);
        }

        private float _backup;
        public void Move()
        {
            _backup = transform.position.x; 
            transform.DOMoveX( _speed, .5f).SetRelative();
        }
    }
}
