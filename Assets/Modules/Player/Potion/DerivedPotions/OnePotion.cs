using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals
{
    public class OnePotion : Potion
    {
        public OnePotion()
        {
            _potionType = Enums.PotionType.One;
        }

        public override bool UsePotion()
        {
            if (GameManager.I.Player.OnTile.Type == Enums.TileType.Attack ||
                GameManager.I.Player.OnTile.Type == Enums.TileType.Defence)
            {
                BaseEnemy baseEnemy = GameManager.I.Stage.Enemies[Random.Range(0, GameManager.I.Stage.Enemies.Count)];
                GameManager.I.StartCoroutine(GameManager.I.Player.CardAction(1, baseEnemy));
                return true;
            }
            return false;
        }

    }
}
