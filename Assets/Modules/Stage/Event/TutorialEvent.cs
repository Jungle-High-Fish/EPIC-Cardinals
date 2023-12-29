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
using Cardinals.Tutorial;
using Cardinals.Game;
using TMPro;

namespace Cardinals.Tutorial
{
    [CreateAssetMenu(fileName = "TutorialEvent", menuName = "Cardinals/Event/Tutorial")]
    public class TutorialEvent: BattleEvent
    {
        [SerializeField, InlineEditor] private List<TutorialDataSO> _tutorialData;

        #region Tutorial
        private bool [] _isTutorialClear;
        private int _curTutorialIndex;
        private int _curQuestIndex;
        private int _curSequenceIndex;
        #endregion

        public override IEnumerator Flow(StageController stageController)
        {
            Debug.Log("전투 플로우 실행");

            GameManager.I.UI.UIEndTurnButton.Deactivate();
            
            // 전투 세팅
            Player player = GameManager.I.Player;
            StageController stage = GameManager.I.Stage;
            Board.Board board = GameManager.I.Stage.Board;
            DiceManager diceManager = GameManager.I.Stage.DiceManager;
            _enemies = new();
            EnemyGradeType enemyGradeType;
            
            // 몬스터 세팅
            InitEnemy(_enemies, CheckEnemyKillQuest);
            GameManager.I.CurrentEnemies = _enemies;
            enemyGradeType = _enemies.First().EnemyData.enemyGradeType;

            // 카드 매니저 세팅
            diceManager.OnBattle(isTutorial:true);

            // 튜토리얼 데이터
            _isTutorialClear = new bool[_tutorialData.Count - 1];
            for (int i = 0; i < _isTutorialClear.Length - 1; i++) _isTutorialClear[i] = false;
            _curTutorialIndex = 0;
            _curQuestIndex = 0;
            _curSequenceIndex = 0;

            // 스팀 핸들러 연결
            GameManager.I.SteamHandler.SetBattleStateDisplay(
                GameManager.I.Stage.Index,
                _enemies.Select(x => x.EnemyData.enemyType).ToList()
            );

            // 플레이어 이벤트 등록
            player.HomeReturnEvent += OnPlayerRound;

            // 시작 정보 초기화
            _turn = 1;
            _round = 0;
            _roundStartTile = player.OnTile;
            GameManager.I.StartCoroutine(stage.StartFlag.Show(_roundStartTile));
            GameManager.I.UI.UINewPlayerInfo.TurnRoundStatus.SetRound(_round);

            // 튜토리얼 초기화
            TutorialDataSO curTutorial = null;

            do // 전투 시작
            {
                // 턴 UI 업데이트
                GameManager.I.UI.UINewPlayerInfo.TurnRoundStatus.SetTurn(_turn);

                // 플레이어 턴 알림
                yield return GameManager.I.UI.UITurnAlert.Show(
                    ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_UI_PlayerTurnAlert)
                );
                yield return new WaitForSeconds(1.3f);
                yield return GameManager.I.UI.UITurnAlert.Hide();

                // 튜토리얼 UI 세팅
                if (curTutorial == null || curTutorial != _tutorialData[_curTutorialIndex]) {
                    curTutorial = _tutorialData[_curTutorialIndex];
                    GameManager.I.UI.UITutorial.Init(curTutorial);
                }

                if (curTutorial.Quests[_curQuestIndex].QuestType != TutorialQuestType.KillMonster) {
                    GameManager.I.UI.UIEndTurnButton.Deactivate();
                }

                // 전투 업데이트
                yield return player.StartTurn();
                foreach (var enemy in _enemies)
                {
                    yield return enemy.StartTurn();
                }

                // 플레이어 행동
                if (curTutorial.Quests[_curQuestIndex].QuestType == TutorialQuestType.KillMonster) {
                    yield return diceManager.OnTurn();
                    GameManager.I.UI.UIEndTurnButton.Activate();
                } else {
                    int[] diceNumbers = new int[5];
                    for (int i = 0; i < 5; i++) {
                        if (i < curTutorial.Cards.Count()) {
                            diceNumbers[i] = curTutorial.Cards[i];
                        } else {
                            diceNumbers[i] = -1;
                        }
                    }
                    yield return diceManager.OnTutorialTurn(diceNumbers);
                }
                yield return GameManager.I.Player.OnTurn();
                CheckTurnEndQuest();

                if (CheckPlayerWin) break;

                // 버프 처리
                player.OnBuff();
                for (int i = _enemies.Count - 1; i >= 0; i--) _enemies[i].OnBuff();

                // 플레이어 PreEndTurn 처리
                yield return player.PreEndTurn();

                // 적 턴 알림
                yield return GameManager.I.UI.UITurnAlert.Show(_enemies.First().EnemyData.turnAlertSprite);
                yield return new WaitForSeconds(1.3f);
                yield return GameManager.I.UI.UITurnAlert.Hide();

                // 적 행동
                foreach (var e in _enemies)
                {
                    yield return e.OnPreTurn();
                }
                for (int i = _enemies.Count - 1; i >= 0; i--)
                {
                    yield return _enemies[i].OnTurn();
                }

                if (CheckEnemyWin) break;
                
                // 보드 관련 처리
                yield return SummonsAction();
                yield return board.OnTurnEnd();

                // 플레이어 턴 종료 처리
                yield return player.EndTurn(); 

                // 적 턴 종료 처리
                for (int i = _enemies.Count - 1; i >= 0; i--)
                {
                    yield return _enemies[i].EndTurn();
                }

                // 튜토리얼 처리
                if (_curQuestIndex == -1 || _tutorialData[_curTutorialIndex].Quests[_curQuestIndex].QuestType != TutorialQuestType.KillMonster) {
                    _isTutorialClear[_curTutorialIndex++] = true;
                    _curQuestIndex = 0;
                    _curSequenceIndex = 0;
                }

                yield return new WaitForSeconds(1f);
            } while (_isTutorialClear.Any(x => !x));

