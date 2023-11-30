using System;
using Cardinals.BoardEvent.Card;
using Cardinals.BoardEvent.Roulette;
using Cardinals.BoardEvent.Shop;
using Cardinals.BoardEvent.Tile;
using Cardinals.Enemy;
using Cardinals.Game;
using Cardinals.UI;
using Unity.VisualScripting;
using UnityEngine;
using Util;

namespace Cardinals
{
    public class UIManager : MonoBehaviour
    {
        public UIStage UIStage => _uiStage;
        public UIRewardPanel UIRewardPanel => _uiRewardPanel;
        public UIPlayerInfo UIPlayerInfo => _uiPlayerInfo;
        public UIEndTurnButton UIEndTurnButton => _uiEndTurnButton;
        public UICardSystem UICardSystem => _uiCardSystem;
        public UITileSelection UITileSelection => _uiTileSelection;
        public UIMagicLevelUpPanel UIMagicLevelUpPanel => _uiMagicLevelUpPanel;
        public UIMapButton UIMapButton => _uiMapButton;
        public UITileInfo UIHoveredTileInfo => _uiHoveredTileInfo;
        public UITileInfo UITileInfo => _uiTileInfo;

        public Canvas MainUICanvas => _mainUICanvas;
        public DescriptionArea DescCanvasDescArea { get; private set; }

        private Canvas _mainUICanvas;
        private Canvas _playerUICanvas;
        private Canvas _cardUICanvas;
        private Canvas _enemyUICanvas;
        private Canvas _systemUICanvas;
        private Canvas _descriptionUICanvas;

        private UIStage _uiStage;
        private UIRewardPanel _uiRewardPanel;
        private UIEndTurnButton _uiEndTurnButton;
        private UIMapButton _uiMapButton;
        private UIPlayerInfo _uiPlayerInfo;
        private UICardSystem _uiCardSystem;
        private UITileSelection _uiTileSelection;
        private UIMagicLevelUpPanel _uiMagicLevelUpPanel;
        private UITileInfo _uiHoveredTileInfo;
        private UITileInfo _uiTileInfo;

        #region Board-Event 
        private UICardEvent _uiCardEvent;
        private UIShop _uiShop;
        private UIRoulette _uiRoulette;
        private UITileEvent _uiTileEvent;
        public UICardEvent UICardEvent => _uiCardEvent;
        public UIShop UIShop => _uiShop;
        public UIRoulette UIRoulette => _uiRoulette;
        public UITileEvent UITileEvent => _uiTileEvent;
        #endregion
        
        public TEMP_UIStageMap TEMP_UIStageMap { get; private set; }
        
        public void Init() {
            InstantiateCanvas(Constants.Common.InstanceName.MainUICanvas, out _mainUICanvas);
            InstantiateCanvas(Constants.Common.InstanceName.PlayerUICanvas, out _playerUICanvas);
            InstantiateCanvas(Constants.Common.InstanceName.CardUICanvas, out _cardUICanvas);
            InstantiateCanvas(Constants.Common.InstanceName.EnemyUICanvas, out _enemyUICanvas);
            InstantiateCanvas(Constants.Common.InstanceName.SystemUICanvas, out _systemUICanvas);
            _systemUICanvas.GetComponent<Canvas>().sortingOrder = 1;
            
            // 설명 창 캔버스
            DescCanvasDescArea = FindObjectOfType<DescriptionArea>();
            
            
            InstantiateStageInfoUI();
            InstantiateTurnEndButtonUI();
            InstantiateMapButtonUI();
            InstantiateRewardUI();
            InstantiatePlayerUI();
            InstantiateTileSelectionUI();

            InstantiateEnemyUIParent();
            
            // TEMP
            TEMP_InstantiateUIMap();
            
            // Board Event
            InstantiateBoardEventCardUI();
            InstantiateBoardEventRouletteUI();
            InstantiateBoardEventShopUI();
            InstantiateBoardEventTileUI();

            // Tile Level Up
            InstantiateMagicLevelUpUI();
            
            InstantiateHoveredTileInfoUI();
            InstantiateTileInfoUI();
        }

        public void InitPlayerUI() {
            _uiPlayerInfo.gameObject.SetActive(true);
            _uiPlayerInfo.Init();
        }

        public void SetEnemyUI(BaseEnemy enemyComp) {
            GameObject UIEnemyInfoPrefab 
                = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UIEnemyInfo);
            
            GameObject uiEnemyInfo = GameObject.Instantiate(UIEnemyInfoPrefab, GameManager.I.Stage.EnemyInfoController.transform);
            uiEnemyInfo.GetComponent<UIEnemyInfo>().Init(enemyComp);
        }

        public void SetCardSystemUI() {
            InstantiateCardSystemUI();
            _uiCardSystem.Init();
        }

        public void ForceUpdatUI() {
            Canvas.ForceUpdateCanvases();
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

        private void InstantiateTurnEndButtonUI() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_TurnEndButton);
            GameObject turnEndButtonUIObj = Instantiate(prefab, _mainUICanvas.transform);

            _uiEndTurnButton = turnEndButtonUIObj.GetComponent<UIEndTurnButton>();
            _uiEndTurnButton.Init();
            turnEndButtonUIObj.SetActive(false);
        }

        private void InstantiateMapButtonUI() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_MapButton);
            GameObject turnEndButtonUIObj = Instantiate(prefab, _mainUICanvas.transform);

            _uiMapButton = turnEndButtonUIObj.GetComponent<UIMapButton>();
            _uiMapButton.Init();
            _uiMapButton.gameObject.SetActive(false);
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

        private void InstantiateTileSelectionUI() {
            GameObject tileSelectionUIPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_TileSelection);
            GameObject tileSelectionUIObj = Instantiate(tileSelectionUIPrefab, _mainUICanvas.transform);
            _uiTileSelection = tileSelectionUIObj.GetComponent<UITileSelection>();

            tileSelectionUIObj.SetActive(false);
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

        private void InstantiateBoardEventTileUI()
        {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_BoardEvent_Tile);
            GameObject obj = Instantiate(prefab, _mainUICanvas.transform);
            _uiTileEvent = obj.GetComponent<UITileEvent>();
            obj.SetActive(false); 
        }

        private void InstantiateMagicLevelUpUI() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_MagicLevelUpPanel);
            GameObject obj = Instantiate(prefab, _mainUICanvas.transform);
            _uiMagicLevelUpPanel = obj.GetComponent<UIMagicLevelUpPanel>();
            obj.SetActive(false);
        }
        
        
        private void TEMP_InstantiateUIMap() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Stage_Map);
            GameObject obj = Instantiate(prefab, _systemUICanvas.transform);
            TEMP_UIStageMap = obj.GetComponent<TEMP_UIStageMap>();
            obj.SetActive(false); 
        }

        private void InstantiateEnemyUIParent()
        {
            var obj = new GameObject();
            obj.transform.SetParent(_enemyUICanvas.transform);

            obj.name = "@EnemyInfoController";
            obj.AddComponent<EnemyInfoController>();
        }

        private void InstantiateHoveredTileInfoUI() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_HoveredTileInfo);
            GameObject obj = Instantiate(prefab, _mainUICanvas.transform);

            _uiHoveredTileInfo = obj.GetComponent<UITileInfo>();
            obj.SetActive(false);
        }

        private void InstantiateTileInfoUI() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_TileInfo);
            GameObject obj = Instantiate(prefab, _mainUICanvas.transform);

            _uiTileInfo = obj.GetComponent<UITileInfo>();
            obj.SetActive(false);
        }
    }
}