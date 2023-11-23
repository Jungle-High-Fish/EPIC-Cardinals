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
            if(GameManager.I.Player.OnTile.Type==Enums.TileType.Attack||
                GameManager.I.Player.OnTile.Type == Enums.TileType.Defence)
            {
                GameManager.I.Player.CardAction(4, null);
                return true;
            }
            return false;
        }
    }
}
