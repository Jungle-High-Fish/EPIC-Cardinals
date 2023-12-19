using System.Collections;
using UnityEngine;
using Cardinals.Board;
using Cardinals.BoardEvent;
using DG.Tweening;

namespace Cardinals.Enemy.Summon
{
    /// <summary>
    /// 보드 위에 존재하는 소환수
    /// </summary>
    public abstract class BaseBoardObject : MonoBehaviour, IMovingBoardObject
    {
        public Tile OnTile => _onTile;
        
        [Header("Information")]
        protected Tile _onTile;
        [SerializeField] protected int _moveCount;
        protected virtual int MoveCount => _moveCount; 
        [SerializeField] protected float _moveDuration = .2f;
        
        [Header("Component")]
        [SerializeField] protected SpriteRenderer _renderer;
        [SerializeField] protected Animator _animator;

        public virtual void Init(Tile tile)
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
            _animator = GetComponentInChildren<Animator>();
            
            GameManager.I.Stage.BoardObjects.Add(this);
            _onTile = tile;
            transform.position = GetPosition(_onTile.gameObject.transform.position);
        }

        protected virtual Vector3 GetPosition(Vector3 vector) => vector + new Vector3(0, 1, 0);

        public virtual IEnumerator OnTurn()
        {
            var playerTile = GameManager.I.Player.OnTile; 
            Tile tile = _onTile;
            
            for (int i = 0; i < MoveCount; i++)
            {
                tile = tile.Next;
                transform.DOMove(GetPosition(tile.transform.position), _moveDuration);
                yield return new WaitForSeconds(_moveDuration);
            
                if (playerTile == tile) // 이동했는데 플레이어 타일
                {
                    yield return OnCollisionPlayer();
                    break;
                }
            }
            
            _onTile = tile;
        }
        
        public virtual IEnumerator OnCollisionPlayer()
        {
            yield return null;
        }
        
        public virtual IEnumerator ArrivePlayer()
        {
            yield return null;
        }

        public void Destroy()
        {
            GameManager.I.Stage.BoardObjects.Remove(this);
            Destroy(gameObject);
        }
    }
}