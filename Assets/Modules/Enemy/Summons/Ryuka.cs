using System.Collections;
using System.Linq;
using Cardinals.Board;
using Cardinals.BoardEvent;
using DG.Tweening;
using UnityEngine;

namespace Cardinals.Enemy.Summon
{
    public class Ryuka : BaseBoardObject
    {
        public override IEnumerator OnTurn()
        {
            Execute();
            yield return base.OnTurn();
        }

        /// <summary>
        /// 몬스터 체력을 회복 시킴
        /// </summary>
        void Execute()
        {
            var enemy = GameManager.I.CurrentEnemies.FirstOrDefault();

            if (enemy != null)
            {
                enemy.Heal(2);
            }
        }

        public override IEnumerator OnCollisionPlayer()
        {
            transform
                .DOShakePosition(.5f, .5f)
                .OnComplete(() =>
                {
                    base.Destroy();
                });
            yield return null;
        }
    }
}