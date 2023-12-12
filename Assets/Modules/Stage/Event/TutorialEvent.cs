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
    public class TutorialEvent: BaseEvent
    {
        private List<BaseEnemy> _enemies;
        public List<BaseEnemy> Enemies => _enemies;

        [SerializeField, InlineEditor] private EnemyDataSO[] _enemyList;
        public EnemyDataSO[] EnemyList => _enemyList;

        [SerializeField] private List<TutorialDataSO> _tutorialData;

        #region Tutorial
        private bool [] _isTutorialClear;
        private int _curTutorialIndex;
        private int _curQuestIndex;
        private int _curSequenceIndex;
        #endregion

        #region Battle
        private StageController _stageController;

        public override IEnumerator Flow(StageController stageController)
        {
            Debug.Log("전투 플로우 실행");

            GameManager.I.UI.UIEndTurnButton.Deactivate();
            
            // 전투 세팅
            _stageController = stageController;
            List<Reward> rewards = new();
            
            // 몬스터 세팅
            InitEnemies(rewards);

            // 카드 매니저 세팅
            GameManager.I.Stage.DiceManager.OnBattle(true);

            // 튜토리얼 데이터
            _isTutorialClear = new bool[_tutorialData.Count - 1];
            for (int i = 0; i < _isTutorialClear.Length - 1; i++) _isTutorialClear[i] = false;
            _curTutorialIndex = 0;
            _curQuestIndex = 0;
            _curSequenceIndex = 0;

            TutorialDataSO curTutorial = null;
            do // 전투 시작
            {
                // 튜토리얼 UI 세팅
                if (curTutorial == null || curTutorial != _tutorialData[_curTutorialIndex]) {
                    curTutorial = _tutorialData[_curTutorialIndex];
                    GameManager.I.UI.UITutorial.Init(curTutorial);
                }

                if (curTutorial.Quests[_curQuestIndex].QuestType != TutorialQuestType.KillMonster) {
                    GameManager.I.UI.UIEndTurnButton.Deactivate();
                }

                // 전투 업데이트
                GameManager.I.Player.StartTurn();
                _enemies.ForEach(enemy => enemy.StartTurn());

                // 플레이어 행동 초기화

                // 플레이어 행동
                if (curTutorial.Quests[_curQuestIndex].QuestType == TutorialQuestType.KillMonster) {
                    GameManager.I.Stage.DiceManager.OnTurn();
                } else {
                    GameManager.I.Stage.DiceManager.DrawHandDecksForTutorial(curTutorial.Cards);
                }
                GameManager.I.Player.OnTurn();

                yield return GameManager.I.WaitNext(); // 대기?

                CheckTurnEndQuest();

                GameManager.I.Stage.DiceManager.SetDiceSelectable(false);

                // 적 행동
                for (int i = _enemies.Count - 1; i >= 0; i--) _enemies[i].OnTurn();
                yield return new WaitForSeconds(1f);
                
                // 카운트 처리
                GameManager.I.Player.EndTurn();
                for (int i = _enemies.Count - 1; i >= 0; i--) _enemies[i].EndTurn();
                
                // 보드 관련 처리
                yield return SummonsAction();
                _stageController.Board.OnTurnEnd();
                
                yield return new WaitForSeconds(1.5f);

                // 튜토리얼 처리
                if (_curQuestIndex == -1 || _tutorialData[_curTutorialIndex].Quests[_curQuestIndex].QuestType != TutorialQuestType.KillMonster) {
                    _isTutorialClear[_curTutorialIndex++] = true;
                    _curQuestIndex = 0;
                    _curSequenceIndex = 0;
                }
            } while (_isTutorialClear.Any(x => !x));

            if (_enemies.Count == 0)
            {
                IsClear = true;
                
                // 전투 종료 초기화
                GameManager.I.Player.Win();
                GameManager.I.Player.EndBattle();
                GameManager.I.Stage.Board.ClearBoardAfterBattleEvent();
                RemoveSummons();

                GameManager.I.UI.UIEndTurnButton.Deactivate();

                _curTutorialIndex = _tutorialData.Count - 1;
                curTutorial = _tutorialData[_curTutorialIndex];
                GameManager.I.UI.UITutorial.Init(curTutorial);

                yield return GameManager.I.Stage.SelectBlessFlow();
            }
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
                    GameManager.I.UI.UITutorial.ShowEndTurnQuest(false);
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

        public void CheckTurnEndQuest() {
            GameManager.I.UI.UITutorial.AchieveEndTurnQuest();
        }

        public (bool hasSequence, QuestData.CardUseQuestData targetSequence) CheckIfHasCardSequence() {
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

        private void InitEnemies(List<Reward> rewards) {
            Vector3[] enemyPositions = _stageController.Board.SetEnemyNumber(_enemyList.Length);

            _enemies = new();
            for (int i = 0, cnt = _enemyList.Length; i < cnt; i++)
            {
                var enemyData = _enemyList[i];
                var enemy = _stageController.InstantiateEnemy(enemyData, enemyPositions[i]);
                
                enemy.DieEvent += () =>
                {
                    //AddReward(rewards, enemy.Rewards);
                    _enemies.Remove(enemy);
                    if (_enemies.Count> 0) GameManager.I.Stage.Board.SetEnemyNumber(_enemies.Count); // 1마리가 되었을 때, 카드 드래그 영역 수정
                    CheckEnemyKillQuest();
                };
                
                _enemies.Add(enemy);
            }

            GameManager.I.CurrentEnemies = _enemies;
            //_stageController.EnemyInfoController.Init(_enemyList.Length);
        }

        private IEnumerator SummonsAction()
        {
            var summons = GameManager.I.Stage.BoardObjects;
            for (int i = summons.Count - 1; i >= 0; i--)
            {
                yield return summons[i].OnTurn();
            }

            yield return null;
        }

        private void RemoveSummons()
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
        IEnumerator WaitReward(IEnumerable<Reward> rewards)
        {
            GameManager.I.UI.UIEndTurnButton.Activate(true);
            
            // 보상 설정
            _stageController.RewardBox.Set(rewards); // 해당 위치에서 구체화 됩니다. 

            IsSelect = false;
            yield return new WaitUntil(() => IsSelect); // 플레이어의 보상 선택 후 [턴 종료] 대기
            
            _stageController.RewardBox.Disable();
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
        #endregion
    }
}