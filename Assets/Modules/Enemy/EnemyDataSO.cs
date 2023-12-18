using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Util;

namespace Cardinals.Game {

    [CreateAssetMenu(fileName = "EnemyData", menuName = "Cardinals/EnemyData", order = 1)]
    public class EnemyDataSO: ScriptableObject {
        public string enemyName;
        [FormerlySerializedAs("enemyGrade")] public EnemyGradeType enemyGradeType;
        public EnemyType enemyType;
        [AssetSelector(Paths = "Assets/Resources/Prefabs/Enemy/EnemyAnimation")]
        [InlineEditor(InlineEditorModes.LargePreview)] 
        public GameObject prefab;
        [AssetSelector(Paths = "Assets/Resources/Prefabs/Enemy/EnemyAnimation")]
        [InlineEditor(InlineEditorModes.LargePreview)] 
        public GameObject berserkPrefab;
        public int maxHP;

        public static EnemyDataSO Data(EnemyType enemyType) {
            return ResourceLoader.LoadSO<EnemyDataSO>(Constants.FilePath.Resources.SO_EnemyData + enemyType.ToString());
        }
    }

}

