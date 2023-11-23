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
            GameManager.I.StartCoroutine(GameManager.I.Player.MoveTo(3, 0.5f));
            return true;
        }
    }
}
