using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;

namespace Cardinals.Game {
    public class EnemyDataEditor : OdinMenuEditorWindow {
        private CreateNewEnemyData _createNewEnemyData;
        
        [MenuItem("Tools/몬스터 데이터 편집")]
        private static void OpenWindow() {
            GetWindow<EnemyDataEditor>().Show();
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            if (_createNewEnemyData != null) {
                _createNewEnemyData.DestoryData();
            }
        }

        protected override OdinMenuTree BuildMenuTree() {
            var tree = new OdinMenuTree();

            _createNewEnemyData = new CreateNewEnemyData();
            tree.Add("새 몬스터", _createNewEnemyData);

            tree.AddAllAssetsAtPath(
                "몬스터 리스트", 
                Constants.FilePath.Resources.Get(
                    Constants.FilePath.Resources.SO +
                    Constants.FilePath.Resources.SO_EnemyData
                ), 
                typeof(EnemyDataSO)
            );

            return tree;
        }

        protected override void OnBeginDrawEditors() {
            OdinMenuTreeSelection selection = this.MenuTree.Selection;

            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();

                if (SirenixEditorGUI.ToolbarButton("삭제")) {
                    EnemyDataSO EnemyData = (EnemyDataSO)selection.SelectedValue;
                    string path = AssetDatabase.GetAssetPath(EnemyData);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        public class CreateNewEnemyData {
            [Title("몬스터 설정")]
            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            [ShowInInspector] private EnemyDataSO _EnemyData;
            
            public CreateNewEnemyData() {
                _EnemyData = ScriptableObject.CreateInstance<EnemyDataSO>();
            }

            public void DestoryData() {
                if (_EnemyData != null) {
                    DestroyImmediate(_EnemyData);
                }
            }

            [Button("생성", ButtonSizes.Large)]
            private void Create() {
                AssetDatabase.CreateAsset(
                    _EnemyData, 
                    Constants.FilePath.Resources.Get(
                        Constants.FilePath.Resources.SO +
                        Constants.FilePath.Resources.SO_EnemyData + 
                        _EnemyData.enemyType.ToString() + ".asset"
                    )
                );
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif