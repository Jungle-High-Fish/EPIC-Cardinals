using System.Collections;
using System.Collections.Generic;
using Cardinals;
using Cardinals.Entity.UI;
using Cardinals.Enums;
using Cardinals.UI;
using Util;
using UnityEngine;
using UnityEngine.UI;

namespace Cardinals.UI
{
    public class UIPlayerStatus : UIEntityStatus
    {
        [Header("Action")]
        [SerializeField] protected Transform _actionTr;
        [SerializeField] protected Image _actionIconImg;
        
        [Header("Bubble")]
        [SerializeField] private Bubble _bubble;
        
        public override void Init(BaseEntity entity)
        {
            base.Init(entity);
            (entity as Player).UpdateActionEvent += UpdateAction;

            UpdateAction();

            _entity.Bubble = _bubble;
            _bubble.SetBubble(_entity.BubbleText.start);
        }


        private void UpdateAction(PlayerActionType type = PlayerActionType.None)
        {
            if (type == PlayerActionType.None || type == PlayerActionType.Cancel)
            {
                _actionTr.gameObject.SetActive(false);
            }
            else
            {
                _actionTr.gameObject.SetActive(true);
                _actionIconImg.sprite =
                    ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_Player_Action + type);
            
                _actionTr.localScale = Vector3.one;
                // _actionTr.DOPunchScale(new Vector3(.5f, .5f, 1), .3f, 2).SetEase(Ease.InBounce);
            }
        }
    }
}