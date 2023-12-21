using System;
using Cardinals.BoardEvent.Alchemy;
using Cardinals.BoardEvent.Card;
using Cardinals.BoardEvent.Roulette;
using Cardinals.BoardEvent.Shop;
using Cardinals.BoardEvent.Tile;
using Cardinals.Enemy;
using Cardinals.Entity.UI;
using Cardinals.Game;
using Cardinals.UI;
using Cardinals.UI.Description;
using Cardinals.Tutorial;
using Cardinals.UI.NewDice;
using Unity.VisualScripting;
using UnityEngine;
using Util;

namespace Cardinals
{
    public class UIManager : MonoBehaviour
    {
        public GameSettingUI GameSettingUI => _gameSettingUI;
        public UIPausePanel UIPausePanel => _uiPausePanel;
        public SaveFileLoaderPanel SaveFileLoaderPanel => _saveFileLoaderPanel;

        public UIStage UIStage => _uiStage;
        public UIRewardPanel UIRewardPanel => _uiRewardPanel;
        public UICardSystem UICardSystem => _uiCardSystem;
        public UITileSelection UITileSelection => _uiTileSelection;
        public UIMagicLevelUpPanel UIMagicLevelUpPanel => _uiMagicLevelUpPanel;
        public UITileInfo UIHoveredTileInfo => _uiHoveredTileInfo;
        public UINewPlayerInfo UINewPlayerInfo => _uiNewPlayerInfo;
        public UIEndTurnButton UIEndTurnButton => _uiNewPlayerInfo.EndTurnButton;
        public UINewDicePanel UINewDicePanel => _uiNewDicePanel;
        public Bubble UISystemBubble => _uiSystemBubble;
        public UIPlayerResultPanel UIPlayerResultPanel => _uiPlayerResultPanel;
        public UIClearDemoGame UIClearDemoGame => _uiClearDemoGame;
        
        public Canvas MainUICanvas => _mainUICanvas;
        public DescriptionArea DescCanvasDescArea { get; private set; }

        private Canvas _mainUICanvas;
        private Canvas _playerUICanvas;
        private Canvas _cardUICanvas;
        private Canvas _enemyUICanvas;
        private Canvas _systemUICanvas;
        private Canvas _descriptionUICanvas;

        private GameSettingUI _gameSettingUI;
        private UIPausePanel _uiPausePanel;
        private SaveFileLoaderPanel _saveFileLoaderPanel;

        private UIStage _uiStage;
        private UIRewardPanel _uiRewardPanel;
        private UIMapButton _uiMapButton;
        private UIPlayerInfo _uiPlayerInfo;
        private UICardSystem _uiCardSystem;
        private UITileSelection _uiTileSelection;
        private UIMagicLevelUpPanel _uiMagicLevelUpPanel;
        private UITileInfo _uiHoveredTileInfo;
        private UINewPlayerInfo _uiNewPlayerInfo;
        private UINewDicePanel _uiNewDicePanel;
        private UIPlayerResultPanel _uiPlayerResultPanel;
        private UIClearDemoGame _uiClearDemoGame;
        
        private Bubble _uiSystemBubble;

        #region Board-Event 
        private UIDiceEvent _uiDiceEvent;
        private UIShop _uiShop;
        private UIRoulette _uiRoulette;
        private UITileEvent _uiTileEvent;
        private UIAlchemyEventPanel _uiAlchemyEventPanel;
        public UIDiceEvent UIDiceEvent => _uiDiceEvent;
        public UIShop UIShop => _uiShop;
        public UIRoulette UIRoulette => _uiRoulette;
        public UITileEvent UITileEvent => _uiTileEvent;
        public UIAlchemyEventPanel UIAlchemyEventPanel => _uiAlchemyEventPanel;
        #endregion

        #region Tutorial
        private UITutorial _uiTutorial;

        public UITutorial UITutorial => _uiTutorial;
        #endregion

        #region Mouse
        private UIMouseHint _uiMouseHint;
        public UIMouseHint UIMouseHint => _uiMouseHint;
        #endregion
        
        public TEMP_UIStageMap TEMP_UIStageMap { get; private set; }
        public UIStageMap UIStageMap { get; private set; }
        
        public void Init() {
            InstantiateCanvas(Constants.Common.InstanceName.MainUICanvas, out _mainUICanvas);
            InstantiateCanvas(Constants.Common.InstanceName.PlayerUICanvas, out _playerUICanvas);
            InstantiateCanvas(Constants.Common.InstanceName.CardUICanvas, out _cardUICanvas);
            InstantiateCanvas(Constants.Common.InstanceName.EnemyUICanvas, out _enemyUICanvas);
            InstantiateCanvas(Constants.Common.InstanceName.SystemUICanvas, out _systemUICanvas);
            _systemUICanvas.GetComponent<Canvas>().sortingOrder = 1;
            InitDescriptionCanvas();
            
            InstantiatePausePanelUI();
            InstantiateGameSettingUI();
            InstantiateSaveFileLoaderUI();
            
            InstantiateStageInfoUI();
            //InstantiateMapButtonUI();
            InstantiateRewardUI();
            //InstantiatePlayerUI();
            InstantiateNewPlayerUI();
            InstantiateTileSelectionUI();
            InstantiateNewDicePanelUI();

            //InstantiateEnemyUIParent();
            
            
            // Board Event
            InstantiateBoardEventDiceUI();
            InstantiateBoardEventRouletteUI();
            InstantiateBoardEventShopUI();
            InstantiateBoardEventTileUI();
            InstantiateAlchemyEventPanelUI();
            

            // Tile Level Up
            InstantiateMagicLevelUpUI();
            
            InstantiateHoveredTileInfoUI();

            // Tutorial
            InstantiateTutorialUI();

            // Mouse
            InstantiateMouseHintUI();

            InstantiateSystemBubbleUI();
            InstantiatePlayResultUI();
            InstantiateClearDemoGameUI();
        }

        void InitDescriptionCanvas()
        {
            InstantiateCanvas(Constants.Common.InstanceName.DescriptionUICanvas, out _descriptionUICanvas);
            _descriptionUICanvas.sortingOrder = 1;
            
            var obj = new GameObject();
            obj.transform.SetParent(_descriptionUICanvas.transform);
            
            DescCanvasDescArea = obj.AddComponent<DescriptionArea>();
            DescCanvasDescArea.InitCanvas();
        }

        public void InitPlayerUI() {
            _uiPlayerInfo.gameObject.SetActive(true);
            _uiPlayerInfo.Init();
        }

        public void SetEnemyUI(BaseEnemy enemyComp) {
            GameObject UIEnemyInfoPrefab 
                = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UIEnemyStatus);
            
            GameObject uiEnemyInfo = GameObject.Instantiate(UIEnemyInfoPrefab, _enemyUICanvas.transform);
            uiEnemyInfo.GetComponent<UIEnemyStatus>().Init(enemyComp);
        }

        public void SetCardSystemUI() {
            InstantiateCardSystemUI();
            _uiCardSystem.Init();
        }

        public void ForceUpdateUI() {
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

        private void InstantiatePausePanelUI() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_PausePanel);
            GameObject pausePanelUIObj = Instantiate(prefab, _systemUICanvas.transform);

