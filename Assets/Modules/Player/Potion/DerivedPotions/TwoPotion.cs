using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals
{

    public class TwoPotion : Potion
    {
        public TwoPotion()
        {
            _potionType = Enums.PotionType.Two;
        }

        public override bool UsePotion()
        {
            if (GameManager.I.Stage.Board.IsBoardSquare) {
                if(GameManager.I.Player.OnTile.Type==Enums.TileType.Attack||
                    GameManager.I.Player.OnTile.Type == Enums.TileType.Defence)
                {
                    GameManager.I.Stage.CardManager.PotionUseAction(2);
                    return true;
                }
            } else {
                GameManager.I.Stage.CardManager.PotionUseAction(2);
                return true;
            }

            return false;
        }
    }

}