using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;


using Cardinals.Tutorial;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

namespace Cardinals.Game {
    public class TutorialDataEditor : OdinMenuEditorWindow {
        private CreateNewTutorialData _createNewTutorialData;
        
        [MenuItem("Tools/튜토리얼 데이터 편집")]
        private static void OpenWindow() {
            GetWindow<TutorialDataEditor>().Show();
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            if (_createNewTutorialData != null) {
                _createNewTutorialData.DestoryData();
            }
        }

        protected override OdinMenuTree BuildMenuTree() {
            var tree = new OdinMenuTree();

            _createNewTutorialData = new CreateNewTutorialData();
            tree.Add("새 튜토리얼", _createNewTutorialData);

            tree.AddAllAssetsAtPath(
                "튜토리얼 리스트", 
                Constants.FilePath.Resources.Get(
                    Constants.FilePath.Resources.SO +
                    Constants.FilePath.Resources.SO_TutorialData
                ), 
                typeof(TutorialDataSO)
            );

            return tree;
        }

        protected override void OnBeginDrawEditors() {
            OdinMenuTreeSelection selection = this.MenuTree.Selection;

            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();

                if (SirenixEditorGUI.ToolbarButton("삭제")) {
                    TutorialDataSO TutorialData = (TutorialDataSO)selection.SelectedValue;
                    string path = AssetDatabase.GetAssetPath(TutorialData);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        public class CreateNewTutorialData {
            [Title("튜토리얼 설정")]
            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            [ShowInInspector] private TutorialDataSO _TutorialData;
            
            public CreateNewTutorialData() {
                _TutorialData = ScriptableObject.CreateInstance<TutorialDataSO>();
            }

            public void DestoryData() {
                if (_TutorialData != null) {
                    DestroyImmediate(_TutorialData);
                }
            }

            [Button("생성", ButtonSizes.Large)]
            private void Create() {
                AssetDatabase.CreateAsset(
                    _TutorialData, 
                    Constants.FilePath.Resources.Get(
                        Constants.FilePath.Resources.SO +
                        Constants.FilePath.Resources.SO_TutorialData + 
                        _TutorialData.TutorialName.ToString() + ".asset"
                    )
                );
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif