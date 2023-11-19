using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Cardinals.Game;
using Cardinals.Constants;
using Sirenix.OdinInspector;
using Unity.Burst.Intrinsics;

namespace Cardinals
{
    public class GameManager : Singleton<GameManager>
    {
        private static UIManager _ui;
        public UIManager UI
        {
            get
            {
                _ui ??= FindObjectOfType<UIManager>();
                return _ui;
            }
        }
        
        [SerializeField] private List<Stage> _stageList;
        public StageController Stage { get; private set; }
        
        /// <summary>
        /// 외부에서 참조 가능한 현재 전투 중인 적
        /// </summary>
        public IEnumerable<BaseEnemy> CurrentEnemies { get; set; }

        public Player Player => Stage?.Player ?? null;

        #region Game

        [Button]
        public void GameStart()
        {
            StartCoroutine(MainGameFlow());
        }

        private void Start() {
            GameStart();
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
            StageController stageController = LoadStage();
            Stage = stageController;
            yield return stageController.Load(stage);
            yield return stageController.Flow();
        }
    
        private StageController LoadStage()
        {
            GameObject stageControllerObj = new GameObject("@" + Constants.Common.InstanceName.StageController);
            StageController stageController = stageControllerObj.AddComponent<StageController>();

            return stageController;
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