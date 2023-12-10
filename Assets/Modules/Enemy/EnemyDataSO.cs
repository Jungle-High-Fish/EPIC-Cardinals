using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Cardinals.Game {

    [CreateAssetMenu(fileName = "EnemyData", menuName = "Cardinals/EnemyData", order = 1)]
    public class EnemyDataSO: ScriptableObject {
        public string enemyName;
        public EnemyGrade enemyGrade;
        public EnemyType enemyType;
        [AssetSelector(Paths = "Assets/Resources/Prefabs/Enemy/EnemyAnimation")]
        [InlineEditor(InlineEditorModes.LargePreview)] 
        public GameObject prefab;
        [AssetSelector(Paths = "Assets/Resources/Prefabs/Enemy/EnemyAnimation")]
        [InlineEditor(InlineEditorModes.LargePreview)] 
        public GameObject berserkPrefab;
        public int maxHP;
    }

}

