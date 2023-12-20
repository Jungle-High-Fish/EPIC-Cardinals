using System.Collections;
using Cardinals.Enemy.Summon;
using Cardinals.Enums;
using Cardinals.UI;
using Cardinals.UI.Description;
using DG.Tweening;
using Unity.VisualScripting;
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
        private NewBoardEventDataSO _data;
        protected override int MoveCount => _data.moveCount; 
        
        private int _count;
        public void Init(Board.Tile tile, NewBoardEventType type)
        {
            Init(tile);
            
            _data = ResourceLoader.LoadSO<NewBoardEventDataSO>(Resources.SO_BoardEventData + type);
            _count = _data.keepTurnCount;

            _renderer.sprite = _data.sprite;
            
            transform.AddComponent<BoardEventObjectDescription>().Init(_data);
            GetComponent<DescriptionConnector>().Init();
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
            if (_data.executeType == NewBoardEventExecuteType.Touch)
            {
                yield return Execute();
                base.Destroy();
            }
        }

        public override IEnumerator ArrivePlayer()
        {
            if (_data.executeType == NewBoardEventExecuteType.Arrive)
            {
                yield return Execute();
                base.Destroy();
            }
        }

        protected abstract IEnumerator Execute();
    }
}