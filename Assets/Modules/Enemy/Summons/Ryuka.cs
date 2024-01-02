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
        public override void Init(Tile tile, string type)
        {
            base.Init(tile, type);

            // 위 아래 움직임 
            _renderer.transform.DOMoveY(1.5f, Random.Range(1.5f, 2f))
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.OutQuad);
        }
        
        /// <summary>
        /// 몬스터 체력을 회복 시킴
        /// </summary>
        public override IEnumerator OnTurn()
        {
            var enemy = GameManager.I.CurrentEnemies.FirstOrDefault();

            _renderer.sprite = _data.spec_sprite_1;
            transform.DOPunchScale(new Vector3(.3f, .3f, .3f), .5f, 2, 1)
                .OnComplete(() => _renderer.sprite = _data.sprite);
            
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