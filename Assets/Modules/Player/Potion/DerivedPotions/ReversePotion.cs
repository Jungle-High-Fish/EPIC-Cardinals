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
            GameManager.I.StartCoroutine(GameManager.I.Player.PrevMoveTo(3, 0.5f));
            return true;
        }
    }
}
