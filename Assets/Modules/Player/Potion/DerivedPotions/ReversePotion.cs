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
            GameManager.I.Stage.CardManager.CardUsePrevMove(3);
            return true;
        }
    }
}
