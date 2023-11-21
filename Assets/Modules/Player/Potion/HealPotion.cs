using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cardinals.Buff;
namespace Cardinals
{

    public class HealPotion : Potion
    {
        public HealPotion(int money, string name)
        {
            Money = money;
            Name = name;
        }

        public override void UsePotion()
        {
            GameManager.I.Player.AddBuff(new HealBuff(3));
        }
    }
}
