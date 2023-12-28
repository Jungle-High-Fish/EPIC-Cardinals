using Cardinals.Enums;
using Cardinals.Game;
using UnityEngine;
using Util;

namespace Cardinals.UI.Description
{
    public class EnemyActionDescription: MonoBehaviour, IDescription
    {
        private EnemyActionType _type;
        
        public void Init(EnemyActionType type)
        {
            _type = type;
        }

        public string Name => TMPUtils.LocalizedText($"ENEMY_ACTION_{_type.ToString().ToUpper()}_NAME");
        public string Description => TMPUtils.LocalizedText($"ENEMY_ACTION_{_type.ToString().ToUpper()}_EXPLAIN");
        public Sprite IconSprite { get; }
        public Color Color { get; }
        public string Key => $"enemyAction_{_type.ToString()}";
    }
}