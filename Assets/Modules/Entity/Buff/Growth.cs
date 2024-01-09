using Cardinals.Enums;
using Cardinals.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.Buff
{
    public class Growth : BaseBuff
    {
        public int IncreaseValue { get; private set; }
        
        public Growth() : base(BuffType.Growth, BuffCountDecreaseType.Event)
        {
            Value = 10;
        }

        public override void Execute(BaseEntity entity)
        {
            var evt = GameManager.I.Stage.CurEvent as BattleEvent;
            if (evt != null)
            {
                var backup = IncreaseValue; 
                IncreaseValue = 10 *((evt.Turn + 1) / 10);

                if (backup != IncreaseValue)
                {                    
                    entity.AddBuff(new Growth());
                }
            }
        }
    }
}
