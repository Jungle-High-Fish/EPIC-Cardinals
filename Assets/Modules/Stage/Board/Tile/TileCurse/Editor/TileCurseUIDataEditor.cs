using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;

using Cardinals.Board;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;

namespace Cardinals.Game {
    public class TileCurseUIDataEditor : OdinMenuEditorWindow {
        private CreateNewTileCurseUIData _createNewTileCurseUIData;
        
        [MenuItem("Tools/저주 데이터 편집")]
        private static void OpenWindow() {
            GetWindow<TileCurseUIDataEditor>().Show();
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            if (_createNewTileCurseUIData != null) {
                _createNewTileCurseUIData.DestoryData();
            }
        }

        protected override OdinMenuTree BuildMenuTree() {
            var tree = new OdinMenuTree();

            _createNewTileCurseUIData = new CreateNewTileCurseUIData();
            tree.Add("새 저주", _createNewTileCurseUIData);

            tree.AddAllAssetsAtPath(
                "저주 리스트", 
                Constants.FilePath.Resources.Get(
                    Constants.FilePath.Resources.SO +
                    Constants.FilePath.Resources.SO_TileCurseUIData
                ), 
                typeof(TileCurseUIDataSO)
            );

            return tree;
        }

        protected override void OnBeginDrawEditors() {
            OdinMenuTreeSelection selection = this.MenuTree.Selection;

            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();

                if (SirenixEditorGUI.ToolbarButton("삭제")) {
                    TileCurseUIDataSO TileCurseUIData = (TileCurseUIDataSO)selection.SelectedValue;
                    string path = AssetDatabase.GetAssetPath(TileCurseUIData);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        public class CreateNewTileCurseUIData {
            [Title("저주 설정")]
            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            [ShowInInspector] private TileCurseUIDataSO _TileCurseUIData;
            
            public CreateNewTileCurseUIData() {
                _TileCurseUIData = ScriptableObject.CreateInstance<TileCurseUIDataSO>();
            }

            public void DestoryData() {
                if (_TileCurseUIData != null) {
                    DestroyImmediate(_TileCurseUIData);
                }
            }

            [Button("생성", ButtonSizes.Large)]
            private void Create() {
                AssetDatabase.CreateAsset(
                    _TileCurseUIData, 
                    Constants.FilePath.Resources.Get(
                        Constants.FilePath.Resources.SO +
                        Constants.FilePath.Resources.SO_TileCurseUIData + 
                        _TileCurseUIData.curseType.ToString() + ".asset"
                    )
                );
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif