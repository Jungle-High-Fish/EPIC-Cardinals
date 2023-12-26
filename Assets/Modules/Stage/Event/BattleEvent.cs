using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cardinals.Enums;
using Cardinals.Enemy;
using Sirenix.Utilities;
using UnityEngine;
using Util;
using Sirenix.OdinInspector;
using Cardinals.Board;
using System;

namespace Cardinals.Game
{
    [CreateAssetMenu(fileName = "BattleEvent", menuName = "Cardinals/Event/Battle")]
    public class BattleEvent: BaseEvent
    {
        public virtual int Turn => _turn;
        public virtual int Round => _round;
        public Tile RoundStartTile => _roundStartTile;

        [SerializeField, InlineEditor] protected EnemyDataSO[] _enemyList;
        protected int _turn = 1;
        protected int _round = 0;
        protected Tile _roundStartTile;

        protected bool CheckPlayerWin => !GameManager.I.CurrentEnemies.Any();
        protected bool CheckEnemyWin => GameManager.I.Player.Hp == 0;

        public override IEnumerator Flow(StageController stageController)
        {
            Debug.Log("전투 플로우 실행");
            
            // 전투 세팅
            Player player = GameManager.I.Player;
            StageController stage = GameManager.I.Stage;
            Board.Board board = GameManager.I.Stage.Board;
            //CardManager cardManager = GameManager.I.Stage.CardManager;
            DiceManager diceManager = GameManager.I.Stage.DiceManager;
            List<BaseEnemy> enemies = new();
            EnemyGradeType enemyGradeType;

            // 이벤트 시작 이벤트 호출
            stage.OnEventStartCall();
            
            // 몬스터 정보 초기화
            InitEnemy(enemies);
            GameManager.I.CurrentEnemies = enemies;
            enemyGradeType = enemies.First().EnemyData.enemyGradeType;
            
            //cardManager.OnBattle();
            diceManager.OnBattle();

            GameManager.I.SteamHandler.SetBattleStateDisplay(
                GameManager.I.Stage.Index,
                enemies.Select(x => x.EnemyData.enemyType).ToList()
            );

            // 플레이어 이벤트 등록
            player.HomeReturnEvent += OnPlayerRound;

            // 시작 정보 초기화
            _turn = 1;
            _round = 0;
            _roundStartTile = player.OnTile;
            GameManager.I.StartCoroutine(stage.StartFlag.Show(_roundStartTile));
            GameManager.I.UI.UINewPlayerInfo.TurnRoundStatus.SetRound(_round);

            do // 전투 시작
            {
                // 턴 UI 업데이트
                GameManager.I.UI.UINewPlayerInfo.TurnRoundStatus.SetTurn(_turn);

                // 3턴마다 보드 이벤트 생성
                if (_turn++ % 3 == 0) GameManager.I.Stage.GenerateNewBoardEvent();
                
                // 전투 업데이트
                yield return player.StartTurn();
                foreach (var enemy in enemies)
                {
                    yield return enemy.StartTurn();
                }

                // 플레이어 행동
                //yield return cardManager.OnTurn();
                yield return diceManager.OnTurn();
                yield return player.OnTurn();
                if (CheckPlayerWin) break;

                // 아래 내용 플레이어 OnTurn으로 이동했습니다.
                // // 턴 종료 버튼 활성화
                // GameManager.I.UI.UIEndTurnButton.Activate();
                //
                // yield return GameManager.I.WaitNext(); // 플레이어의 [턴 종료] 버튼 선택 대기

                // GameManager.I.UI.UIEndTurnButton.Deactivate();
                // cardManager.SetCardSelectable(false);
                
                // 버프 처리
                player.OnBuff();
                for (int i = enemies.Count - 1; i >= 0; i--) enemies[i].OnBuff();

                // 플레이어 PreEndTurn 처리
                yield return player.PreEndTurn();

                // 적 행동
                foreach (var e in enemies)
                {
                    yield return e.OnPreTurn();
                }
                for (int i = enemies.Count - 1; i >= 0; i--)
                {
                    yield return enemies[i].OnTurn();
                }

                if (CheckEnemyWin) break;
                
                // 보드 관련 처리
                yield return SummonsAction();
                yield return board.OnTurnEnd();
                
                // 플레이어 턴 종료 처리
                yield return player.EndTurn();

                // 적 턴 종료 처리
                for (int i = enemies.Count - 1; i >= 0; i--)
                {
                    yield return enemies[i].EndTurn();
                }
            } while (!(CheckPlayerWin || CheckEnemyWin));

            yield return GameManager.I.Stage.DiceManager.EndBattle();
            yield return stage.StartFlag.Hide();

            // 플레이어의 승리
            if (CheckPlayerWin)
            {
                IsClear = true;
                
                // 전투 종료 초기화
                player.Win();
                player.EndBattle();
                board.ClearBoardAfterBattleEvent();
                RemoveSummons();
                yield return WaitReward(enemyGradeType);
            }

            player.HomeReturnEvent -= OnPlayerRound;
        }

