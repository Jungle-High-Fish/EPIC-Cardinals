using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.Enemy;
using UnityEngine;
using Util;

namespace Cardinals.Game
{
    [CreateAssetMenu(fileName = "BattleEvent", menuName = "Cardinals/Event/Battle")]
    public class BattleEvent: BaseEvent
    {
        [SerializeField] private EnemyType[] _enemyList;
        public EnemyType[] EnemyList => _enemyList;

        public Vector3[] GetPositions()
        {
            return new[] { new Vector3()};
        }

        #region Battle
        private StageController _stageController;

        public override IEnumerator Flow(StageController stageController)
        {
            Debug.Log("asdf");
            _stageController = stageController;

            Debug.Log("전투 시작");
            // 전투 세팅
        
            // 몬스터 설정
            List<BaseEnemy> enemies = new();
            var posArr = GetPositions();
            for (int i = 0, cnt = _enemyList.Length; i < cnt; i++)
            {
                var type = _enemyList[i];
                var enemy = _stageController.InstantiateEnemy(type);
                
                enemy.DieEvent += () =>
                {
                    enemies.Remove(enemy);
                };
                
                enemies.Add(enemy);
            }

            GameManager.I.CurrentEnemies = enemies;
            
            // 초기화
            do // 전투 시작
            {
                // 전투 업데이트
                GameManager.I.PlayerData.Turn++;
                GameManager.I.TempPlayer.StartTurn();
                enemies.ForEach(enemy => enemy.StartTurn());

                // 플레이어 행동 초기화

                // 플레이어 행동
                GameManager.I.TempPlayer.OnTurn();
                yield return null; // 대기?
                
                // 적 행동
                enemies.ForEach(enemy => enemy.OnTurn());
                
                // 카운트 처리
                GameManager.I.TempPlayer.EndTurn();
                enemies.ForEach(enemy => enemy.EndTurn());
                
                yield return new WaitForSeconds(1f);
            } while (enemies.Count > 0 && GameManager.I.TempPlayer.Hp > 0);

            if (enemies.Count == 0)
            {
                IsClear = true;
            }
        }
        #endregion
    }
}