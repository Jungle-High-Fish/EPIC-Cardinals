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
            var pos = GameManager.I.Player.transform.position + new Vector3(0, 10, 0);
            
            _renderer.transform.position = pos;
            _renderer.transform.DOMoveY(0, .5f)
                               .SetEase(Ease.InOutElastic)
                               .OnComplete(Execute);
        }

        void Execute()
        {
            GameManager.I.Player.Hit(10);
            Destroy(gameObject);
        }
    }
}