using System;
using Cardinals.Enums;

namespace Cardinals.Enemy
{
    public class Pattern
    {
        public EnemyActionType Type { get; }
        public int? Value { get; }
        
        public Action Action { get; }
        public Pattern(EnemyActionType type, int? value = null, Action action = null)
        {
            Type = type;
            Value = value;
            Action = action;
        }
    }
}