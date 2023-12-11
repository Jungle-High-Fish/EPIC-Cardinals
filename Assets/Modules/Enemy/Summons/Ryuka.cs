using System.Collections;
using System.Linq;
using Cardinals.Board;
using DG.Tweening;
using UnityEngine;

namespace Cardinals.Enemy.Summon
{
    public class Ryuka : BaseEnemySummon
    {
        [Header("Data")]
        [SerializeField] private int _moveCount;
        [SerializeField] private float _moveDuration;

        public void Init(Tile tile)
        {
            base.Init();
            GameManager.I.Stage.Summons.Add(this);
            
            _onTile = tile;
            transform.position = GetPosition(_onTile.gameObject.transform.position);
        }

        Vector3 GetPosition(Vector3 vector) => vector + new Vector3(0, 1, 0);
        
        public override IEnumerator OnTurn()
        {
            TouchedPlayer();
            
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

        void TouchedPlayer()
        {
            var enemy = GameManager.I.CurrentEnemies.FirstOrDefault();

            if (enemy != null)
            {
                enemy.Heal(2);
            }
        }

        public override void Execute()
        {
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