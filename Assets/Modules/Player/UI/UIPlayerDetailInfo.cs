using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.Game;
using TMPro;
using UnityEngine;
using Util;

namespace Cardinals.UI {

    public class UIPlayerDetailInfo : MonoBehaviour {
        private ComponentGetter<Transform> _blessListArea = new ComponentGetter<Transform>(
            TypeOfGetter.ChildByName, 
            "Bless List Panel/Scroll/Viewport/Content"
        );

        private ComponentGetter<Transform> _artifactListArea = new ComponentGetter<Transform>(
            TypeOfGetter.ChildByName, 
            "Artifact List Panel/Scroll/Viewport/Content"
        );

        private ComponentGetter<Transform> _potionListArea = new ComponentGetter<Transform>(
            TypeOfGetter.ChildByName, 
            "Potion List Panel/Potion Slot Area"
        );

        private ComponentGetter<TextMeshProUGUI> _moneyText = new ComponentGetter<TextMeshProUGUI>(
            TypeOfGetter.ChildByName, 
            "Money Panel/Money Text"
        );

        private bool _isPanelOpen;

        private List<UIBless> _blessList = new List<UIBless>();
        private List<UIArtifact> _artifactList = new List<UIArtifact>();
        private List<UIPotion> _potionList = new List<UIPotion>();

        public void Init() {
            InstantiatePotionUI();
        }

        public void Activate() {
            gameObject.SetActive(true);
            _isPanelOpen = true;

            if (GameManager.I.Stage == null) return;
            if (GameManager.I.Stage.Player == null) return;

            UpdateBlessUI(BlessType.Empty);
            UpdateArtifactUI(null);
            UpdatePotionUI(0, null);
            UpdateMoneyUI(0);
        }

        public void Deactivate() {
            gameObject.SetActive(false);
            _isPanelOpen = false;
        }

        private void OnEnable() {
            if (GameManager.I.Stage == null) return;
            if (GameManager.I.Stage.Player == null) return;
            
            GameManager.I.Player.PlayerInfo.AddBlessEvent -= UpdateBlessUI;
            GameManager.I.Player.PlayerInfo.AddBlessEvent += UpdateBlessUI;
            
            GameManager.I.Player.PlayerInfo.AddArtifactEvent -= UpdateArtifactUI;
            GameManager.I.Player.PlayerInfo.AddArtifactEvent += UpdateArtifactUI;
            
            GameManager.I.Player.PlayerInfo.AddPotionEvent -= UpdatePotionUI;
            GameManager.I.Player.PlayerInfo.AddPotionEvent += UpdatePotionUI;
            GameManager.I.Player.PlayerInfo.DeletePotionEvent -= UpdatePotionUI;
            GameManager.I.Player.PlayerInfo.DeletePotionEvent += UpdatePotionUI;
            
            GameManager.I.Player.PlayerInfo.UpdateGoldEvent -= UpdateMoneyUI;
            GameManager.I.Player.PlayerInfo.UpdateGoldEvent += UpdateMoneyUI;
        }

        private void OnDisable() {
            if (GameManager.I.Stage == null) return;
            if (GameManager.I.Stage.Player == null) return;
            
            GameManager.I.Player.PlayerInfo.AddBlessEvent -= UpdateBlessUI;
            GameManager.I.Player.PlayerInfo.AddArtifactEvent -= UpdateArtifactUI;
            GameManager.I.Player.PlayerInfo.AddPotionEvent -= UpdatePotionUI;
            GameManager.I.Player.PlayerInfo.DeletePotionEvent -= UpdatePotionUI;
            GameManager.I.Player.PlayerInfo.UpdateGoldEvent -= UpdateMoneyUI;
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
                if (b == BlessType.Empty) continue;
                
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
                if (a == null) continue;

                var artifactUIPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_PlayerDetail_Artifact);
                var artifactUIObj = Instantiate(artifactUIPrefab, _artifactListArea.Get(gameObject));
                var artifactUI = artifactUIObj.GetComponent<UIArtifact>();
                artifactUI.Init(a.Type);
                _artifactList.Add(artifactUI);
            }
        }

        private void InstantiatePotionUI() {
            ResetUIData(_potionList);

            for (int i = 0; i < Constants.GameSetting.Player.MaxPotionCapacity; i++) {
                var potionUIPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_PlayerDetail_Potion);
                var potionUIObj = Instantiate(potionUIPrefab, _potionListArea.Get(gameObject));
                var potionUI = potionUIObj.GetComponent<UIPotion>();
                potionUI.Init(i);
                _potionList.Add(potionUI);
            }
        }

        private void UpdatePotionUI(int index, Potion potion) {
            if (!_isPanelOpen) return;

            for (int i = 0; i < Constants.GameSetting.Player.MaxPotionCapacity; i++) {
                _potionList[i].Set(GameManager.I.Stage.Player.PlayerInfo.PotionList[i]);
            }
        }

        private void UpdateMoneyUI(int money) {
            if (!_isPanelOpen) return;

            _moneyText.Get(gameObject).text = GameManager.I.Player.PlayerInfo.Gold.ToString();
        }
    }

}