using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Cardinals.Game;
using EventType = Cardinals.Game.EventType;

namespace Cardinals
{
    public class GameManager : Singleton<GameManager>
    {
        
        private static UIManager _ui;

        public UIManager UI
        {
            get
            {
                if (_ui == null)
                {
                    //_ui = I.GetComponent<UIManager>()
                }

                return _ui;
            }
        }
        
        private Stage _curStage;
        
        public Stage CurStage
        {
            get => _curStage;
            set
            {
                _curStage = value;
                // UI Update
            }
        }

        #region Game
        private IEnumerator GameFlow()
        {
            // 스테이지 로드
            CurStage = LoadStage();
            
            // 다음 사건을 읽음
            while (CurStage.MoveNext())
            {
                // 현재 사건에 따른 이벤트 플로우 수행
                using var evt = CurStage.Current as Game.BaseEvent;
                evt.On();
                
                IEnumerator evtFlow = evt.Type switch
                {
                    EventType.Battle => BattleFlow(evt as BattleEvent),
                    _ => throw new ArgumentOutOfRangeException()
                };
                
                yield return evtFlow;

                if (!evt.IsClear)
                {
                    // 게임 오버
                }
            }
            
            // 게임 클리어
        }
    
        private Stage LoadStage()
        {
            Stage stage = null;
            if (true) // 첫 데이타
            {
                // stage = new Stage();
            }
            else // 기존 데이타 
            {
                
            }

            return stage;
        }
        #endregion
        


        #region Battle
        IEnumerator BattleFlow(BattleEvent battleEvt)
        {
            // 전투 세팅
        
            // 몬스터 설정
        
            // 초기화

            do // 전투 시작
            {
                // 전투 업데이트

                // 플레이어 행동 초기화

                // 플레이어 행동

                // 적행동

                // 카운트 처리
            } while (true);
        
            yield return null;
        }
        #endregion
        
    }
}