using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals
{
    public class ThreePotion : Potion 
    {
        public ThreePotion()
        {
            _potionType = Enums.PotionType.Three;
        }

        public override bool UsePotion()
        {
            if (GameManager.I.Player.OnTile.Type == Enums.TileType.Attack ||
                GameManager.I.Player.OnTile.Type == Enums.TileType.Defence)
            {
                GameManager.I.Player.CardAction(3, null);
                return true;
            }
            return false;
        }
    }
}
