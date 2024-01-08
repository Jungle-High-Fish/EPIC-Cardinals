using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;
using Cardinals.Game;
using Cardinals.Constants;
using Sirenix.OdinInspector;
using Unity.Burst.Intrinsics;
using Cardinals.Util;
using Cardinals.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Resources = Cardinals.Constants.FilePath.Resources;
using Cardinals.Log;

namespace Cardinals
{
    public class GameManager : Singleton<GameManager>
    {
        public UIManager UI => _ui;
        public StageController Stage => _stage;
        public GameSetting GameSetting => _gameSetting;
        public Localization Localization => _localization;
        public SteamHandler SteamHandler => _steamHandler;
        public SaveSystem SaveSystem => _saveSystem;
        public SoundManager Sound => _soundManager;
        public LogManager LogManager => _logManager;

        public int CurrentStageIndex => _currentStageIndex;
        public List<Stage> RuntimeStageList => _stageRuntimeList;
        
        private GameSetting _gameSetting;
        private Localization _localization;
        private LogManager _logManager;
        private SteamHandler _steamHandler;
        private SaveSystem _saveSystem;

        private static UIManager _ui;
        [SerializeField] private List<Stage> _stageList;
        private List<Stage> _stageRuntimeList;
        private static StageController _stage;
        private int _currentStageIndex = 0;
        
        /// <summary>
        /// 외부에서 참조 가능한 현재 전투 중인 적
        /// TODO: StageController에서 접근하도록 수정
        /// </summary>
        public IEnumerable<BaseEnemy> CurrentEnemies { get; set; }
        public Player Player => Stage?.Player ?? null;

        private ComponentGetter<CameraController> _cameraController
            = new ComponentGetter<CameraController>(TypeOfGetter.Global);
        public CameraController CameraController => _cameraController.Get();

        private ComponentGetter<LightController> _lightController
            = new ComponentGetter<LightController>(TypeOfGetter.Global);
        public LightController LightController => _lightController.Get();

        private SoundManager _soundManager;

        #region Game

        [Button]
        private void DeleteAllPlayerPrefs() {
            PlayerPrefs.DeleteAll();
        }

        [Button]
        public void GameStart(SaveFileData saveFileData=null, bool skipTutorial=false)
        {
            StartCoroutine(LoadMainGame(saveFileData, skipTutorial));
        }

        public void GoToTitle() {
            GameObject sceneChanger = new GameObject("@SceneChanger");
            sceneChanger.AddComponent<SceneChanger>().ChangeScene("Title", this);
        }

        public void GoToCredit()
        {
            GameObject sceneChanger = new GameObject("@SceneChanger");
            sceneChanger.AddComponent<SceneChanger>().ChangeSceneCredit("Title", this);
        }

        private void Start() {
            SteamHandlerInit();
            SaveSystemInit();
            LogManagerInit();

            LoadGameSetting();
            GenerateCoreObjects();

            _steamHandler.SetLobbyStateDisplay();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                TitleManager titleManager = FindAnyObjectByType<TitleManager>();
                if (titleManager != null) {
                    if (GameManager.I.UI.SaveFileLoaderPanel.IsActive) {
                        GameManager.I.UI.SaveFileLoaderPanel.Hide();
                    } else if (_ui.GameSettingUI.IsActive) {
                        _ui.GameSettingUI.Hide();
                    } else {
                        _ui.GameSettingUI.Show();
                    }
                } else {
                    if (_ui.GameSettingUI.IsActive) {
                        _ui.GameSettingUI.Hide();
                    } else if (_ui.UIPausePanel.IsActive) {
                        _ui.UIPausePanel.Hide();
                    } else {
                        _ui.UIPausePanel.Show();
                    }
                }
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F3)) {
                if (_ui.UIDebugPanel.IsShow) {
                    _ui.UIDebugPanel.Hide();
                } else {
                    _ui.UIDebugPanel.Show();
                }
            }

