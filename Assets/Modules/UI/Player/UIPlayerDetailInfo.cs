using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.Game;
using UnityEngine;
using Util;

namespace Cardinals.UI {

    public class UIPlayerDetailInfo : MonoBehaviour {
        private ComponentGetter<Transform> _blessListArea = new ComponentGetter<Transform>(
            TypeOfGetter.ChildByName, 
            "Bless List Panel/Scroll/Viwport/Content"
        );

        private ComponentGetter<Transform> _artifactListArea = new ComponentGetter<Transform>(
            TypeOfGetter.ChildByName, 
            "Artifact List Panel/Scroll/Viwport/Content"
        );

        private ComponentGetter<Transform> _potionListArea = new ComponentGetter<Transform>(
            TypeOfGetter.ChildByName, 
            "Potion List Panel/Potion Slot Area"
        );

        private bool _isPanelOpen;

        private List<UIBless> _blessList = new List<UIBless>();
        private List<UIArtifact> _artifactList = new List<UIArtifact>();
        private List<UIPotion> _potionList = new List<UIPotion>();

        public void Activate() {
            gameObject.SetActive(true);
            _isPanelOpen = true;
        }

        private void OnEnable() {
            GameManager.I.Player.PlayerInfo.AddBlessEvent -= UpdateBlessUI;
            GameManager.I.Player.PlayerInfo.AddBlessEvent += UpdateBlessUI;

            GameManager.I.Player.PlayerInfo.AddArtifactEvent -= UpdateArtifactUI;
            GameManager.I.Player.PlayerInfo.AddArtifactEvent += UpdateArtifactUI;
        }

        private void OnDisable() {
            GameManager.I.Player.PlayerInfo.AddBlessEvent -= UpdateBlessUI;
            GameManager.I.Player.PlayerInfo.AddArtifactEvent -= UpdateArtifactUI;
        }

        private void ResetUIData(IList dataList) {
            foreach(var d in dataList) {
                Destroy((d as MonoBehaviour).gameObject);
            }

            dataList.Clear();
        }

        private void UpdateBlessUI(BlessType _) {
            if (!_isPanelOpen) return;

            ResetUIData(_blessList);

            var blessList = GameManager.I.Player.PlayerInfo.BlessList;
            foreach (var b in blessList) {
                var blessUIPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_PlayerDetail_Bless);
                var blessUIObj = Instantiate(blessUIPrefab, _blessListArea.Get(gameObject));
                var blessUI = blessUIObj.GetComponent<UIBless>();
                blessUI.Init(b);
                _blessList.Add(blessUI);
            }
        }

        private void UpdateArtifactUI(Artifact _) {
            if (!_isPanelOpen) return;

            ResetUIData(_artifactList);

            var artifactList = GameManager.I.Player.PlayerInfo.ArtifactList;
            foreach (var a in artifactList) {
                var artifactUIPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_PlayerDetail_Artifact);
                var artifactUIObj = Instantiate(artifactUIPrefab, _artifactListArea.Get(gameObject));
                var artifactUI = artifactUIObj.GetComponent<UIArtifact>();
                artifactUI.Init(a.Type);
                _artifactList.Add(artifactUI);
            }
        }
    }

}