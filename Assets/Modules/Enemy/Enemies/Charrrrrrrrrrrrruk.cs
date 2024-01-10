using System.Collections;
using System.Linq;
using Cardinals.Enums;
using Cardinals.Game;
using Cardinals.Board;
using Cardinals.Buff;
using UnityEngine;

namespace Cardinals.Enemy
{
    public class Charrrrrrrrrrrrruk : BaseEnemy
    {
        private int _sleepCount;
        public override void Init(EnemyDataSO enemyData)
        {
            base.Init(enemyData);

            _sleepCount = 2;

            AllSeal();
            Patterns = new[]
            {
                new Pattern(EnemyActionType.Attack, 15),
                new Pattern(EnemyActionType.Attack, 10),
                new Pattern(EnemyActionType.UserDebuff, action: Powerless),
                new Pattern(EnemyActionType.Attack, 10),
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
                _sleepCount--;
                FixPattern = new Pattern(EnemyActionType.Sleep);
            }
            else
            {
                BerserkMode = true;
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

        void Powerless()
        {
            GameManager.I.Player.AddBuff(new Powerless(3));
        }
    }
}