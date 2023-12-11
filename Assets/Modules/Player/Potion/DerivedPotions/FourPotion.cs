using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals
{
    public class FourPotion : Potion
    {
        public FourPotion()
        {
            _potionType = Enums.PotionType.Four;
        }

        public override bool UsePotion()
        {
            if (GameManager.I.Stage.Board.IsBoardSquare) {
                if(GameManager.I.Player.OnTile.Type==Enums.TileType.Attack||
                    GameManager.I.Player.OnTile.Type == Enums.TileType.Defence)
                {
                    GameManager.I.Stage.CardManager.PotionUseAction(4);
                    return true;
                }
            } else {
                GameManager.I.Stage.CardManager.PotionUseAction(4);
                return true;
            }
            
            return false;
        }
    }
}
