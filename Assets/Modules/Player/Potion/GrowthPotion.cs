using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals
{
    public class GrowthPotion : Potion
    {
       public GrowthPotion()
       {
            _potionType = Enums.PotionType.Growth;
       }

        public override bool UsePotion()
        {
            GameManager.I.Player.OnTile.TileMagic.MagicLevelUp();
            return true;
        }
    }
}
