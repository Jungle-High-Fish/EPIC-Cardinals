using System;
using Cardinals.Enums;

namespace Cardinals.Enemy
{
    public class Pattern
    {
        public EnemyActionType Type { get; }
        public int? Value { get; }
        
        public int CalculValue { get; set; }
        
        public Action Action { get; }
        
        public Action PreAction { get; }
        
        public Pattern(EnemyActionType type, int? value = null, Action action = null, Action preAction = null)
        {
            Type = type;
            Value = value;
            Action = action;
            PreAction = preAction;
        }
    }
}