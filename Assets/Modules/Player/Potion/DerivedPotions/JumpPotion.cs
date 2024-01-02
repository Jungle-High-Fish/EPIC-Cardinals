using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cardinals.Enums;

namespace Cardinals
{
    public class JumpPotion : Potion
    {
        public JumpPotion()
        {
            _potionType = PotionType.Jump;
        }

        public override bool UsePotion()
        {
            if (!GameManager.I.Stage.DiceManager.PotionUseMove(3)) return false;
            return true;
        }
    }
}