            _steamHandler.EveryFrame();
        }

        private void OnApplicationQuit() {
            Steamworks.SteamClient.Shutdown();
        }

        private void SteamHandlerInit() {
            _steamHandler = new SteamHandler();
            _steamHandler.Init();
        }

        private void SaveSystemInit() {
            _saveSystem = new SaveSystem();
        }

        private void LogManagerInit() {
            _logManager = new LogManager();
            _logManager.Init();
        }

        private void LoadGameSetting() {
            _localization = new Localization();

            _gameSetting = new GameSetting();
            _gameSetting.Init();
        }

        private void LoadFromSaveData(SaveFileData saveFileData) {
            _stageRuntimeList = new List<Stage>();
            for (int i = 0; i < _stageList.Count; i++) {
                var stage = _stageList[i];
                stage.Init(
                    i == saveFileData.CurrentStageIndex ? saveFileData.ClearedStageEventIndex : -1, 
                    saveFileData.StageEventSequence[i].StageEventList
                );
                _stageRuntimeList.Add(stage);
            }

            _currentStageIndex = saveFileData.CurrentStageIndex;

            // 게임 누적 데이타
            TurnCount = saveFileData.TurnCount;
            DiceRollingCount = saveFileData.DiceRollingCount;
            ExecuteEnemyCount = saveFileData.ExecuteEnemyCount;
            PlayTime = saveFileData.PlayTime;
        }

        private void MakeNewGameData(bool skipTutorial) {
            _saveSystem.ClearCurrentSaveFileData();

            _stageRuntimeList = new List<Stage>();
            for (int i = 0; i < _stageList.Count; i++) {
                var stage = _stageList[i];
                stage.Init(-1, skipTutorial: skipTutorial);
                _stageRuntimeList.Add(stage);
            }

            _currentStageIndex = 0;
        }

        IEnumerator LoadMainGame(SaveFileData saveFileData=null, bool skipTutorial=false) {
            var titleManager = FindAnyObjectByType<TitleManager>();
            UILoading loadingUI = null;
            if (titleManager != null) {
                loadingUI = titleManager.LoadingUI;
            }
            yield return loadingUI?.Show();

            var loading = SceneManager.LoadSceneAsync("StageTest");

            while (!loading.isDone) {
                yield return null;
            }

            if (saveFileData == null) {
                MakeNewGameData(skipTutorial);
            } else {
                LoadFromSaveData(saveFileData);
            }

            yield return new WaitForSeconds(1.5f);
            StartCoroutine(MainGameFlow(loadingUI));
        }

        private IEnumerator MainGameFlow(UILoading loadingUI)
        {
            UILoading loading = loadingUI;

            // 업적 관련 구독
            timeRecordCoroutine = TimeRecord();
            StartCoroutine(timeRecordCoroutine);
            GenerateCoreObjects();

            bool isInitialStage = true;
            for (int i = _currentStageIndex; i < _stageRuntimeList.Count; i++)
            {
                _currentStageIndex = i;
                yield return StageFlow(_stageRuntimeList[i], loading, isInitialStage);
                loading = null;
                isInitialStage = false;
            }
        }

        private IEnumerator StageFlow(Stage stage, UILoading loadingUI, bool isInitialStage)
        {   
            _stage = LoadStageController();

            yield return _stage.Load(stage, loadingUI, isInitialStage);
            yield return _stage.Flow();

            DestoryStageController();
        }
    
        private StageController LoadStageController()
        {
            GameObject stageControllerObj = new GameObject("@" + Constants.Common.InstanceName.StageController);
            StageController stageController = stageControllerObj.AddComponent<StageController>();

            return stageController;
        }

        private void DestoryStageController()
        {
            _stage.DestroyBaseObjs();
            Destroy(_stage.gameObject);
        }

        private void GenerateCoreObjects() {
            _ui = InitUI();
            _soundManager = InitSound();
            _soundManager.PlayBGM();
            _gameSetting.InitUI(_ui.GameSettingUI);
        }

        private UIManager InitUI() {
            GameObject stageUIObj = new GameObject("@" + Constants.Common.InstanceName.UI, typeof(RectTransform));
            _ui = stageUIObj.AddComponent<UIManager>();

            _ui.Init();
            return _ui;
        }

        private SoundManager InitSound() {
            GameObject soundManagerPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_SoundManager);
            var soundManagerObj = Instantiate(soundManagerPrefab);
            soundManagerObj.name = "@" + Constants.Common.InstanceName.SoundManager;

            var soundManager = soundManagerObj.GetComponent<SoundManager>();
            return soundManager;
        }

        public void GameClear()
        {
            Debug.Log("게임 클리어");
            GameEnd();
        }

        public IEnumerator GameOver()
        {
            GameEnd();
            yield return GameOverFlow();
        }

        IEnumerator GameOverFlow()
        {
            bool next = false;
            
            UI.CanvasInactive(); // UI 닫기
            CurrentEnemies.FirstOrDefault()?.gameObject.SetActive(false); // 몬스터 disable
            Player.SetGameOver(); // 플레이어 처리
            Player.transform.SetParent(GameObject.Find("MapObject").transform);
            
            // 카메라 줌인
            yield return CameraController.PlayerZoomIn();
            
            // 배경 검정색으로 
            var prefab = ResourceLoader.LoadPrefab(Resources.Prefabs_UI_Ending_FadeInBlack);
            var obj = Instantiate(prefab);
            var renderer = obj.GetComponent<SpriteRenderer>();
            renderer.color = new Color(0, 0, 0, 0);
            renderer.DOFade(1, 1f).SetEase(Ease.Linear)
                .OnComplete(() => { next = true;});
            yield return new WaitUntil(() => next);
            
            // 카메라 이동
            next = false;
            Player.transform.DOMove(Player.transform .position+ new Vector3(-1.2f, .6f, -1.2f), 1.5f)
                .OnComplete(() => { next = true;});
            yield return new WaitUntil(() => next);
            
            // 텍스트 출력
            UI.UISystemBubble.SetBubble(GameManager.I.Localization.Get(LocalizationEnum.PLAYER_SCRIPT_GAMEOVER), -1);
            (UI.UISystemBubble.transform as RectTransform).position = Camera.main.WorldToScreenPoint(Player.transform.position) + new Vector3(0, 150, 0);
            yield return new WaitForSeconds(1.5f);
            
            // 결과 창 출력
            yield return UI.UIPlayerResultPanel.Set(TurnCount, DiceRollingCount, ExecuteEnemyCount, PlayTime);
        }

        public void GameEnd()
        {
            StopCoroutine(timeRecordCoroutine);   
        }

        public void Retry()
        {
            
        }

        #endregion

        #region Game Play Info (업적 카운트)
        public int TurnCount;
        public int DiceRollingCount;
        public int ExecuteEnemyCount;
        public ulong PlayTime;

        private IEnumerator timeRecordCoroutine;
        IEnumerator TimeRecord()
        {
            while (true)
            {
                PlayTime++;
                yield return new WaitForSeconds(1f);
            }
        }
        
        #endregion

        #region Util
        public bool IsWaitingForNext => !_next;
        private bool _next = true;

        public IEnumerator WaitNext() {
            _next = false;
            yield return new WaitUntil(() => _next);
        }

        public void Next() {
            _next = true;
        }
        #endregion
    }
}