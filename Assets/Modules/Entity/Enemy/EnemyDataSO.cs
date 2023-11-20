using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Cardinals.Game {

    [CreateAssetMenu(fileName = "EnemyData", menuName = "Cardinals/EnemyData", order = 1)]
    public class EnemyDataSO: ScriptableObject {
        public string enemyName;
        public EnemyType enemyType;
        [InlineEditor(InlineEditorModes.LargePreview)] public Sprite sprite;
        public int maxHP;
    }

}

