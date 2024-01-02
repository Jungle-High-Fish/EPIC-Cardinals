using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Cardinals.Title
{
    public class EnvController : MonoBehaviour
    {
        [SerializeField] private Transform _leftCollider;
        [SerializeField] private Transform _rightCollider;

        private List<Mover> _moverList = new();
        void Start()
        {
            for (int i = 0, cnt = transform.childCount; i < cnt; i++)
            {
                var mover = transform.GetChild(i).GetComponent<Mover>();
                mover.Init(_leftCollider, _rightCollider);
                _moverList.Add(mover);
            }
        }

        public void MoveAll()
        {
            _moverList.ForEach(m => m.Move());
        }
    }
}

