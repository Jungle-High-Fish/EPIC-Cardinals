using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;

namespace Cardinals.Game {
    public class ArtifactDataEditor : OdinMenuEditorWindow {
        private CreateNewArtifactData _createNewArtifactData;
        
        [MenuItem("Tools/아티팩트 데이터 편집")]
        private static void OpenWindow() {
            GetWindow<ArtifactDataEditor>().Show();
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            if (_createNewArtifactData != null) {
                _createNewArtifactData.DestoryData();
            }
        }

        protected override OdinMenuTree BuildMenuTree() {
            var tree = new OdinMenuTree();

            _createNewArtifactData = new CreateNewArtifactData();
            tree.Add("새 아티팩트", _createNewArtifactData);

            tree.AddAllAssetsAtPath(
                "아티팩트 리스트", 
                Constants.FilePath.Resources.Get(
                    Constants.FilePath.Resources.SO +
                    Constants.FilePath.Resources.SO_ArtifactData
                ), 
                typeof(ArtifactDataSO)
            );

            return tree;
        }

        protected override void OnBeginDrawEditors() {
            OdinMenuTreeSelection selection = this.MenuTree.Selection;

            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();

                if (SirenixEditorGUI.ToolbarButton("삭제")) {
                    ArtifactDataSO artifactData = (ArtifactDataSO)selection.SelectedValue;
                    string path = AssetDatabase.GetAssetPath(artifactData);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        public class CreateNewArtifactData {
            [Title("아티팩트 설정")]
            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            [ShowInInspector] private ArtifactDataSO _artifactData;
            
            public CreateNewArtifactData() {
                _artifactData = ScriptableObject.CreateInstance<ArtifactDataSO>();
            }

            public void DestoryData() {
                if (_artifactData != null) {
                    DestroyImmediate(_artifactData);
                }
            }

            [Button("생성", ButtonSizes.Large)]
            private void Create() {
                AssetDatabase.CreateAsset(
                    _artifactData, 
                    Constants.FilePath.Resources.Get(
                        Constants.FilePath.Resources.SO +
                        Constants.FilePath.Resources.SO_ArtifactData + 
                        _artifactData.artifactType.ToString() + ".asset"
                    )
                );
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif