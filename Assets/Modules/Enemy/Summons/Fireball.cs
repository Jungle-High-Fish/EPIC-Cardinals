using System;
using System.Collections;
using System.Numerics;
using Cardinals.Board;
using Cardinals.Enums;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace Cardinals.BoardObject.Summon
{
    public class Fireball : BaseBoardObject
    {
        public override void Init(Tile tile, string type)
        {
            base.Init(tile, type);

            // 위 아래 움직임 
            _renderer.transform.DOMoveY(1.5f, Random.Range(1.5f, 2f))
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.OutQuad);
        }

        protected override IEnumerator Execute()
        {
            bool next = false;
            GameManager.I.Player.Hit(5, TileMagicType.Fire);
            transform.DOShakePosition(.5f, .5f).OnComplete(() => { next = true; });
            yield return new WaitUntil(() => next);
        }
    }
}
