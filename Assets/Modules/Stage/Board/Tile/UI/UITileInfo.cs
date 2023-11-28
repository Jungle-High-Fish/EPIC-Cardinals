using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.Board;
using UnityEngine;
using UnityEngine.UI;
using Util;
using TMPro;
using DG.Tweening;

namespace Cardinals.UI {
    public class UITileInfo: MonoBehaviour {
        ComponentGetter<Image> _header = new ComponentGetter<Image>(
            TypeOfGetter.ChildByName, 
            "Header"
        );

        ComponentGetter<Transform> _levelGuage = new ComponentGetter<Transform>(
            TypeOfGetter.ChildByName, 
            "Body/LevelGuage"
        );

        ComponentGetter<Image> _expBar = new ComponentGetter<Image>(
            TypeOfGetter.ChildByName, 
            "Body/LevelGuage/Bar"
        );

        ComponentGetter<TextMeshProUGUI> _levelText = new ComponentGetter<TextMeshProUGUI>(
            TypeOfGetter.ChildByName, 
            "Body/LevelGuage/LevelText"
        );

        ComponentGetter<TextMeshProUGUI> _expText = new ComponentGetter<TextMeshProUGUI>(
            TypeOfGetter.ChildByName, 
            "Body/LevelGuage/ExpText"
        );

        ComponentGetter<Image> _actionEmblem = new ComponentGetter<Image>(
            TypeOfGetter.ChildByName, 
            "Body/ActionEmblem"
        );

        ComponentGetter<Image> _eventEmblem = new ComponentGetter<Image>(
            TypeOfGetter.ChildByName, 
            "Body/EventEmblem"
        );

        private Tile _tile;
        
        public void Show(Tile tile, bool isAnimation = true) {
            gameObject.SetActive(true);
            
            if (isAnimation) {
                ShowAnimation();
            } else {
                transform.localScale = Vector3.one;
            }

            _tile = tile;

            if (_tile.TileAction is TileEventAction) {
                _eventEmblem.Get(gameObject).gameObject.SetActive(true);
                _actionEmblem.Get(gameObject).gameObject.SetActive(false);
                _levelGuage.Get(gameObject).gameObject.SetActive(false);
            } else {
                _eventEmblem.Get(gameObject).gameObject.SetActive(false);
                _actionEmblem.Get(gameObject).gameObject.SetActive(true);
                _levelGuage.Get(gameObject).gameObject.SetActive(true);
            }
        }

        public void Hide(bool isAnimation = true) {
            _tile = null;

            if (isAnimation) {
                HideAnimation();
            } else {
                gameObject.SetActive(false);
            }
        }

        private void Update() {
            if (_tile == null) return;

            if (_tile.TileAction is TileEventAction) {
                ShowEventTileInfo();
            } else {
                ShowNormalTileInfo();
            }
        }

        private void ShowEventTileInfo() {
            _header.Get(gameObject).color = TileMagic.Data(TileMagicType.None).elementColor;

            _eventEmblem.Get(gameObject).sprite = 
                ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_BoardEvent + 
                (_tile.TileAction as TileEventAction).EventType);
            
            if (_eventEmblem.Get(gameObject).sprite == null) {
                _eventEmblem.Get(gameObject).color = Color.clear;
            } else {
                _eventEmblem.Get(gameObject).color = Color.white;
            }
        }

        private void ShowNormalTileInfo() {
            _header.Get(gameObject).color = TileMagic.Data(_tile.TileMagic.Type).elementColor;
            _expBar.Get(gameObject).color = TileMagic.Data(_tile.TileMagic.Type).elementColor;

            _expBar.Get(gameObject).transform.localScale = new Vector3(
                _tile.Exp / (float)Constants.GameSetting.Tile.LevelUpExp[_tile.Level], 
                1, 
                1
            );

            _levelText.Get(gameObject).text = $"Lv. {_tile.Level}";
            _expText.Get(gameObject).text = $"{_tile.Exp}/{Constants.GameSetting.Tile.LevelUpExp[_tile.Level]}";

            _actionEmblem.Get(gameObject).sprite = ResourceLoader.LoadSO<TileSymbolsSO>(
                Constants.FilePath.Resources.SO_TileSymbolsData
            )[_tile.Type, _tile.Level];
        }

        private void ShowAnimation() {
            transform.localScale = Vector3.zero;
            transform.DOScale(1, 0.4f).SetEase(Ease.OutBack);
        }

        private void HideAnimation() {
            transform.DOScale(0, 0.4f).SetEase(Ease.InBack).OnComplete(() => {
                gameObject.SetActive(false);
            });
        }
    }
}
