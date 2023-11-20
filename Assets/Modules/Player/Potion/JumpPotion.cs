using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals
{
    public class JumpPotion : Potion
    {
        public JumpPotion(int money, string name)
        {
            Money = money;
            Name = name;
        }

        public override void UsePotion()
        {
            Debug.Log("물약 사용");
            GameManager.I.StartCoroutine(GameManager.I.Player.MoveTo(3, 0.5f));
        }
    }
}
