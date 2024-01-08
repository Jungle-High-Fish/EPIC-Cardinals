using System.Linq;
using Cardinals;
using Cardinals.Enums;
using Cardinals.Game;
using UnityEngine;
using Cardinals.Board;
using Cardinals.Buff;
using UnityEngine.ProBuilder;

namespace Cardinals.Enemy
{
    public class PicPic :BaseEnemy
    {
        public override void Init(EnemyDataSO enemyData)
        {
            base.Init(enemyData);
            
            Patterns = new[]
            {
                new Pattern(EnemyActionType.TileCurse, action: SpawnFireball),
                new Pattern(EnemyActionType.Attack, 10),
                new Pattern(EnemyActionType.UserDebuff, action: Weak),
                new Pattern(EnemyActionType.Defense, 7),
                new Pattern(EnemyActionType.Attack, 10),
                new Pattern(EnemyActionType.Attack, 8),
            };
            
            
            Rewards = new Reward[]
            {
                new(RewardType.Gold, 80),
                new(RewardType.Potion, 1),
            };
        }

        void SpawnFireball()
        {
            var list = GameManager.I.Stage.Board.GetCursedTilesList()?.ToList();
            
            if (list != null)
            {
                var index = Random.Range(0, list.Count);
                var tile = list[index];
                
                tile.SetCurse(TileCurseType.Fireball, 2);
            }
        }

        void Weak()
        {
            GameManager.I.Player.AddBuff(new Weak(3));
        }
    }
}