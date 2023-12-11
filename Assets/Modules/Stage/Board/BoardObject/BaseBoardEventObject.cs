using System.Collections;
using Cardinals.Enemy.Summon;
using Cardinals.Enums;
using DG.Tweening;
using UnityEngine;
using Util;
using Resources = Cardinals.Constants.FilePath.Resources;

namespace Cardinals.BoardEvent
{
    /// <summary>
    /// 신규 기획된 보드 이벤트
    /// </summary>
    public abstract class BaseBoardEventObject : BaseBoardObject
    {
        public Board.Tile OnTile { get; }
        
        protected NewBoardEventDataSO _data;
        private int _count;
        public void Init(NewBoardEventType type)
        {
            _data = ResourceLoader.LoadSO<NewBoardEventDataSO>(Resources.SO_BoardEventData);

            _count = _data.keepTurnCount;
        }

        public override IEnumerator OnTurn()
        {
            if (_data.onBoardType == NewBoardEventOnType.Move)
            {
                yield return base.OnTurn();
            }

            if (--_count == 0)
            {
                transform.DOLocalMoveY(-1, .5f).SetEase(Ease.InBounce)
                    .OnComplete(base.Destroy);
            }
        }

        public override IEnumerator OnCollisionPlayer()
        {
            yield return Execute();
        }

        protected abstract IEnumerator Execute();
    }
}