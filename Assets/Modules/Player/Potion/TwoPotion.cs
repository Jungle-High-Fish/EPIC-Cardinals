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
            if (GameManager.I.Player.OnTile.Type == Enums.TileType.Attack ||
                GameManager.I.Player.OnTile.Type == Enums.TileType.Defence)
            {
                GameManager.I.Player.CardAction(2, null);
                return true;
            }

            return false;
        }
    }

}