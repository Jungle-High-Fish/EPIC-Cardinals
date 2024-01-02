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
            if (!GameManager.I.Player.OnTile.IsSealed)
            {
                GameManager.I.Stage.DiceManager.PotionUseAction(2);
                return true;
            }
            return false;
        }
    }

}