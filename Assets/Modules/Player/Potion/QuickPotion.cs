using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals
{

    public class QuickPotion : Potion
    {
        public QuickPotion(int money, string name)
        {
            Money = money;
            Name = name;
        }

        public override void UsePotion()
        {
            GameManager.I.Stage.CardManager.Draw(2);
        }
    }
}
