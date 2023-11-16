using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Cardinals.Board;
namespace Cardinals
{
    public class Player : BaseEntity, IBoardPiece
    {
        [SerializeField] private Board.Tile _onTile;

        public Player() : base(12){

        }
        public override void OnTurn()
        {
            throw new System.NotImplementedException();
        }

        public override void EndTurn()
        {
            base.EndTurn();
        }

      
        public IEnumerator MoveTo(int count,float time)
        {
            for(int i = 0; i < count; i++)
            {
                Vector3 nextPos = _onTile.Next.transform.position;
                nextPos.y += 1.3f;
                transform.DOJump(nextPos, 2, 1, time);
                yield return new WaitForSeconds(time);
                _onTile = _onTile.Next;
            }
            
        }

    }
}

