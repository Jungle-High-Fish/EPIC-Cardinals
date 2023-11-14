using System.Collections;
using Cardinals.Enums;
using UnityEngine;

namespace Cardinals.Game
{
    public class BattleEvent : BaseEvent
    {
        public EnemyType[] EnemyType { get; }
        
        public BattleEvent(StageEventType evtType, params EnemyType[] enemyType) : base(evtType)
        {
            EnemyType = enemyType;
        }

        public Vector3[] GetPositions()
        {

            return new[] { new Vector3()};
        }
    }
}