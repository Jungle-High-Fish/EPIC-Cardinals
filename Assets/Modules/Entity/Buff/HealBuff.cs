using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.Buff
{
    public class HealBuff : BaseBuff
    {
        public HealBuff(int count) : base(Enums.BuffType.Heal, count:count){

        }

        public override void Execute(BaseEntity entity)
        {
            entity.Heal(3);
        }
    }

}
