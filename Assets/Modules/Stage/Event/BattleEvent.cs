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

        public Vector3[] GetPositions()
        {
            return new[] { new Vector3()};
        }

        #region Battle
        private StageController _stageController;

        public override IEnumerator Flow(StageController stageController)
        {
            Debug.Log("전투 플로우 실행");
            
            // 전투 세팅
            _stageController = stageController;
            List<Reward> rewards = new();
        
            // 몬스터 설정
            _enemies = new();
            var posArr = GetPositions();
            for (int i = 0, cnt = _enemyList.Length; i < cnt; i++)
            {
                var enemyData = _enemyList[i];
                var enemy = _stageController.InstantiateEnemy(enemyData);
                
                enemy.DieEvent += () =>
                {
                    AddReward(rewards, enemy.Rewards);
                    _enemies.Remove(enemy);
                };
                
                _enemies.Add(enemy);
            }

            GameManager.I.CurrentEnemies = _enemies;
            _stageController.Board.SetEnemyNumber(_enemies.Count);

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
                _enemies.ForEach(enemy => enemy.OnTurn());
                
                // 카운트 처리
                GameManager.I.Player.EndTurn();
                _enemies.ForEach(enemy => enemy.EndTurn());
                
                // 보드 관련 처리
                yield return SummonsAction();
                _stageController.Board.OnTurnEnd();
                
                yield return new WaitForSeconds(1f);
            } while (_enemies.Count > 0 && GameManager.I.Player.Hp > 0);

            if (_enemies.Count == 0)
            {
                IsClear = true;
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
        #endregion

        /// <summary>
        /// [축복] 메테오 소환
        /// [TODO] 메테오 소환. 해당 로직 위치 고민 필요
        /// </summary>
        public void Meteor()
        {
            Debug.Log("메테오가 소환됩니다.");
            Enemies.ForEach(e => e.Hit(20));
        }
    }
}