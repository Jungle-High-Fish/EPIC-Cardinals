using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;

namespace Cardinals.Board {
    public class BoardDataEditor : OdinMenuEditorWindow {
        private CreateNewBoardData _createNewBoardData;
        
        [MenuItem("Tools/보드 데이터 편집")]
        private static void OpenWindow() {
            GetWindow<BoardDataEditor>().Show();
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            if (_createNewBoardData != null) {
                _createNewBoardData.DestoryData();
            }
        }

        protected override OdinMenuTree BuildMenuTree() {
            var tree = new OdinMenuTree();

            _createNewBoardData = new CreateNewBoardData();
            tree.Add("새 보드", _createNewBoardData);

            tree.AddAllAssetsAtPath(
                "보드 데이터 리스트", 
                Constants.FilePath.Resources.Get(
                    Constants.FilePath.Resources.SO +
                    Constants.FilePath.Resources.SO_BoardData
                ), 
                typeof(BoardDataSO)
            );

            return tree;
        }

        protected override void OnBeginDrawEditors() {
            OdinMenuTreeSelection selection = this.MenuTree.Selection;

            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();

                if (SirenixEditorGUI.ToolbarButton("삭제")) {
                    BoardDataSO boardData = (BoardDataSO)selection.SelectedValue;
                    string path = AssetDatabase.GetAssetPath(boardData);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        public class CreateNewBoardData {
            [Title("기본 설정")]
            [ShowInInspector, LabelText("파일 이름")]
            private static string _fileName = "NewBoardData";

            [Title("보드 설정")]
            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            [ShowInInspector] private BoardDataSO _boardData;
            
            public CreateNewBoardData() {
                _boardData = ScriptableObject.CreateInstance<BoardDataSO>();
                _boardData.Init("New Board");
            }

            public void DestoryData() {
                if (_boardData != null) {
                    DestroyImmediate(_boardData);
                }
            }

            [Button("새 보드 생성", ButtonSizes.Large)]
            private void Create() {
                AssetDatabase.CreateAsset(
                    _boardData, 
                    Constants.FilePath.Resources.Get(
                        Constants.FilePath.Resources.SO +
                        Constants.FilePath.Resources.SO_BoardData + 
                        _fileName + ".asset"
                    )
                );
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif