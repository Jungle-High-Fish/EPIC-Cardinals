using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Cardinals.Game;
using Cardinals.Constants;
using Sirenix.OdinInspector;
using Unity.Burst.Intrinsics;
using Cardinals.Util;
using Cardinals.UI;
using UnityEngine.SceneManagement;

namespace Cardinals
{
    public class GameManager : Singleton<GameManager>
    {
        public UIManager UI => _ui;
        public StageController Stage => _stage;
        public GameSetting GameSetting => _gameSetting;
        public Localization Localization => _localization;
        public SteamHandler SteamHandler => _steamHandler;
        public SoundManager Sound => _soundManager;
        
        private GameSetting _gameSetting;
        private Localization _localization;
        private SteamHandler _steamHandler;

        private static UIManager _ui;
        [SerializeField] private List<Stage> _stageList;
        private static StageController _stage;
        
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
        public void GameStart()
        {
            StartCoroutine(LoadMainGame());
        }

        private void Start() {
            SteamHandlerInit();

            LoadGameSetting();
            GenerateCoreObjects();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (_ui.GameSettingUI.IsActive) {
                    _ui.GameSettingUI.Hide();
                } else {
                    _ui.GameSettingUI.Show();
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

        private void LoadGameSetting() {
            _localization = new Localization();

            _gameSetting = new GameSetting();
            _gameSetting.Init();
        }

        private void LoadSaveData() {
            
        }

        IEnumerator LoadMainGame() {
            var loading = SceneManager.LoadSceneAsync("StageTest");

            while (!loading.isDone) {
                yield return null;
            }
            
            StartCoroutine(MainGameFlow());
        }

        private IEnumerator MainGameFlow()
        {
            foreach (var stage in _stageList)
            {
                yield return StageFlow(stage);
            }
        }

        private IEnumerator StageFlow(Stage stage)
        {   
            GenerateCoreObjects();
            _stage = LoadStage();

            yield return _stage.Load(stage);
            yield return _stage.Flow();
        }
    
        private StageController LoadStage()
        {
            GameObject stageControllerObj = new GameObject("@" + Constants.Common.InstanceName.StageController);
            StageController stageController = stageControllerObj.AddComponent<StageController>();

            return stageController;
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
        }

        public void GameOver()
        {
            Debug.Log("게임 오버");
        }
        #endregion

        #region Util
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