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
            FindAnyObjectByType<CardManager>().OnTurn();
        }

        public override void EndTurn()
        {
            base.EndTurn();
            GameManager.I.Next();
            FindAnyObjectByType<CardManager>().EndTurn();
        }

        public void SetTile(Board.Tile tile)
        {
            _onTile = tile;
            transform.position = tile.transform.position + new Vector3(0, 1.3f, 0);
        }
      
        public IEnumerator MoveTo(int count,float time)
        {
            _onTile?.Leave(this);
            for(int i = 0; i < count; i++)
            {
                Vector3 nextPos = _onTile.Next.transform.position;
                nextPos.y += 1.3f;
                transform.DOJump(nextPos, 2, 1, time);
                yield return new WaitForSeconds(time);
                _onTile = _onTile.Next;
            }
            _onTile.Arrive(this);
        }

        public IEnumerator CardAction(int num) {
            _onTile.CardAction(num);
            yield return null;
        }
    }
}

