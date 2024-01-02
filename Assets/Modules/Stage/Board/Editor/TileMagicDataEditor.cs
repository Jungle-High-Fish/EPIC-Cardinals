using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;

namespace Cardinals.Game {
    public class TileMagicDataEditor : OdinMenuEditorWindow {
        private CreateNewTileMagicData _createNewTileMagicData;
        
        [MenuItem("Tools/원소 데이터 편집")]
        private static void OpenWindow() {
            GetWindow<TileMagicDataEditor>().Show();
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            if (_createNewTileMagicData != null) {
                _createNewTileMagicData.DestoryData();
            }
        }

        protected override OdinMenuTree BuildMenuTree() {
            var tree = new OdinMenuTree();

            _createNewTileMagicData = new CreateNewTileMagicData();
            tree.Add("새 원소", _createNewTileMagicData);

            tree.AddAllAssetsAtPath(
                "원소 리스트", 
                Constants.FilePath.Resources.Get(
                    Constants.FilePath.Resources.SO +
                    Constants.FilePath.Resources.SO_MagicData
                ), 
                typeof(TileMagicDataSO)
            );

            return tree;
        }

        protected override void OnBeginDrawEditors() {
            OdinMenuTreeSelection selection = this.MenuTree.Selection;

            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();

                if (SirenixEditorGUI.ToolbarButton("삭제")) {
                    TileMagicDataSO TileMagicData = (TileMagicDataSO)selection.SelectedValue;
                    string path = AssetDatabase.GetAssetPath(TileMagicData);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        public class CreateNewTileMagicData {
            [Title("원소 설정")]
            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            [ShowInInspector] private TileMagicDataSO _tileMagicData;
            
            public CreateNewTileMagicData() {
                _tileMagicData = ScriptableObject.CreateInstance<TileMagicDataSO>();
            }

            public void DestoryData() {
                if (_tileMagicData != null) {
                    DestroyImmediate(_tileMagicData);
                }
            }

            [Button("생성", ButtonSizes.Large)]
            private void Create() {
                AssetDatabase.CreateAsset(
                    _tileMagicData, 
                    Constants.FilePath.Resources.Get(
                        Constants.FilePath.Resources.SO +
                        Constants.FilePath.Resources.SO_MagicData + 
                        _tileMagicData.magicType.ToString() + ".asset"
                    )
                );
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif