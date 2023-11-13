using System.Collections;

namespace Cardinals.Game
{
    public class BattleEvent : BaseEvent
    {
        public EnemyType EnemyType { get; }
        
        public BattleEvent(EventType evtType, EnemyType enemyType) : base(evtType)
        {
            EnemyType = enemyType;
        }
    }
}