using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cardinals.Enums;

namespace Cardinals
{

    public class GrimoireSatchel : Artifact
    {
        public GrimoireSatchel(int money, string name, ArtifactType type)
        {
            Money = money;
            Name = name;
            Type = type;
            OnEffect();
        }

        private void OnEffect()
        {
            GameManager.I.Player.SetMaxHP(GameManager.I.Player.MaxHp-10);
        }
    }

}