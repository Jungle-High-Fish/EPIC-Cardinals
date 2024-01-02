using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;

namespace Cardinals.Game {
    public class BlessDataEditor : OdinMenuEditorWindow {
        private CreateNewBlessData _createNewBlessData;
        
        [MenuItem("Tools/축복 데이터 편집")]
        private static void OpenWindow() {
            GetWindow<BlessDataEditor>().Show();
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            if (_createNewBlessData != null) {
                _createNewBlessData.DestoryData();
            }
        }

        protected override OdinMenuTree BuildMenuTree() {
            var tree = new OdinMenuTree();

            _createNewBlessData = new CreateNewBlessData();
            tree.Add("새 축복", _createNewBlessData);

            tree.AddAllAssetsAtPath(
                "츅복 리스트", 
                Constants.FilePath.Resources.Get(
                    Constants.FilePath.Resources.SO +
                    Constants.FilePath.Resources.SO_BlessData
                ), 
                typeof(BlessDataSO)
            );

            return tree;
        }

        protected override void OnBeginDrawEditors() {
            OdinMenuTreeSelection selection = this.MenuTree.Selection;

            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();

                if (SirenixEditorGUI.ToolbarButton("삭제")) {
                    BlessDataSO BlessData = (BlessDataSO)selection.SelectedValue;
                    string path = AssetDatabase.GetAssetPath(BlessData);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        public class CreateNewBlessData {
            [Title("축복 설정")]
            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            [ShowInInspector] private BlessDataSO _BlessData;
            
            public CreateNewBlessData() {
                _BlessData = ScriptableObject.CreateInstance<BlessDataSO>();
            }

            public void DestoryData() {
                if (_BlessData != null) {
                    DestroyImmediate(_BlessData);
                }
            }

            [Button("생성", ButtonSizes.Large)]
            private void Create() {
                AssetDatabase.CreateAsset(
                    _BlessData, 
                    Constants.FilePath.Resources.Get(
                        Constants.FilePath.Resources.SO +
                        Constants.FilePath.Resources.SO_BlessData + 
                        _BlessData.blessType.ToString() + ".asset"
                    )
                );
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif