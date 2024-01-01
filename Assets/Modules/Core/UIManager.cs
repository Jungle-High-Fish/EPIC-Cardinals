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
using UnityEngine;
using Util;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Cardinals.Test;

namespace Cardinals
{
    public class UIManager : MonoBehaviour
    {
        public GameSettingUI GameSettingUI => _gameSettingUI;
        public UIPausePanel UIPausePanel => _uiPausePanel;
        public SaveFileLoaderPanel SaveFileLoaderPanel => _saveFileLoaderPanel;
        public UIPlayerInfo UIPlayerInfo => _uiPlayerInfo;
        public UIDebugPanel UIDebugPanel => _uiDebugPanel;

        public UIPlayerStatus UIPlayerStatus => _uiPlayerStatus;

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
        public UIStageEffect UIStageEffect => _uiStageEffect;
        public UITurnAlert UITurnAlert => _uiTurnAlert;
        public UINotiBoardEventByTurn UINotiBoardEventByTurn => _uiNotiBoardEventByTurn;
        
        public Canvas MainUICanvas => _mainUICanvas;
        public GameObject UIAntiTouchPanel => _uiAntiTouchPanel;
        public DescriptionArea DescCanvasDescArea { get; private set; }

        private Canvas _mainUICanvas;
        private Canvas _playerUICanvas;
        private Canvas _cardUICanvas;
        private Canvas _enemyUICanvas;
        private Canvas _systemUICanvas;
        private Canvas _descriptionUICanvas;

        private Dictionary<Canvas, UIBackground> _backgrounds
            = new Dictionary<Canvas, UIBackground>();

        private GameSettingUI _gameSettingUI;
        private UIPausePanel _uiPausePanel;
        private SaveFileLoaderPanel _saveFileLoaderPanel;
        private UIDebugPanel _uiDebugPanel;

        private UIPlayerStatus _uiPlayerStatus;
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
        private UIStageEffect _uiStageEffect;
        private UITurnAlert _uiTurnAlert;
        private GameObject _uiAntiTouchPanel;
        private UINotiBoardEventByTurn _uiNotiBoardEventByTurn;
        
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

        private const int MAIN_SORTING_ORDER = 4;
        private const int PLAYER_SORTING_ORDER = 1;
        private const int CARD_SORTING_ORDER = 3;
        private const int ENEMY_SORTING_ORDER = 2;
        private const int SYSTEM_SORTING_ORDER = 6;
        private const int DESCRIPTION_SORTING_ORDER = 5;
        
        public void Init() {
            InstantiateCanvas(
                Constants.Common.InstanceName.MainUICanvas,
                MAIN_SORTING_ORDER,
                out _mainUICanvas
            );
            InstantiateCanvas(
                Constants.Common.InstanceName.PlayerUICanvas,
                PLAYER_SORTING_ORDER,
                out _playerUICanvas
            );
            InstantiateCanvas(
                Constants.Common.InstanceName.CardUICanvas,
                CARD_SORTING_ORDER,
                out _cardUICanvas
            );
            InstantiateCanvas(
                Constants.Common.InstanceName.EnemyUICanvas,
                ENEMY_SORTING_ORDER,
                out _enemyUICanvas
            );
            InstantiateCanvas(
                Constants.Common.InstanceName.SystemUICanvas,
                SYSTEM_SORTING_ORDER,
                out _systemUICanvas
            );
            InitDescriptionCanvas();
            
            InstantiateSystemBubbleUI(_systemUICanvas);
            // Turn Alert
            InstantiateTurnAlertUI(_systemUICanvas);
            InstantiateUITurnEventNoti(_systemUICanvas);
            InstantiatePlayResultUI(_systemUICanvas);
            InstantiateClearDemoGameUI(_systemUICanvas);
            
            // Debug
            InstantiateDebugUI(_systemUICanvas);
            
            // StageInfo
            InstantiateStageInfoUI(_mainUICanvas);
            // Reward
            InstantiateRewardUI(_mainUICanvas);
            // Tile Selection
            InstantiateTileSelectionUI(_mainUICanvas);
            // Select New Dice
            InstantiateNewDicePanelUI(_mainUICanvas);
            // Tile Level Up
            InstantiateMagicLevelUpUI(_mainUICanvas);
            // Tutorial
            InstantiateTutorialUI(_mainUICanvas);
            // Mouse
            InstantiateMouseHintUI(_mainUICanvas);
            // Tile Info
            InstantiateHoveredTileInfoUI(_mainUICanvas);
            
            // Board Event
            InstantiateBoardEventDiceUI(_cardUICanvas);
            InstantiateBoardEventRouletteUI(_cardUICanvas);
            InstantiateBoardEventShopUI(_cardUICanvas);
            InstantiateBoardEventTileUI(_cardUICanvas);
            InstantiateAlchemyEventPanelUI(_cardUICanvas);
            
            InstantiateNewPlayerUI(_playerUICanvas);
            InstantiateUIStageEffect(_systemUICanvas);
            
            InstantiateUIAntiTouchPanel(_systemUICanvas);
            
            InstantiatePausePanelUI(_systemUICanvas);
            InstantiateGameSettingUI(_systemUICanvas);
            InstantiateSaveFileLoaderUI(_systemUICanvas);
        }

