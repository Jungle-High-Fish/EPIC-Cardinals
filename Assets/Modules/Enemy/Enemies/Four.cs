using System.Linq;
using Cardinals;
using Cardinals.Enums;
using Cardinals.Game;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace Cardinals.Enemy
{
    public class Four :BaseEnemy
    {
        public override void Init(EnemyDataSO enemyData)
        {
            base.Init(enemyData);
            
            Patterns = new[]
            {
                new Pattern(EnemyActionType.TileCurse, action: SpawnFireball),
                new Pattern(EnemyActionType.Attack, 10),
                new Pattern(EnemyActionType.Defense, 7),
            };
            
            
            Rewards = new Reward[]
            {
                new(RewardType.Gold, 80),
                new(RewardType.Potion, 1),
            };
        }

        void SpawnFireball()
        {
            var list = GameManager.I.Stage.Board.GetCursedTilesList().ToList();
            
            if (list != null)
            {
                var index = Random.Range(0, list.Count);
                var tile = list[index];
                
                tile.SetCurse(TileCurseType.Fireball, 2);
            }
        }
    }
}