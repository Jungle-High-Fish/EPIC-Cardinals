using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals
{
    public class ThreePotion : Potion 
    {
        public ThreePotion()
        {
            _potionType = Enums.PotionType.Three;
        }

        public override bool UsePotion()
        {
            if (GameManager.I.Player.OnTile.Type == Enums.TileType.Attack ||
                GameManager.I.Player.OnTile.Type == Enums.TileType.Defence)
            {
                BaseEnemy baseEnemy = GameManager.I.Stage.Enemies[Random.Range(0, GameManager.I.Stage.Enemies.Count)];
                GameManager.I.StartCoroutine(GameManager.I.Player.CardAction(3, baseEnemy));
                return true;
            }
            return false;
        }
    }
}
