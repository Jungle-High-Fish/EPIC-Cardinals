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
        private List<BaseEnemy> _enemies;
        public List<BaseEnemy> Enemies => _enemies;

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
            _stageController = stageController;

            Debug.Log("전투 시작");
            // 전투 세팅
        
            // 몬스터 설정
            _enemies = new();
            var posArr = GetPositions();
            for (int i = 0, cnt = _enemyList.Length; i < cnt; i++)
            {
                var type = _enemyList[i];
                var enemy = _stageController.InstantiateEnemy(type);
                
                enemy.DieEvent += () =>
                {
                    _enemies.Remove(enemy);
                };
                
                _enemies.Add(enemy);
            }

            GameManager.I.CurrentEnemies = _enemies;
            
            // 초기화
            do // 전투 시작
            {
                // 전투 업데이트
                //GameManager.I.PlayerData.Turn++;
                GameManager.I.Player.StartTurn();
                _enemies.ForEach(enemy => enemy.StartTurn());

                // 플레이어 행동 초기화

                // 플레이어 행동
                GameManager.I.Player.OnTurn();
                yield return GameManager.I.WaitNext(); // 대기?
                
                // 적 행동
                _enemies.ForEach(enemy => enemy.OnTurn());
                
                // 카운트 처리
                GameManager.I.Player.EndTurn();
                _enemies.ForEach(enemy => enemy.EndTurn());
                
                yield return new WaitForSeconds(1f);
            } while (_enemies.Count > 0 && GameManager.I.Player.Hp > 0);

            if (_enemies.Count == 0)
            {
                IsClear = true;
            }
        }
        #endregion
    }
}