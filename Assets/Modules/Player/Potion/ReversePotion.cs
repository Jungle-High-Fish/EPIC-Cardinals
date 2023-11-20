using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals
{
    public class ReversePotion : Potion
    {
        public ReversePotion(int money, string name)
        {
            Money = money;
            Name = name;
        }

        public override void UsePotion()
        {
            GameManager.I.StartCoroutine(GameManager.I.Player.PrevMoveTo(3, 0.5f));
        }
    }
}
