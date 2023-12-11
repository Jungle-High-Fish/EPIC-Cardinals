using System.Linq;
using Cardinals.Enums;
using Cardinals.Game;
using UnityEngine;
using Util;

namespace Cardinals.Enemy
{
    public class Temp_Seven : BaseEnemy
    {
        public override void Init(EnemyDataSO enemyData)
        {
            base.Init(enemyData);

            Patterns = new[]
            {
                new Pattern(EnemyActionType.Spawn, action: SpawnRyuka),
                new Pattern(EnemyActionType.Attack, 10),
                new Pattern(EnemyActionType.Defense, 10)
            };
            
            Rewards = new Reward[]
            {
                new(RewardType.Gold, 100)
            };
        }

        void SpawnRyuka()
        {
            var list = GameManager.I.Stage.Board.TileSequence
                .Where(t => GameManager.I.Player.OnTile != t).ToList();

            for (int i = 0; i < 2; i++)
            {
                var idx = Random.Range(0, list.Count);
                var tile = list[idx]; 
           
                var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Enemy_Spawn_Ryuka);
                var ryuka = GameObject.Instantiate(prefab).GetComponent<Enemy.Summon.Ryuka>();
                ryuka.Init(tile);
            }
        }
    }
}