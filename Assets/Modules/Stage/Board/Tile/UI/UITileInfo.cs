using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.Board;
using UnityEngine;
using UnityEngine.UI;
using Util;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace Cardinals.UI {
    public class UITileInfo: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
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

        ComponentGetter<UITileDescriptionHandler> _descriptionArea = new ComponentGetter<UITileDescriptionHandler>(
            TypeOfGetter.ChildByName, 
            "DescriptionArea"
        );

        private Tile _tile;

        private bool _isOnTile = true;
        private bool _isHovering = false;

        private BoardEventType _prevEventType = BoardEventType.Empty;
        private TileMagicType _prevMagicType = TileMagicType.None;
        int prevLevel = 0;

        private float _prevExp = 0;
        
        public void InitOnTile() {
            //Show(GameManager.I.Stage.Board[0]);
        }

        public void Show(Tile tile, bool isAnimation = true, bool isOnTile = true) {
            gameObject.SetActive(true);
            
            if (isAnimation) {
                ShowAnimation();
            } else {
                //transform.localScale = Vector3.one;
            }

            _tile = tile;

            if (_tile.HasEvent) {
                _eventEmblem.Get(gameObject).gameObject.SetActive(true);
                _actionEmblem.Get(gameObject).gameObject.SetActive(false);
                _levelGuage.Get(gameObject).gameObject.SetActive(false);
            } else {
                _eventEmblem.Get(gameObject).gameObject.SetActive(false);
                _actionEmblem.Get(gameObject).gameObject.SetActive(true);
                _levelGuage.Get(gameObject).gameObject.SetActive(true);

                _prevExp = _tile.Exp;
            }

            _descriptionArea.Get(gameObject).Init(_tile, isOnTile);
            _isOnTile = isOnTile;

            Canvas.ForceUpdateCanvases();
            StartCoroutine(_descriptionArea.Get(gameObject).ShowPanels());
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

            if (_tile.HasEvent) {
                ShowEventTileInfo();
            } else {
                ShowNormalTileInfo();
            }
        }

        private void OnEnable() {
            if (GameManager.I.Player != null) {
                GameManager.I.Player.PlayerInfo.AddBlessEvent -= OnAddBless;
                GameManager.I.Player.PlayerInfo.AddBlessEvent += OnAddBless;
            }

            if (GameManager.I.Stage != null) {
                GameManager.I.Stage.OnEventStart -= RefreshDescription;
                GameManager.I.Stage.OnEventStart += RefreshDescription;
            }
        }

        private void OnDisable() {
            if (GameManager.I == null) return;

            if (GameManager.I.Player != null) {
                GameManager.I.Player.PlayerInfo.AddBlessEvent -= OnAddBless;
            }

            if (GameManager.I.Stage != null) {
                GameManager.I.Stage.OnEventStart -= RefreshDescription;
            }
        }

        public void OnPointerEnter(PointerEventData eventData) {
            _isHovering = true;

            // if (_tile == null) return;
            // if (_canHover == false) return;

            // StartCoroutine(_descriptionArea.Get(gameObject).ShowPanels());
        }

        public void OnPointerExit(PointerEventData eventData) {
            _isHovering = false;
            // _descriptionArea.Get(gameObject).HidePanels();
        }

        private void ShowEventTileInfo() {
            bool needUpdateDescription = false;
            if (_prevEventType != _tile.TileEvent.EventType) {
                needUpdateDescription = true;
            }
            _prevEventType = _tile.TileEvent.EventType;

            _header.Get(gameObject).color = TileMagic.Data(TileMagicType.None).elementColor;

            _eventEmblem.Get(gameObject).sprite = 
                ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_BoardEvent + 
                _tile.TileEvent.EventType);
            
            if (_eventEmblem.Get(gameObject).sprite == null) {
                _eventEmblem.Get(gameObject).color = Color.clear;
            } else {
                _eventEmblem.Get(gameObject).color = Color.white;
            }

            if (needUpdateDescription) {
                RefreshDescription();
            }
        }

        private void ShowNormalTileInfo() {
            bool needUpdateDescription = false;
            if (_prevMagicType != _tile.TileMagic.Type || prevLevel != _tile.Level) {
                needUpdateDescription = true;
            }
            _prevMagicType = _tile.TileMagic.Type;
            prevLevel = _tile.Level;

            _header.Get(gameObject).color = TileMagic.Data(_tile.TileMagic.Type).elementColor;
            _expBar.Get(gameObject).color = TileMagic.Data(_tile.TileMagic.Type).elementColor;

            bool needUpdateExp = false;
            if (_prevExp != _tile.Exp) {
                needUpdateExp = true;
            }
            _prevExp = _tile.Exp;

            _levelText.Get(gameObject).text = $"Lv. {(_tile.Level < Constants.GameSetting.Tile.MaxLevel ? _tile.Level : "MAX")}";
            
            if (_tile.Level < Constants.GameSetting.Tile.MaxLevel) {
                _expText.Get(gameObject).text = $"{_tile.Exp}/{Constants.GameSetting.Tile.LevelUpExp[_tile.Level]}";
            } else {
                _expText.Get(gameObject).text = $"-/-";
            }

            _expBar.Get(gameObject).transform.localScale = new Vector3(
                _tile.Level < Constants.GameSetting.Tile.MaxLevel ?
                Mathf.Clamp(_tile.Exp / (float)Constants.GameSetting.Tile.LevelUpExp[_tile.Level], 0, 1f) :
                1f, 
                1f, 
                1f
            );

            _actionEmblem.Get(gameObject).sprite = TileMagic.Data(_tile.TileMagic.Type).uiSprite;

            if (needUpdateDescription) {
                RefreshDescription();
            }

            if (needUpdateExp) {
                UpdateExp();
            }
        }

        private void UpdateExp() {
            float expRatio;
            if (_tile.Level < Constants.GameSetting.Tile.MaxLevel) {
                expRatio = Mathf.Clamp(_tile.Exp / (float)Constants.GameSetting.Tile.LevelUpExp[_tile.Level], 0, 1);
            } else {
                expRatio = 1;
            }

            _expBar.Get(gameObject).transform
                .DOScaleX(expRatio, 0.4f)
                .SetEase(Ease.OutCubic);
            _levelGuage.Get(gameObject).transform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 3, 0);
        }

        private void OnAddBless(BlessType type) {
            RefreshDescription();
        }

        private void RefreshDescription() {
            _descriptionArea.Get(gameObject).Init(_tile, _isOnTile);

            StartCoroutine(_descriptionArea.Get(gameObject).ShowPanels());
        }

        private void ShowAnimation() {
            transform.localScale = Vector3.zero;
            transform.DOScale(.8f, 0.4f).SetEase(Ease.OutBack);
        }

        private void HideAnimation() {
            transform.DOScale(0, 0.4f).SetEase(Ease.InBack).OnComplete(() => {
                gameObject.SetActive(false);
            });
        }
    }
}
