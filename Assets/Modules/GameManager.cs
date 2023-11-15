using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using UnityEngine;
using Util;
using Cardinals.Game;

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
        
        public PlayerData PlayerData { get; set; } // 임시

        #region Game
        private IEnumerator GameFlow()
        {
            // 스테이지 로드
            CurStage = LoadStage(); 
            UI.UIStage.Init(CurStage);
            UI.UIStage.Visit();
            
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
                // stage = new Stage();
            }
            else // 기존 데이타 
            {
                
            }

            return stage;
        }

        private void GameClear()
        {
            
        }

        private void GameOver()
        {
            
        }
        #endregion
        


        #region Battle
        IEnumerator BattleFlow(BattleEvent battleEvt)
        {
            // 전투 세팅
        
            // 몬스터 설정
            List<BaseEnemy> enemies = new();
            var posArr = battleEvt.GetPositions();
            for (int i = 0, cnt = enemies.Count; i < cnt; i++)
            {
                BaseEnemy enemy = null;// [TODO] battleEvt.EnemyType에 따른 인스턴스 생성 필요
                // [TODO] 화면에 생성
                
                enemy.Die += () =>
                {
                    // [TODO] 오브젝트 파괴 필요
                    enemies.Remove(enemy);
                };
                
                enemies.Add(enemy);
            }
            
            // 초기화

            do // 전투 시작
            {
                // 전투 업데이트
                PlayerData.Turn++;
                enemies.ForEach(enemy => enemy.StartTurn());

                // 플레이어 행동 초기화

                // 플레이어 행동
                yield return null; // 대기?
                
                // 적 행동
                enemies.ForEach(enemy => enemy.OnTurn());
                

                // 카운트 처리
                // [TODO] player.EndTurn();
                enemies.ForEach(enemy => enemy.EndTurn());
                
            } while (enemies.Count > 0); // [TODO] && Player == Alive
        }
        #endregion
        
    }
}