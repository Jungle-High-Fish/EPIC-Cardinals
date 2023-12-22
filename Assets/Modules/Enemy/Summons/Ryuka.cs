using System.Collections;
using System.Linq;
using Cardinals.Board;
using Cardinals.BoardEvent;
using DG.Tweening;
using UnityEngine;

namespace Cardinals.BoardObject.Summon
{
    public class Ryuka : BaseBoardObject
    {
        /// <summary>
        /// 몬스터 체력을 회복 시킴
        /// </summary>
        public override IEnumerator OnTurn()
        {
            var enemy = GameManager.I.CurrentEnemies.FirstOrDefault();
            enemy?.Heal(2);
            
            yield return base.OnTurn();
        }

        protected override IEnumerator Execute()
        {
            bool next = false;
            transform.DOShakePosition(.5f, .5f).OnComplete(() => { next = true; });
            yield return new WaitUntil(() => next);
        }
    }
}