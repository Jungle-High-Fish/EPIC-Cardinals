using System;
using Cardinals.Enums;
using System.Collections;
using UnityEngine;
using Util;

namespace Cardinals.Board.Curse
{
    public class Seal : TileCurseData
    {
        public Seal() : base(TileCurseType.Seal)
        {
            Action = SealTile();
        }
        
        IEnumerator SealTile()
        {
            Debug.Log("타일이 봉인됩니다.");
            BaseTile.ChangeState(TileState.Seal);

            yield return null;
        }
    }
}