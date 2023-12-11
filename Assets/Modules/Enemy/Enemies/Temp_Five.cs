using System.Collections;
using System.Collections.Generic;
using Cardinals.Buff;
using Cardinals.Enums;
using Cardinals.Game;
using Modules.Entity.Buff;
using UnityEngine;

namespace Cardinals.Enemy
{
    public class temp_Five : BaseEnemy
    {
        
        public override void Init(EnemyDataSO enemyData)
        {
            base.Init(enemyData);

            FixPattern = new Pattern(EnemyActionType.UserDebuff, action: Confusion);
            Patterns = new[]
            {
                new Pattern(EnemyActionType.Attack, 7),
                new Pattern(EnemyActionType.Attack, 5),
            };
            
            Rewards = new Reward[]
            {
                new(RewardType.Gold, 100)
            };
        }

        void Confusion()
        {
            GameManager.I.Player.AddBuff(new Confusion());
        }
    }
}