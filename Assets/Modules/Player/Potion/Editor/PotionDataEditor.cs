using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;

namespace Cardinals.Game {
    public class PotionDataEditor : OdinMenuEditorWindow {
        private CreateNewPotionData _createNewPotionData;
        
        [MenuItem("Tools/포션 데이터 편집")]
        private static void OpenWindow() {
            GetWindow<PotionDataEditor>().Show();
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            if (_createNewPotionData != null) {
                _createNewPotionData.DestoryData();
            }
        }

        protected override OdinMenuTree BuildMenuTree() {
            var tree = new OdinMenuTree();

            _createNewPotionData = new CreateNewPotionData();
            tree.Add("새 포션", _createNewPotionData);

            tree.AddAllAssetsAtPath(
                "포션 리스트", 
                Constants.FilePath.Resources.Get(
                    Constants.FilePath.Resources.SO +
                    Constants.FilePath.Resources.SO_PotionData
                ), 
                typeof(PotionDataSO)
            );

            return tree;
        }

        protected override void OnBeginDrawEditors() {
            OdinMenuTreeSelection selection = this.MenuTree.Selection;

            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();

                if (SirenixEditorGUI.ToolbarButton("삭제")) {
                    PotionDataSO potionData = (PotionDataSO)selection.SelectedValue;
                    string path = AssetDatabase.GetAssetPath(potionData);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        public class CreateNewPotionData {
            [Title("포션 설정")]
            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            [ShowInInspector] private PotionDataSO _potionData;
            
            public CreateNewPotionData() {
                _potionData = ScriptableObject.CreateInstance<PotionDataSO>();
            }

            public void DestoryData() {
                if (_potionData != null) {
                    DestroyImmediate(_potionData);
                }
            }

            [Button("생성", ButtonSizes.Large)]
            private void Create() {
                AssetDatabase.CreateAsset(
                    _potionData, 
                    Constants.FilePath.Resources.Get(
                        Constants.FilePath.Resources.SO +
                        Constants.FilePath.Resources.SO_PotionData + 
                        _potionData.potionType.ToString() + ".asset"
                    )
                );
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif