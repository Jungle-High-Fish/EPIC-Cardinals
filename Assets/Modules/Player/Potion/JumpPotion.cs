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

        public override void UsePotion()
        {
            Debug.Log("���� ���");
            GameManager.I.StartCoroutine(GameManager.I.Player.MoveTo(3, 0.5f));
        }
    }
}
