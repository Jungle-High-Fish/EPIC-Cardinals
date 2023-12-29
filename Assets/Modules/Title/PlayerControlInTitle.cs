using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.Title
{
    public class PlayerControlInTitle : MonoBehaviour
    {
        private TileMaker _tile;
        private Animator _animator;
        private EnvController _env;
        
        [Button]
        public void StartFlow()
        {
            _tile = FindObjectOfType<TileMaker>();
            _animator = GetComponentInChildren<Animator>();
            _env = FindObjectOfType<EnvController>();
            StartCoroutine(Flow());
        }
        
        public IEnumerator Flow()
        {
            float interval = .6f;
            while (true)
            {
                _animator.Play("Jump");
                transform.DOJump(transform.position, 1, 1, .5f);
                _env.MoveAll();

                // Camera.main.transform.DOJump(Camera.main.transform.position, .1f, 1, .5f);
                yield return _tile.MoveFlow(interval);
                yield return new WaitForSeconds(interval);
            }
        }
    }

}