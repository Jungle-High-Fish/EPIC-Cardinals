using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cardinals.Enums;

namespace Cardinals.Game
{
    public class GrimoireSatchel : Artifact
    {
        public GrimoireSatchel(ArtifactType type)
        {
            Type = type;
            OnEffect();
        }

        private void OnEffect()
        {
            GameManager.I.Player.SetMaxHP(GameManager.I.Player.MaxHp - 10);
            GameManager.I.Player.Hp = 30;
            
        }
    }
}