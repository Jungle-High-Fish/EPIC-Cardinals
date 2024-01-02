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
            if(!GameManager.I.Player.OnTile.IsSealed)
            {
                GameManager.I.Stage.DiceManager.PotionUseAction(4);
                return true;
            }
            return false;
        }
    }
}
