using System;
using Cardinals.BoardEvent.Card;
using Cardinals.BoardEvent.Roulette;
using Cardinals.BoardEvent.Shop;
using Cardinals.Enemy;
using Cardinals.Game;
using Cardinals.UI;
using UnityEngine;
using Util;

namespace Cardinals
{
    public class UIManager : MonoBehaviour
    {
        public UIStage UIStage => _uiStage;
        public UIRewardPanel UIRewardPanel => _uiRewardPanel;
        public UIPlayerInfo UIPlayerInfo => _uiPlayerInfo;

        private Canvas _mainUICanvas;
        private Canvas _playerUICanvas;
        private Canvas _cardUICanvas;
        private Canvas _enemyUICanvas;

        private UIStage _uiStage;
        private UIRewardPanel _uiRewardPanel;
        private UIPlayerInfo _uiPlayerInfo;
        private UICardSystem _uiCardSystem;

        #region Board-Event 
        private UICardEvent _uiCardEvent;
        private UIShop _uiShop;
        private UIRoulette _uiRoulette;
        public UICardEvent UICardEvent => _uiCardEvent;
        public UIShop UIShop => _uiShop;
        public UIRoulette UIRoulette => _uiRoulette;
        #endregion
        
        public void Init() {
            InstantiateCanvas(Constants.Common.InstanceName.MainUICanvas, out _mainUICanvas);
            InstantiateCanvas(Constants.Common.InstanceName.PlayerUICanvas, out _playerUICanvas);
            InstantiateCanvas(Constants.Common.InstanceName.CardUICanvas, out _cardUICanvas);
            InstantiateCanvas(Constants.Common.InstanceName.EnemyUICanvas, out _enemyUICanvas);

            InstantiateStageInfoUI();
            InstantiateRewardUI();
            InstantiatePlayerUI();
            
            // Board Event
            InstantiateBoardEventCardUI();
            InstantiateBoardEventRouletteUI();
            InstantiateBoardEventShopUI();
        }

        public void InitPlayerUI() {
            _uiPlayerInfo.gameObject.SetActive(true);
            _uiPlayerInfo.Init();
        }

        public void SetEnemyUI(BaseEnemy enemyComp) {
            GameObject UIEnemyInfoPrefab 
                = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UIEnemyInfo);
            
            GameObject uiEnemyInfo = GameObject.Instantiate(UIEnemyInfoPrefab, _enemyUICanvas.transform);
            uiEnemyInfo.GetComponent<UIEnemyInfo>().Init(enemyComp);
        }

        public void SetCardSystemUI() {
            InstantiateCardSystemUI();
            _uiCardSystem.Init();
        }

        private void InstantiateCanvas(string name, out Canvas canvas) {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Canvas);
            canvas = Instantiate(prefab).GetComponent<Canvas>();
            canvas.name = name;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.worldCamera = Camera.main;
            canvas.transform.SetParent(transform);
        }

        private void InstantiateStageInfoUI() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_StageInfo);
            GameObject stageInfoUIObj = Instantiate(prefab, _mainUICanvas.transform);

            _uiStage = stageInfoUIObj.GetComponent<UIStage>();
            stageInfoUIObj.SetActive(false);
        }

        private void InstantiateRewardUI() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_RewardPanel);
            GameObject rewardUIObj = Instantiate(prefab, _mainUICanvas.transform);

            _uiRewardPanel = rewardUIObj.GetComponent<UIRewardPanel>();
            rewardUIObj.SetActive(false);
        }

        private void InstantiatePlayerUI() {
            GameObject playerUIPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UIPlayerInfo);
            GameObject playerUIObj = Instantiate(playerUIPrefab, _playerUICanvas.transform);

            _uiPlayerInfo = playerUIObj.GetComponent<UIPlayerInfo>();
            playerUIObj.SetActive(false);
        }

        private void InstantiateCardSystemUI() {
            GameObject cardSystemUIPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_CardSystem);
            GameObject cardSystemUIObj = Instantiate(cardSystemUIPrefab, _cardUICanvas.transform);
            _uiCardSystem = cardSystemUIObj.GetComponent<UICardSystem>();
        }

        private void InstantiateBoardEventCardUI() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_BoardEvent_Card);
            GameObject obj = Instantiate(prefab, _cardUICanvas.transform);
            _uiCardEvent = obj.GetComponent<UICardEvent>();
            obj.SetActive(false); 
        }
        private void InstantiateBoardEventRouletteUI() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_BoardEvent_Roulette);
            GameObject obj = Instantiate(prefab, _mainUICanvas.transform);
            _uiRoulette = obj.GetComponent<UIRoulette>();
            obj.SetActive(false); 
        }
        private void InstantiateBoardEventShopUI() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_BoardEvent_Shop);
            GameObject obj = Instantiate(prefab, _mainUICanvas.transform);
            _uiShop = obj.GetComponent<UIShop>();
            obj.SetActive(false); 
        }
    }
}