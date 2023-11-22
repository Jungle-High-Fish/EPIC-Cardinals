using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
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
        }

        private void OnDisable() {
            GameManager.I.Player.PlayerInfo.AddBlessEvent -= UpdateBlessUI;
        }

        private void ResetBlessUI() {
            foreach(var bless in _blessList) {
                Destroy(bless.gameObject);
            }

            _blessList.Clear();
        }

        private void UpdateBlessUI(BlessType _) {
            if (!_isPanelOpen) return;

            ResetBlessUI();

            var blessList = GameManager.I.Player.PlayerInfo.BlessList;
            foreach (var b in blessList) {
                var blessUIPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_PlayerDetail_Bless);
                var blessUIObj = Instantiate(blessUIPrefab, _blessListArea.Get(gameObject));
                var blessUI = blessUIObj.GetComponent<UIBless>();
                blessUI.Init(b);
                _blessList.Add(blessUI);
            }
        }
    }

}