            _uiPausePanel = pausePanelUIObj.GetComponent<UIPausePanel>();
            _uiPausePanel.Init();
            pausePanelUIObj.SetActive(false);
        }

        private void InstantiateGameSettingUI() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_GameSetting);
            GameObject gameSettingUIObj = Instantiate(prefab, _systemUICanvas.transform);

            _gameSettingUI = gameSettingUIObj.GetComponent<GameSettingUI>();
            gameSettingUIObj.SetActive(false);
        }

        private void InstantiateSaveFileLoaderUI() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_SaveFileLoaderPanel);
            GameObject saveFileLoaderUIObj = Instantiate(prefab, _systemUICanvas.transform);

            _saveFileLoaderPanel = saveFileLoaderUIObj.GetComponent<SaveFileLoaderPanel>();
            _saveFileLoaderPanel.Init();
            saveFileLoaderUIObj.SetActive(false);
        }

        private void InstantiateStageInfoUI() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_StageInfo);
            GameObject stageInfoUIObj = Instantiate(prefab, _mainUICanvas.transform);

            _uiStage = stageInfoUIObj.GetComponent<UIStage>();
            // stageInfoUIObj.SetActive(false);
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

        private void InstantiateNewPlayerUI() {
            GameObject playerUIPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UINewPlayerInfo);
            GameObject playerUIObj = Instantiate(playerUIPrefab, _playerUICanvas.transform);

            _uiNewPlayerInfo = playerUIObj.GetComponent<UINewPlayerInfo>();
            _uiNewPlayerInfo.Init();
            playerUIObj.SetActive(false);
        }

        public void InstantiatePlayerStatusUI()
        {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UIPlayerStatus);
            GameObject obj = Instantiate(prefab, _playerUICanvas.transform);
            obj.GetComponent<UIPlayerStatus>().Init(GameManager.I.Player);
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

        private void InstantiateNewDicePanelUI()
        {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_NewDicePanel);
            GameObject obj = Instantiate(prefab, _mainUICanvas.transform);
            _uiNewDicePanel = obj.GetComponent<UINewDicePanel>();
            obj.SetActive(false);
        }

        private void InstantiateBoardEventDiceUI() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_BoardEvent_Dice);
            GameObject obj = Instantiate(prefab, _cardUICanvas.transform);
            _uiDiceEvent = obj.GetComponent<UIDiceEvent>();
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

        private void InstantiateAlchemyEventPanelUI()
        {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_BoardEvent_AlchemyEvent);
            GameObject obj = Instantiate(prefab, _systemUICanvas.transform);
            _uiAlchemyEventPanel = obj.GetComponent<UIAlchemyEventPanel>();
            obj.SetActive(false); 
        }

        private void InstantiateMagicLevelUpUI() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_MagicLevelUpPanel);
            GameObject obj = Instantiate(prefab, _mainUICanvas.transform);
            _uiMagicLevelUpPanel = obj.GetComponent<UIMagicLevelUpPanel>();
            obj.SetActive(false);
        }

        // private void InstantiateEnemyUIParent()
        // {
        //     var obj = new GameObject();
        //     obj.transform.SetParent(_enemyUICanvas.transform);
        //
        //     obj.name = "@EnemyInfoController";
        //     obj.AddComponent<EnemyInfoController>();
        // }

        private void InstantiateHoveredTileInfoUI() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_HoveredTileInfo);
            GameObject obj = Instantiate(prefab, _mainUICanvas.transform);

            _uiHoveredTileInfo = obj.GetComponent<UITileInfo>();
            obj.SetActive(false);
        }
        
        private void InstantiateTutorialUI() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Tutorial);
            GameObject obj = Instantiate(prefab, _mainUICanvas.transform);

            _uiTutorial = obj.GetComponent<UITutorial>();
            obj.SetActive(false);
        }

        private void InstantiateMouseHintUI() {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_MouseHint);
            GameObject obj = Instantiate(prefab, _mainUICanvas.transform);

            _uiMouseHint = obj.GetComponent<UIMouseHint>();
            obj.SetActive(false);
        }

        private void InstantiateSystemBubbleUI()
        {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Ending_SystemBubble);
            GameObject obj = Instantiate(prefab, _systemUICanvas.transform);

            _uiSystemBubble = obj.GetComponent<Bubble>();
            obj.SetActive(false);
        }
        
        private void InstantiatePlayResultUI()
        {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Ending_PlayerResultPanel);
            GameObject obj = Instantiate(prefab, _systemUICanvas.transform);

            _uiPlayerResultPanel = obj.GetComponent<UIPlayerResultPanel>();
            _uiPlayerResultPanel.Init();
            obj.SetActive(false);
        }
        
        private void InstantiateClearDemoGameUI()
        {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Ending_ClearDemoGamePanel);
            GameObject obj = Instantiate(prefab, _systemUICanvas.transform);

            _uiClearDemoGame = obj.GetComponent<UIClearDemoGame>();
            _uiClearDemoGame.Init();
            obj.SetActive(false);
        }
        
        public void CanvasInactive()
        {
            _cardUICanvas.gameObject.SetActive(false);
            _enemyUICanvas.gameObject.SetActive(false);
            _descriptionUICanvas.gameObject.SetActive(false);
            _mainUICanvas.gameObject.SetActive(false);
            _playerUICanvas.gameObject.SetActive(false);
        }
    }
}