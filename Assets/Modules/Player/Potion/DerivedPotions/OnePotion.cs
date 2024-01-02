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
            if (!GameManager.I.Player.OnTile.IsSealed)
            {
                GameManager.I.Stage.DiceManager.PotionUseAction(1);
                return true;
            }
            return false;
        }

    }
}
