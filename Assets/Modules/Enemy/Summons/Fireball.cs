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
    public class Fireball : BaseEnemySummon
    {
        [Header("Movement")]
        [SerializeField] private float _maxY;
        [SerializeField] private float _minY;

        [SerializeField] private float _moveDuration;
        
        [Header("info")]
        [SerializeField] private float _moveCount;
        [SerializeField] private int _damage;
        public int Damage => _damage;

        
        Vector3 GetPosition(Vector3 vector3) => vector3 + new Vector3(0, _maxY, 0);
        public void Init(Tile tile)
        {
            base.Init();
            GameManager.I.Stage.Summons.Add(this);
            
            _onTile = tile;
            transform.position = GetPosition(_onTile.gameObject.transform.position);

            // 위 아래 움직임 
            _renderer.transform.DOMoveY(_minY, Random.Range(1.5f, 2f))
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.OutQuad);
        }

        public override IEnumerator OnTurn()
        {
            var playerTile = GameManager.I.Player.OnTile; 
            Tile tile = _onTile;
            
            for (int i = 0; i < _moveCount; i++)
            {
                tile = tile.Next;
                transform.DOMove(GetPosition(tile.transform.position), _moveDuration);
                yield return new WaitForSeconds(_moveDuration);

                if (playerTile == tile) // 이동했는데 플레이어 타일
                {
                    Execute();
                    break;
                }
            }
            
            SetTile(tile);
            
            yield return null;
        }

        private void SetTile(Tile tile) // [TODO] 만약 타일에 구독 가능한 형태라면...
        {
            if (_onTile != null)
            {
                // 기존 타일에 이벤트 연결 끊기    
            }
            
            // 플레이어가 지나갈 때 이벤트 연결하기

            _onTile = tile;
        }
        
        [Button]
        public override void Execute()
        {
            GameManager.I.Player.Hit(_damage);
            transform
                .DOShakePosition(.5f, .5f)
                .OnComplete(() =>
                {
                    Delete();
                });
        }

        public void OnDestroy()
        {
            GameManager.I.Stage.Summons.Remove(this);
        }
    }
}
