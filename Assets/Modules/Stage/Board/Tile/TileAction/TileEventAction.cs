using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using Util;

namespace Cardinals.Board {
    
    public class TileEventAction: TileAction
    {
        [Header("컴포넌트")]
        [SerializeField] [ReadOnly] private BoardEventType _eventType;
        public BoardEventType EventType => _eventType;
        
        [SerializeField] private SpriteRenderer _renderer;
        public void Set(BoardEventType type)
        {
            _eventType = type; // 타입에 맞는 이벤트 지정

            _renderer ??= _tile.GetComponentInChildren<SpriteRenderer>();
            _renderer.sprite =  ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_BoardEvent + type);
             
            // 타일 위에 해당 컴포넌트 설정 필요
        }
        
        
        public override void Act(int value, BaseEntity target) {
            // nothing
        }

        public override void ArriveAction() {

            if (_eventType != default)
            {
                switch (_eventType)
                {
                    case BoardEventType.Shop :
                        GameManager.I.UI.UIShop.Init();
                        break;
                    case BoardEventType.Roulette:
                        GameManager.I.UI.UIRoulette.Init();
                        break;
                    case BoardEventType.Number:
                        GameManager.I.UI.UICardEvent.Init();;
                        break;
                    default: break;
                }

                _renderer.sprite = null;
                _eventType = default; 
            }
        }
        
    }

}