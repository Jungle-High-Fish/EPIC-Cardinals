using System.Collections;
using UnityEngine;
using Cardinals.Board;

namespace Cardinals.Enemy.Summon
{
    /// <summary>
    /// 보드 위에 존재하는 소환수
    /// </summary>
    public abstract class BaseEnemySummon : MonoBehaviour
    {
        protected Tile _onTile;
        public Tile OnTile => _onTile;
        
        [Header("Component")]
        [SerializeField] protected SpriteRenderer _renderer;
        [SerializeField] protected Animator _animator;

        protected void Init()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
            _animator = GetComponentInChildren<Animator>();
        }
        
        public virtual IEnumerator OnTurn()
        {
            yield return null;
        }

        public virtual void Execute()
        {
        
        }

        public void Delete()
        {
            Destroy(gameObject);
        }
    }
}