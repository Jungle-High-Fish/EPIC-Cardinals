using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;

using Cardinals.Board;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;

namespace Cardinals.Game {
    public class BoardEventDataEditor : OdinMenuEditorWindow {
        private CreateNewBoardEventData _createNewBoardEventData;
        
        [MenuItem("Tools/이벤트 데이터 편집")]
        private static void OpenWindow() {
            GetWindow<BoardEventDataEditor>().Show();
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            if (_createNewBoardEventData != null) {
                _createNewBoardEventData.DestoryData();
            }
        }

        protected override OdinMenuTree BuildMenuTree() {
            var tree = new OdinMenuTree();

            _createNewBoardEventData = new CreateNewBoardEventData();
            tree.Add("새 이벤트", _createNewBoardEventData);

            tree.AddAllAssetsAtPath(
                "이벤트 리스트", 
                Constants.FilePath.Resources.Get(
                    Constants.FilePath.Resources.SO +
                    Constants.FilePath.Resources.SO_BoardObjectData
                ), 
                typeof(BoardEventDataSO)
            );

            return tree;
        }

        protected override void OnBeginDrawEditors() {
            OdinMenuTreeSelection selection = this.MenuTree.Selection;

            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();

                if (SirenixEditorGUI.ToolbarButton("삭제")) {
                    BoardEventDataSO BoardEventData = (BoardEventDataSO)selection.SelectedValue;
                    string path = AssetDatabase.GetAssetPath(BoardEventData);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        public class CreateNewBoardEventData {
            [Title("이벤트 설정")]
            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            [ShowInInspector] private BoardEventDataSO _BoardEventData;
            
            public CreateNewBoardEventData() {
                _BoardEventData = ScriptableObject.CreateInstance<BoardEventDataSO>();
            }

            public void DestoryData() {
                if (_BoardEventData != null) {
                    DestroyImmediate(_BoardEventData);
                }
            }

            [Button("생성", ButtonSizes.Large)]
            private void Create() {
                AssetDatabase.CreateAsset(
                    _BoardEventData, 
                    Constants.FilePath.Resources.Get(
                        Constants.FilePath.Resources.SO +
                        Constants.FilePath.Resources.SO_BoardObjectData + 
                        _BoardEventData.eventType.ToString() + ".asset"
                    )
                );
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif