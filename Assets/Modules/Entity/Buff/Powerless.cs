using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using UnityEngine;

namespace Cardinals.Buff
{
    public class Powerless : BaseBuff
    {
        public Powerless(int count) : base(BuffType.Powerless, BuffCountDecreaseType.Turn, count)
        {
        }
    }
}