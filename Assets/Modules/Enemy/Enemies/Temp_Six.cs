using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cardinals.Buff;
using Cardinals.Enums;
using Cardinals.Game;
using UnityEngine;

namespace Cardinals.Enemy
{
    public class temp_Six : BaseEnemy
    {
        public override void Init(EnemyDataSO enemyData)
        {
            base.Init(enemyData);

            Patterns = new[]
            {
                new Pattern(EnemyActionType.TileCurse, action: Seal),
                new Pattern(EnemyActionType.Attack, 8),
                new Pattern(EnemyActionType.Defense, 8),
                new Pattern(EnemyActionType.NoAction),
            };
            
            Rewards = new Reward[]
            {
                new(RewardType.Gold, 60),
                new(RewardType.Potion, 1)
            };
        }

        void Seal()
        {
            var list = GameManager.I.Stage.Board.GetCursedTilesList()?.ToList();
            
            if (list != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    var index = Random.Range(0, list.Count);
                    var tile = list[index];
                    tile.SetCurse(TileCurseType.Seal, 2);
                    
                    list.RemoveAt(index);
                }
            }
        }
    }
}