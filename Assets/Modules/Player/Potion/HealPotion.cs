using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cardinals.Buff;
using Cardinals.Enums;

namespace Cardinals
{

    public class HealPotion : Potion
    {
        public HealPotion()
        {
            _potionType = PotionType.Heal;
        }

        public override bool UsePotion()
        {
            GameManager.I.Player.AddBuff(new HealBuff(3));
            return true;
        }
    }
}