            yield return GameManager.I.Stage.DiceManager.EndBattle();
            yield return stage.StartFlag.Hide();

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
        }

        public override void Test_ChangeEnemy(EnemyType enemyType) {
            if (GameManager.I.CurrentEnemies == null) return;
            if (!GameManager.I.IsWaitingForNext) return;

            var enemyData = EnemyDataSO.Data(enemyType);
            List<BaseEnemy> originalEnemies = new();
            foreach (var e in _enemies) {
                originalEnemies.Add(e);
            }
            InitEnemy(_enemies, onEnemyDie:CheckEnemyKillQuest, enemyDataSOs: new EnemyDataSO[] {enemyData});
            
            foreach (var e in originalEnemies) {
                e.DieEvent?.Invoke();
            }

            foreach (var e in _enemies) {
                e.UpdatePatternEvent?.Invoke(e.CurPattern);
            }

            GameManager.I.CurrentEnemies = _enemies;
        }


        public void CheckCardQuest(int cardNumber, MouseState howToUse) {
            if (_curQuestIndex == -1) {
                return;
            }

            var curTutorial = _tutorialData[_curTutorialIndex];

            if (curTutorial.Quests[_curQuestIndex].QuestType != TutorialQuestType.Card) {
                return;
            }

            if (curTutorial.Quests[_curQuestIndex].HasCardSequence == true) {
                if (curTutorial.Quests[_curQuestIndex].NeedCardSequence[_curSequenceIndex].CardNumber != cardNumber) {
                    return;
                }

                if (curTutorial.Quests[_curQuestIndex].NeedCardSequence[_curSequenceIndex].HowToUse != howToUse) {
                    return;
                }

                _curSequenceIndex++;
            }

            var questResult = GameManager.I.UI.UITutorial.AchieveQuest(_curQuestIndex, 1);
            if (questResult.hasClear) {
                _curQuestIndex = questResult.nextIdx;
                if (questResult.nextIdx == -1) {
                    GameManager.I.UI.UITutorial.ShowEndTurnQuest(false);
                    return;
                }

                if (curTutorial.Quests[_curQuestIndex].HasCardSequence) {
                    _curSequenceIndex = 0;
                }
            }
        }

