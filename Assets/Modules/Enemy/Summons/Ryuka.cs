using System.Collections;
using System.Linq;
using Cardinals.Board;
using DG.Tweening;
using UnityEngine;

namespace Cardinals.Enemy.Summon
{
    public class Ryuka : BaseEnemySummon
    {
        public void Init()
        {
            
        }
        //Vector3 GetPosition(Vector3 vector3) => vector3 + new Vector3(0, _maxY, 0);
        
        public override IEnumerator OnTurn()
        {
            // var playerTile = GameManager.I.Player.OnTile; 
            // Tile tile = _onTile;
            //
            // for (int i = 0; i < _moveCount; i++)
            // {
            //     tile = tile.Next;
            //     transform.DOMove(GetPosition(tile.transform.position), _moveDuration);
            //     yield return new WaitForSeconds(_moveDuration);
            //
            //     if (playerTile == tile) // 이동했는데 플레이어 타일
            //     {
            //         Execute();
            //         break;
            //     }
            // }
            //
            // SetTile(tile);
            
            yield return null;
        }

        void Execute()
        {
            var enemy = GameManager.I.CurrentEnemies.FirstOrDefault();

            if (enemy != null)
            {
                enemy.Heal(2);
            }
        }
    }
}