using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Util;

namespace Cardinals.Board {
    [Serializable]
    public class TileEvent: MonoBehaviour
    {
        public BoardEventType EventType => _eventType;
        
        private BoardEventType _eventType;

        private ComponentGetter<SpriteRenderer> _renderer 
            = new ComponentGetter<SpriteRenderer>(TypeOfGetter.ChildByName, "Renderer/EventSprite");
        private ComponentGetter<Tile> _tile = new ComponentGetter<Tile>(TypeOfGetter.This);

        public void Set(BoardEventType type)
        {
            _eventType = type; // 타입에 맞는 이벤트 지정

            _renderer.Get(gameObject).sprite =  ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_BoardEvent + type);
            
            _renderer.Get(gameObject).transform.localScale = Vector3.zero;
            _renderer.Get(gameObject).transform.DOScale(1, 1).SetEase(Ease.OutBounce)
                .OnComplete(() =>
                {
                    _renderer.Get(gameObject).transform.LookAt(Camera.main.transform);
                    _renderer.Get(gameObject).transform.Rotate(0, 180, 0);
                });
        }

        public void ClearEvent() {
            _eventType = BoardEventType.Empty;
            _renderer.Get(gameObject).sprite = null;
        }

        public void Activate() {

            if (_eventType != default)
            {
                switch (_eventType)
                {
                    case BoardEventType.Tile :
                        GameManager.I.UI.UITileEvent.Init();
                        break;
                    case BoardEventType.Shop :
                        GameManager.I.UI.UIShop.Init();
                        break;
                    case BoardEventType.Roulette:
                        GameManager.I.UI.UIRoulette.Init();
                        break;
                    case BoardEventType.Number:
                        GameManager.I.UI.UICardEvent.Init();
                        break;
                    default: break;
                }

                ClearEvent();
            }
        }
        
    }

}