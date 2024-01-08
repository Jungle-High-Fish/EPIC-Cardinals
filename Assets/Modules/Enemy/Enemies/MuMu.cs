using System.Linq;
using Cardinals.Enums;
using Cardinals.Game;
using Steamworks.Data;
using Unity.VisualScripting;
using UnityEngine;
using Util;

namespace Cardinals.Enemy
{
    public class MuMu : BaseEnemy
    {
        public override void Init(EnemyDataSO enemyData)
        {
            base.Init(enemyData);

            Patterns = new[]
            {
                new Pattern(EnemyActionType.Spawn, action: SpawnRyuka),
                new Pattern(EnemyActionType.Attack, 10),
                new Pattern(EnemyActionType.Defense, 7)
            };
            
            Rewards = new Reward[]
            {
                new(RewardType.Gold, 100)
            };
        }

        void SpawnRyuka()
        {
            var list = GameManager.I.Stage.Board.TileSequence
                .Where(t => GameManager.I.Player.OnTile != t)
                .Where(t => 
                    !GameManager.I.Stage.BoardObjects.Where(obj => obj.Data.objType != default).Any(obj => obj.OnTile == t))
                .ToList();

            for (int i = 0; i < 2 && i < list.Count; i++)
            {
                var idx = Random.Range(0, list.Count);
                var tile = list[idx];
                list.Remove(tile);
           
                var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_BoardEventObject);
                var obj = Instantiate(prefab);
                obj.AddComponent<BoardObject.Summon.Ryuka>().Init(tile, NewBoardObjectType.Ryuka.ToString());
            }

            if (list.Count == 0)
            {
                GameManager.I.SteamHandler.TriggerAchievement("Cryukabouche");
            }
        }
    }
}