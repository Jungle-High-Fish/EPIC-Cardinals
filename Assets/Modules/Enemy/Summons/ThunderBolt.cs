using Cardinals.Game;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Util;

namespace Cardinals.Enemy.Summon
{
    public class ThunderBolt : MonoBehaviour
    {
        public IEnumerator Init()
        {
            // Player 위치에 벼락이 소환됨
            var pos = GameManager.I.Player.transform.position + new Vector3(Random.Range(-1f, 1f), 10, 0);

            bool doWait = false;
            transform.position = pos;
            transform.DOMoveY(0, Random.Range(0.3f, 0.6f))
                               .SetEase(Ease.InOutElastic)
                               .OnComplete(() => doWait = true);

            yield return new WaitUntil(() => doWait);
            
            Execute();
            var evt = GameManager.I.Stage.CurEvent as BattleEvent;
            if (evt != null) evt.ThunderCntByTurn++;
        }

        void Execute()
        {
            GameManager.I.Player.Hit(7);
            Destroy(gameObject);
        }
    }
}