        /// <summary>
        /// 몬스터 초기화
        /// </summary>
        protected void InitEnemy(List<BaseEnemy> enemies, Action onEnemyDie=null)
        {
            Vector3[] enemyPositions = GameManager.I.Stage.Board.SetEnemyNumber(_enemyList.Length);
            
            for (int i = 0, cnt = _enemyList.Length; i < cnt; i++)
            {
                var enemyData = _enemyList[i];
                var enemy = GameManager.I.Stage.InstantiateEnemy(enemyData, enemyPositions[i]);
                
                enemy.DieEvent += () =>
                {
                    enemies.Remove(enemy);
                    if (enemies.Count> 0)
                    {
                        GameManager.I.Stage.Board.SetEnemyNumber(enemies.Count); // 1마리가 되었을 때, 카드 드래그 영역 수정
                    }
                    else
                    {
                        GameManager.I.Next();
                    }

                    onEnemyDie?.Invoke();   
                };
                
                enemies.Add(enemy);
            }
        }

        protected void OnPlayerRound() {
            _round++;
            GameManager.I.UI.UINewPlayerInfo.TurnRoundStatus.SetRound(_round);
        }
        
        protected IEnumerator SummonsAction()
        {
            var summons = GameManager.I.Stage.BoardObjects;
            for (int i = summons.Count - 1; i >= 0; i--)
            {
                yield return summons[i].OnTurn();
            }

            yield return null;
        }

        protected void RemoveSummons()
        {
            var summons = GameManager.I.Stage.BoardObjects;
            for (int i = summons.Count - 1; i >= 0; i--)
            {
                summons[i].Destroy();
            }
        }

        /// <summary>
        /// 플레이어 전리품 획득 대기
        /// </summary>
        /// <param name="rewards"></param>
        protected IEnumerator WaitReward(EnemyGradeType grade)
        {
            GameManager.I.UI.UIEndTurnButton.Activate(true);
            
            // 보상 설정
            GameManager.I.Stage.RewardBox.SetByGrade(grade); // 해당 위치에서 구체화 됩니다. 

            IsSelect = false;
            yield return new WaitUntil(() => IsSelect); // 플레이어의 보상 선택 후 [턴 종료] 대기
            
            GameManager.I.Player.MotionIdle();
            GameManager.I.Stage.RewardBox.Disable();
        }

        /// <summary>
        /// 한 전투에서의 보상을 누적하는 함수
        /// </summary>
        /// <param name="accrueRewards">누적 보상</param>
        /// <param name="rewards">처치한 몬스터 보상</param>
        void AddReward(List<Reward> accrueRewards, Reward[] rewards)
        {
            if (rewards != null) // 보상이 존재하지 않는 경우, 패스
            {
                foreach (var reward in rewards)
                {
                    // 이미 골드 타입이 있는 경우 누적할 것
                    if (reward.Type == RewardType.Gold)
                    {
                        var r = accrueRewards.FirstOrDefault(x => x.Type == RewardType.Gold);
                        if (r == null)
                        {
                            accrueRewards.Add(reward);   
                        }
                        else
                        {
                            r.Value += reward.Value;
                        }
                    }
                    else
                    {
                        accrueRewards.Add(reward);
                    }
                }
            }
        }
    }
}