        void InitDescriptionCanvas()
        {
            InstantiateCanvas(
                Constants.Common.InstanceName.DescriptionUICanvas, 
                DESCRIPTION_SORTING_ORDER, 
                out _descriptionUICanvas
            );
            
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
            InstantiateCardSystemUI(_cardUICanvas);
            _uiCardSystem.Init();
        }

        public void ForceUpdateUI() {
            Canvas.ForceUpdateCanvases();
        }

        public UIBackground Background(Canvas canvas) {
            UIBackground background = _backgrounds[canvas];
            return background;
        }

        private void InstantiateCanvas(string name, int sortingOrder, out Canvas canvas) {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Canvas);
            canvas = Instantiate(prefab).GetComponent<Canvas>();
            canvas.name = name;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.worldCamera = Camera.main;
            canvas.transform.SetParent(transform);
            canvas.sortingOrder = sortingOrder;

            GameObject backgroundPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Background);
            GameObject backgroundObj = Instantiate(backgroundPrefab, canvas.transform);
            UIBackground background = backgroundObj.GetComponent<UIBackground>();
            background.Init();

            _backgrounds.Add(canvas, background);
        }

        private void InstantiatePausePanelUI(Canvas canvas) {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_PausePanel);
            GameObject pausePanelUIObj = Instantiate(prefab, canvas.transform);

            _uiPausePanel = pausePanelUIObj.GetComponent<UIPausePanel>();
            _uiPausePanel.Init();
            pausePanelUIObj.SetActive(false);
        }

        private void InstantiateGameSettingUI(Canvas canvas) {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_GameSetting);
            GameObject gameSettingUIObj = Instantiate(prefab, canvas.transform);

            _gameSettingUI = gameSettingUIObj.GetComponent<GameSettingUI>();
            gameSettingUIObj.SetActive(false);
        }

        private void InstantiateSaveFileLoaderUI(Canvas canvas) {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_SaveFileLoaderPanel);
            GameObject saveFileLoaderUIObj = Instantiate(prefab, canvas.transform);

            _saveFileLoaderPanel = saveFileLoaderUIObj.GetComponent<SaveFileLoaderPanel>();
            _saveFileLoaderPanel.Init();
            saveFileLoaderUIObj.SetActive(false);
        }

        private void InstantiateStageInfoUI(Canvas canvas) {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_StageInfo);
            GameObject stageInfoUIObj = Instantiate(prefab, canvas.transform);

            _uiStage = stageInfoUIObj.GetComponent<UIStage>();
            // stageInfoUIObj.SetActive(false);
        }

        private void InstantiateMapButtonUI(Canvas canvas) {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_MapButton);
            GameObject turnEndButtonUIObj = Instantiate(prefab, canvas.transform);

            _uiMapButton = turnEndButtonUIObj.GetComponent<UIMapButton>();
            _uiMapButton.Init();
            _uiMapButton.gameObject.SetActive(false);
        }
        
        private void InstantiateRewardUI(Canvas canvas) {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_RewardPanel);
            GameObject rewardUIObj = Instantiate(prefab, canvas.transform);

            _uiRewardPanel = rewardUIObj.GetComponent<UIRewardPanel>();
            rewardUIObj.SetActive(false);
        }

        private void InstantiatePlayerUI(Canvas canvas) {
            GameObject playerUIPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UIPlayerInfo);
            GameObject playerUIObj = Instantiate(playerUIPrefab, canvas.transform);

            _uiPlayerInfo = playerUIObj.GetComponent<UIPlayerInfo>();
            playerUIObj.SetActive(false);
        }

        private void InstantiateNewPlayerUI(Canvas canvas) {
            GameObject playerUIPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UINewPlayerInfo);
            GameObject playerUIObj = Instantiate(playerUIPrefab, canvas.transform);

            _uiNewPlayerInfo = playerUIObj.GetComponent<UINewPlayerInfo>();
            _uiNewPlayerInfo.Init();
            playerUIObj.SetActive(false);
        }

        private void InstantiateUIStageEffect(Canvas canvas) {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_StageEffect);
            GameObject obj = Instantiate(prefab, canvas.transform);

            _uiStageEffect = prefab.GetComponent<UIStageEffect>();
        }

        private void InstantiateUITurnEventNoti(Canvas canvas)
        {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_NotiBoradEventByTurn);
            GameObject obj = Instantiate(prefab, canvas.transform);

            _uiNotiBoardEventByTurn = obj.GetComponent<UINotiBoardEventByTurn>();
            _uiNotiBoardEventByTurn.Init();
        }
        
        private void InstantiateTurnAlertUI(Canvas canvas) {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_TurnAlert);
            GameObject obj = Instantiate(prefab, canvas.transform);

            _uiTurnAlert = obj.GetComponent<UITurnAlert>();
            obj.SetActive(false);
        }
        
        public void InstantiatePlayerStatusUI()
        {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UIPlayerStatus);
            GameObject obj = Instantiate(prefab, _playerUICanvas.transform);
            
            _uiPlayerStatus = obj.GetComponent<UIPlayerStatus>();
            _uiPlayerStatus.Init(GameManager.I.Player);
        }

        private void InstantiateCardSystemUI(Canvas canvas) {
            GameObject cardSystemUIPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_CardSystem);
            GameObject cardSystemUIObj = Instantiate(cardSystemUIPrefab, canvas.transform);
            _uiCardSystem = cardSystemUIObj.GetComponent<UICardSystem>();
        }

        private void InstantiateTileSelectionUI(Canvas canvas) {
            GameObject tileSelectionUIPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_TileSelection);
            GameObject tileSelectionUIObj = Instantiate(tileSelectionUIPrefab, canvas.transform);
            _uiTileSelection = tileSelectionUIObj.GetComponent<UITileSelection>();

            tileSelectionUIObj.SetActive(false);
        }

        private void InstantiateNewDicePanelUI(Canvas canvas)
        {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_NewDicePanel);
            GameObject obj = Instantiate(prefab, canvas.transform);
            _uiNewDicePanel = obj.GetComponent<UINewDicePanel>();
            obj.SetActive(false);
        }

        private void InstantiateBoardEventDiceUI(Canvas canvas) {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_BoardEvent_Dice);
            GameObject obj = Instantiate(prefab, canvas.transform);
            _uiDiceEvent = obj.GetComponent<UIDiceEvent>();
            obj.SetActive(false); 
        }
        
        private void InstantiateBoardEventRouletteUI(Canvas canvas) {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_BoardEvent_Roulette);
            GameObject obj = Instantiate(prefab, canvas.transform);
            _uiRoulette = obj.GetComponent<UIRoulette>();
            obj.SetActive(false); 
        }
        private void InstantiateBoardEventShopUI(Canvas canvas) {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_BoardEvent_Shop);
            GameObject obj = Instantiate(prefab, canvas.transform);
            _uiShop = obj.GetComponent<UIShop>();
            obj.SetActive(false); 
        }

        private void InstantiateBoardEventTileUI(Canvas canvas)
        {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_BoardEvent_Tile);
            GameObject obj = Instantiate(prefab, canvas.transform);
            _uiTileEvent = obj.GetComponent<UITileEvent>();
            obj.SetActive(false); 
        }

        private void InstantiateAlchemyEventPanelUI(Canvas canvas)
        {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_BoardEvent_AlchemyEvent);
            GameObject obj = Instantiate(prefab, canvas.transform);
            _uiAlchemyEventPanel = obj.GetComponent<UIAlchemyEventPanel>();
            obj.SetActive(false); 
        }

        private void InstantiateMagicLevelUpUI(Canvas canvas) {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_MagicLevelUpPanel);
            GameObject obj = Instantiate(prefab, canvas.transform);
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

        private void InstantiateHoveredTileInfoUI(Canvas canvas) {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_HoveredTileInfo);
            GameObject obj = Instantiate(prefab, canvas.transform);

            _uiHoveredTileInfo = obj.GetComponent<UITileInfo>();
            obj.SetActive(false);
        }
        
        private void InstantiateTutorialUI(Canvas canvas) {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Tutorial);
            GameObject obj = Instantiate(prefab, canvas.transform);

            _uiTutorial = obj.GetComponent<UITutorial>();
            obj.SetActive(false);
        }

        private void InstantiateMouseHintUI(Canvas canvas) {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_MouseHint);
            GameObject obj = Instantiate(prefab, canvas.transform);

            _uiMouseHint = obj.GetComponent<UIMouseHint>();
            obj.SetActive(false);
        }

        private void InstantiateSystemBubbleUI(Canvas canvas)
        {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Ending_SystemBubble);
            GameObject obj = Instantiate(prefab, canvas.transform);

            _uiSystemBubble = obj.GetComponent<Bubble>();
            obj.SetActive(false);
        }
        
        private void InstantiatePlayResultUI(Canvas canvas)
        {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Ending_PlayerResultPanel);
            GameObject obj = Instantiate(prefab, canvas.transform);

            _uiPlayerResultPanel = obj.GetComponent<UIPlayerResultPanel>();
            _uiPlayerResultPanel.Init();
            obj.SetActive(false);
        }
        
        private void InstantiateClearDemoGameUI(Canvas canvas)
        {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Ending_ClearDemoGamePanel);
            GameObject obj = Instantiate(prefab, canvas.transform);

            _uiClearDemoGame = obj.GetComponent<UIClearDemoGame>();
            _uiClearDemoGame.Init();
            obj.SetActive(false);
        }
        
        public void CanvasInactive(bool card = false, bool enemy = false, bool desc = false, bool main = false, bool player = false)
        {
            _cardUICanvas.gameObject.SetActive(card);
            _enemyUICanvas.gameObject.SetActive(enemy);
            _descriptionUICanvas.gameObject.SetActive(desc);
            _mainUICanvas.gameObject.SetActive(main);
            _playerUICanvas.gameObject.SetActive(player);
        }

        private void InstantiateUIAntiTouchPanel(Canvas canvas)
        {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_AntiTouchPanel);
            _uiAntiTouchPanel = Instantiate(prefab, canvas.transform);
            _uiAntiTouchPanel.SetActive(false);
        }

        // 디버깅 UI
        private void InstantiateDebugUI(Canvas canvas) {
            GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_DebugPanel);
            GameObject debugUIObj = Instantiate(prefab, canvas.transform);
            _uiDebugPanel = debugUIObj.GetComponent<UIDebugPanel>();
            _uiDebugPanel.Init();
            debugUIObj.SetActive(false);
        }
    }
}