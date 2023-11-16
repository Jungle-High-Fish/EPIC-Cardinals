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
        
        [SerializeField] private Stage _stage1;
        public Stage CurStage { get; private set; }
        
        /// <summary>
        /// 외부에서 참조 가능한 현재 전투 중인 적
        /// </summary>
        public IEnumerable<BaseEnemy> CurrentEnemies { get; set; }
        public TempPlayer TempPlayer { get; set; } = new(); // 임시 
        public PlayerData PlayerData { get; set; } = new();// 임시

        #region Game

        [Button]
        public void GameStart()
        {
            StartCoroutine(MainGameFlow());
        }

        private IEnumerator MainGameFlow()
        {
            StageController stageController = LoadStage();
            yield return stageController.LoadStage(_stage1);
            yield return stageController.StageFlow();
        }
    
        private StageController LoadStage()
        {
            // Stage stage = null;
            // if (true) // 첫 데이타
            // {
            //     PlayerData.Turn = 0;
            //     stage = new Stage
            //         ("수상한 초원",
            //         new BattleEvent(EnemyType.One),
            //         new BattleEvent(EnemyType.Two),
            //         new BattleEvent(EnemyType.Three1, EnemyType.Three2),
            //         new BattleEvent(EnemyType.Four),
            //         new BattleEvent(EnemyType.Boss) );
                
            //     stage.Reset();
            // }
            // else // 기존 데이타 
            // {
                
            // }

            // return stage;
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
    }
}