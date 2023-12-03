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
        private List<BaseEnemy> _enemies;
        public List<BaseEnemy> Enemies => _enemies;

        [SerializeField, InlineEditor] private EnemyDataSO[] _enemyList;
        public EnemyDataSO[] EnemyList => _enemyList;

        #region Battle
        private StageController _stageController;

        public override IEnumerator Flow(StageController stageController)
        {
            Debug.Log("전투 플로우 실행");
            
            // 전투 세팅
            _stageController = stageController;
            List<Reward> rewards = new();
        
            // 몬스터 설정
            Vector3[] enemyPositions = _stageController.Board.SetEnemyNumber(_enemyList.Length);

            _enemies = new();
            for (int i = 0, cnt = _enemyList.Length; i < cnt; i++)
            {
                var enemyData = _enemyList[i];
                var enemy = _stageController.InstantiateEnemy(enemyData, enemyPositions[i]);
                
                enemy.DieEvent += () =>
                {
                    AddReward(rewards, enemy.Rewards);
                    _enemies.Remove(enemy);
                    if (_enemies.Count> 0) GameManager.I.Stage.Board.SetEnemyNumber(_enemies.Count); // 1마리가 되었을 때, 카드 드래그 영역 수정
                };
                
                _enemies.Add(enemy);
            }

            GameManager.I.CurrentEnemies = _enemies;
            _stageController.EnemyInfoController.Init(_enemyList.Length);
            GameManager.I.Stage.CardManager.OnBattle();

            do // 전투 시작
            {
                // 전투 업데이트
                GameManager.I.Player.StartTurn();
                _enemies.ForEach(enemy => enemy.StartTurn());

                // 플레이어 행동 초기화

                // 플레이어 행동
                GameManager.I.Player.OnTurn();

                // 턴 종료 버튼 활성화
                GameManager.I.UI.UIEndTurnButton.Activate();

                yield return GameManager.I.WaitNext(); // 대기?

                GameManager.I.UI.UIEndTurnButton.Deactivate();
                
                // 적 행동
                for (int i = _enemies.Count - 1; i >= 0; i--) _enemies[i].OnTurn();
                yield return new WaitForSeconds(1f);
                
                // 카운트 처리
                GameManager.I.Player.EndTurn();
                for (int i = _enemies.Count - 1; i >= 0; i--) _enemies[i].EndTurn();
                
                // 보드 관련 처리
                yield return SummonsAction();
                _stageController.Board.OnTurnEnd();
                
                yield return new WaitForSeconds(1f);
            } while (_enemies.Count > 0 && GameManager.I.Player.Hp > 0);

            if (_enemies.Count == 0)
            {
                IsClear = true;
                
                // 전투 종료 초기화
                GameManager.I.Player.Win();
                GameManager.I.Player.EndBattle();
                GameManager.I.Stage.Board.ClearBoardAfterBattleEvent();
                RemoveSummons();
                yield return WaitReward(rewards);
            }
        }

        private IEnumerator SummonsAction()
        {
            var summons = GameManager.I.Stage.Summons;
            for (int i = summons.Count - 1; i >= 0; i--)
            {
                yield return summons[i].OnTurn();
            }

            yield return null;
        }

        private void RemoveSummons()
        {
            var summons = GameManager.I.Stage.Summons;
            for (int i = summons.Count - 1; i >= 0; i--)
            {
                summons[i].Delete();
            }
        }

        /// <summary>
        /// 플레이어 전리품 획득 대기
        /// </summary>
        /// <param name="rewards"></param>
        IEnumerator WaitReward(IEnumerable<Reward> rewards)
        {
            GameManager.I.UI.UIEndTurnButton.Activate();
            
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