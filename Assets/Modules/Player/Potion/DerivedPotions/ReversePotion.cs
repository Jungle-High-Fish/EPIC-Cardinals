using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cardinals.Enums;

namespace Cardinals
{
    public class ReversePotion : Potion
    {
        public ReversePotion()
        {
            _potionType = PotionType.Reverse;
        }

        public override bool UsePotion()
        {
            if (!GameManager.I.Stage.DiceManager.PotionUsePrevMove(3)) return false;
            return true;
        }
    }
}
