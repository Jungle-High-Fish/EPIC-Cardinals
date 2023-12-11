using System;
using System.Collections;
using System.Numerics;
using Cardinals.Board;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace Cardinals.Enemy.Summon
{
    public class Fireball : BaseBoardObject
    {
        [Header("Movement")]
        [SerializeField] private float _maxY;
        [SerializeField] private float _minY;
        
        [Header("info")]
        [SerializeField] private int _damage;
        public int Damage => _damage;

        
        protected override Vector3 GetPosition(Vector3 vector3) => vector3 + new Vector3(0, _maxY, 0);
        
        public void Init(Tile tile)
        {
            base.Init(tile);

            // 위 아래 움직임 
            _renderer.transform.DOMoveY(_minY, Random.Range(1.5f, 2f))
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.OutQuad);
        }
        
        [Button]
        public override IEnumerator OnCollisionPlayer()
        {
            GameManager.I.Player.Hit(_damage);
            
            transform
                .DOShakePosition(.5f, .5f)
                .OnComplete(() =>
                {
                    base.Destroy();
                });
            
            yield return null;
        }
    }
}
