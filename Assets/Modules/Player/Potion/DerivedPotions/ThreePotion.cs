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
            if (!GameManager.I.Player.OnTile.IsSealed)
            {
                GameManager.I.Stage.DiceManager.PotionUseAction(3);
                return true;
            }
            return false;
        }
    }
}
