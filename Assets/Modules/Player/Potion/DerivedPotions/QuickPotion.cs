using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cardinals.Enums;

namespace Cardinals
{

    public class QuickPotion : Potion
    {
        public QuickPotion()
        {
            _potionType = PotionType.Quick;
        }

        public override bool UsePotion()
        {
            //GameManager.I.Stage.CardManager.Draw(2);
            return true;
        }
    }
}