        public void CheckMagicSelectQuest() {
            if (_curQuestIndex == -1) {
                return;
            }

            var curTutorial = _tutorialData[_curTutorialIndex];

            if (curTutorial.Quests[_curQuestIndex].QuestType != TutorialQuestType.TileMagicSelect) {
                return;
            }

            var questResult = GameManager.I.UI.UITutorial.AchieveQuest(_curQuestIndex, 1);
            if (questResult.hasClear) {
                _curQuestIndex = questResult.nextIdx;
                if (questResult.nextIdx == -1) {
                    GameManager.I.UI.UITutorial.ShowEndTurnQuest(false);
                    return;
                }
            }
        }

        public void CheckEnemyKillQuest() {
            if (_curQuestIndex == -1) {
                return;
            }

            var curTutorial = _tutorialData[_curTutorialIndex];

            if (curTutorial.Quests[_curQuestIndex].QuestType != TutorialQuestType.KillMonster) {
                return;
            }

            var questResult = GameManager.I.UI.UITutorial.AchieveQuest(_curQuestIndex, 1);
            if (questResult.hasClear) {
                _curQuestIndex = questResult.nextIdx;
                if (questResult.nextIdx == -1) {
                    _isTutorialClear[_curTutorialIndex++] = true;
                    _curQuestIndex = 0;
                    _curSequenceIndex = 0;
                    curTutorial = _tutorialData[_curTutorialIndex];
                    GameManager.I.UI.UITutorial.Init(curTutorial);
                    return;
                }
            }
        }

        public void CheckBlessSelectQuest() {
            if (_curQuestIndex == -1) {
                return;
            }

            var curTutorial = _tutorialData[_curTutorialIndex];

            if (curTutorial.Quests[_curQuestIndex].QuestType != TutorialQuestType.BlessSelect) {
                return;
            }

            var questResult = GameManager.I.UI.UITutorial.AchieveQuest(_curQuestIndex, 1);
            if (questResult.hasClear) {
                _curQuestIndex = questResult.nextIdx;
                if (questResult.nextIdx == -1) {
                    GameManager.I.UI.UITutorial.Close();
                    return;
                }
            }
        }

        public void CheckRewardSelectQuest() {
            if (_curQuestIndex == -1) {
                return;
            }

            var curTutorial = _tutorialData[_curTutorialIndex];

            if (curTutorial.Quests[_curQuestIndex].QuestType != TutorialQuestType.RewardSelect) {
                return;
            }

            var questResult = GameManager.I.UI.UITutorial.AchieveQuest(_curQuestIndex, 1);
            if (questResult.hasClear) {
                _curQuestIndex = questResult.nextIdx;
                if (questResult.nextIdx == -1) {
                    GameManager.I.UI.UITutorial.Close();
                    return;
                }
            }
        }

        public void CheckTurnEndQuest() {
            GameManager.I.UI.UITutorial.AchieveEndTurnQuest();
        }

        public (bool hasSequence, QuestData.CardUseQuestData targetSequence) CheckIfHasDiceSequence() {
            if (_curQuestIndex == -1) {
                return (false, null);
            }

            var curTutorial = _tutorialData[_curTutorialIndex];

            if (curTutorial.Quests[_curQuestIndex].QuestType != TutorialQuestType.Card) {
                return (false, null);
            }

            if (curTutorial.Quests[_curQuestIndex].HasCardSequence == false) {
                return (false, null);
            }

            return (true, curTutorial.Quests[_curQuestIndex].NeedCardSequence[_curSequenceIndex]);
        }
    }
}