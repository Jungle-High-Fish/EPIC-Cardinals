using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals
{
    public class GrowthPotion : Potion
    {
       public GrowthPotion()
       {
            _potionType = Enums.PotionType.Growth;
       }

        public override bool UsePotion()
        {
            if (GameManager.I.Stage.Board.IsBoardSquare) {
                if(GameManager.I.Player.OnTile.Type==Enums.TileType.Attack||
                    GameManager.I.Player.OnTile.Type == Enums.TileType.Defence)
                {
                    GameManager.I.Player.OnTile.TileMagic.GainExpToNextLevel();
                    return true;
                }
            } else {
                GameManager.I.Player.OnTile.TileMagic.GainExpToNextLevel();
                return true;
            }
                
            return false;
        }
    }
}
