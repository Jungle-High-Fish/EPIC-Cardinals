using System.Collections;
using Cardinals.Enums;
using UnityEngine;

namespace Cardinals.Game
{
    public class BattleEvent : BaseEvent
    {
        public EnemyType[] EnemyType { get; }
        
        public BattleEvent(params EnemyType[] enemyType) : base(StageEventType.Battle)
        {
            EnemyType = enemyType;
        }

        public Vector3[] GetPositions()
        {

            return new[] { new Vector3()};
        }
    }
}