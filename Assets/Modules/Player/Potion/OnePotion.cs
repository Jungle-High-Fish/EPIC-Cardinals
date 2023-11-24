using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals
{
    public class OnePotion : Potion
    {
        public OnePotion()
        {
            _potionType = Enums.PotionType.One;
        }

        public override bool UsePotion()
        {
            if (GameManager.I.Player.OnTile.Type == Enums.TileType.Attack ||
                GameManager.I.Player.OnTile.Type == Enums.TileType.Defence)
            {
                GameManager.I.Player.CardAction(1, null);
                return true;
            }
            return false;
        }

    }
}