using System.Collections;
using DG.Tweening;
using UnityEngine;
using Util;

namespace Cardinals.Enemy.Summon
{
    public class ThunderBolt : BaseEnemySummon
    {
        public void Init()
        {
            base.Init();
            
            // Player 위치에 벼락이 소환됨
            var pos = GameManager.I.Player.transform.position + new Vector3(Random.Range(-1f, 1f), 10, 0);
            
            transform.position = pos;
            transform.DOMoveY(0, Random.Range(0.3f, 0.6f))
                               .SetEase(Ease.InOutElastic)
                               .OnComplete(Execute);
        }

        void Execute()
        {
            GameManager.I.Player.Hit(10);
            Delete();
        }
    }
}