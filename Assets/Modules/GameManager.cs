using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cardinals.Enemy;
using Cardinals.Enums;
using UnityEngine;
using Util;
using Cardinals.Game;
using Modules.Enemy;
using Sirenix.OdinInspector;

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
        
        public Stage CurStage { get; private set; }

        public TempPlayer TempPlayer { get; set; } = new(); // 임시 
        public PlayerData PlayerData { get; set; } = new();// 임시

        #region Game

        [Button]
        public void GameStart()
        {
            StartCoroutine(GameFlow());
        }
        private IEnumerator GameFlow()
        {
            // 스테이지 로드
            CurStage = LoadStage(); 
            UI.UIStage.Init(CurStage);
            yield return UI.UIStage.Visit();
            
            // 다음 사건을 읽음
            while (CurStage.MoveNext()) 
            {
                // 현재 사건에 따른 이벤트 플로우 수행
                using var evt = CurStage.Current as Game.BaseEvent;
                
                evt.On();
                
                IEnumerator evtFlow = evt.Type switch
                {
                    StageEventType.Battle => BattleFlow(evt as BattleEvent),
                    _ => throw new ArgumentOutOfRangeException()
                };
                
                yield return evtFlow;

                if (!evt.IsClear)
                {
                    GameOver();
                }
            }

            GameClear();
        }
    
        private Stage LoadStage()
        {
            Stage stage = null;
            if (true) // 첫 데이타
            {
                PlayerData.Turn = 0;
                stage = new Stage
                ("수상한 초원",
                 new BattleEvent(EnemyType.One),
                 new BattleEvent(EnemyType.Boss) );
                
                stage.Reset();
            }
            else // 기존 데이타 
            {
                
            }

            return stage;
        }

        private void GameClear()
        {
            Debug.Log("게임 클리어");
        }

        private void GameOver()
        {
            Debug.Log("게임 오버");
        }
        #endregion
        


        #region Battle

        [Header("임시 몬스터 프리팹")]
        [SerializeField] private Transform EnemyTr;
        [SerializeField] private GameObject UIEnemyPrefab;
        [SerializeField] private Transform EnemyInfoTr;
        [SerializeField] private GameObject UIEnemyInfoPrefab;
        IEnumerator BattleFlow(BattleEvent battleEvt)
        {
            Debug.Log("전투 시작");
            // 전투 세팅
        
            // 몬스터 설정
            List<BaseEnemy> enemies = new();
            var posArr = battleEvt.GetPositions();
            for (int i = 0, cnt = battleEvt.EnemyType.Length; i < cnt; i++)
            {
                var type = battleEvt.EnemyType[i];
                var enemy = EnemyFactory.GetEnemy(type);
                Instantiate(UIEnemyPrefab, EnemyTr).GetComponent<UIEnemy>().Init(enemy);
                Instantiate(UIEnemyInfoPrefab, EnemyInfoTr).GetComponent<UIEnemyInfo>().Init(enemy);
                
                enemy.DieEvent += () =>
                {
                    enemies.Remove(enemy);
                };
                
                enemies.Add(enemy);
            }
            
            // 초기화
            do // 전투 시작
            {
                // 전투 업데이트
                PlayerData.Turn++;
                TempPlayer.StartTurn();
                enemies.ForEach(enemy => enemy.StartTurn());

                // 플레이어 행동 초기화

                // 플레이어 행동
                TempPlayer.OnTurn();
                yield return null; // 대기?
                
                // 적 행동
                enemies.ForEach(enemy => enemy.OnTurn());
                
                // 카운트 처리
                TempPlayer.EndTurn();
                enemies.ForEach(enemy => enemy.EndTurn());
                
                yield return new WaitForSeconds(1f);
            } while (enemies.Count > 0 && TempPlayer.Hp > 0);

            if (enemies.Count == 0)
            {
                battleEvt.IsClear = true;
            }
        }
        #endregion
        
    }
}