using System.Collections;
using System.Linq;
using Cardinals.Enums;
using Cardinals.Game;
using UnityEngine;

namespace Cardinals.Enemy
{
    public class Temp_Nine : BaseEnemy
    {
        private int _sleepCount;
        public override void Init(EnemyDataSO enemyData)
        {
            base.Init(enemyData);

            _sleepCount = 3;
            
            Patterns = new[]
            {
                new Pattern(EnemyActionType.TileCurse, action: AllSeal),
                new Pattern(EnemyActionType.Attack, 15),
                new Pattern(EnemyActionType.Attack, 7),
                new Pattern(EnemyActionType.Defense, 6),
            };
            
            Rewards = new Reward[]
            {
                new(RewardType.Gold, 60),
                new(RewardType.Potion, 1)
            };
        }

        public override IEnumerator StartTurn()
        {
            if (_sleepCount > 0)
            {
                FixPattern = new Pattern(EnemyActionType.Sleep);
            }

            return base.StartTurn();
        }

        void AllSeal()
        {
            var list = GameManager.I.Stage.Board.GetCursedTilesList()?.ToList();

            foreach (var tile in GameManager.I.Stage.Board.TileSequence)
            {
                tile.SetCurse(TileCurseType.Seal, 2);
            }
        }
    }
}