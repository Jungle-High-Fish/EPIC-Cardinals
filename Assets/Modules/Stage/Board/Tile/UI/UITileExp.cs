using System.Collections;
using System.Collections.Generic;
using Cardinals.Board;
using Cardinals.Enums;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    public class UITileExp: MonoBehaviour {
        private ComponentGetter<Image> _expBar = new ComponentGetter<Image>(
            TypeOfGetter.ChildByName, 
            "Bar"
        );

        private ComponentGetter<Image> _actionEmblem = new ComponentGetter<Image>(
            TypeOfGetter.ChildByName, 
            "Action Emblem"
        );

        private ComponentGetter<SpriteRenderer> _tileLevelEffectSprite = new ComponentGetter<SpriteRenderer>(
            TypeOfGetter.ChildByName, 
            "Tile Level Effect"
        );

        private Tile _tile;

        private IEnumerator _updateLevelUI;
        private bool _hasInit = false;

        private int _prevExp = 0;
        
        public void Init(Tile tile) {
            _hasInit = true;

            _tile = tile;
            _expBar.Get(gameObject).transform.localScale = new Vector3(0, 1, 1);

            _prevExp = _tile.Exp;

            if (_updateLevelUI == null) {
                _updateLevelUI = UpdateLevelUI();
                StartCoroutine(_updateLevelUI);
            }
        }

        public IEnumerator UpdateLevelUI() {
            while (!_hasInit) {
                yield return null;
            }

            while (true) {
                if (_prevExp != _tile.Exp) {
                    ShowExpChangeAnimation();
                }
                _prevExp = _tile.Exp;

                _expBar.Get(gameObject).color = TileMagic.Data(_tile.TileMagic.Type).elementColor;

                _actionEmblem.Get(gameObject).sprite 
                    = TileMagic.Data(_tile.TileMagic.Type).uiSprite;

                if (_tile.Level == 0 || _tile.TileState == TileState.Cursed) {
                    _tileLevelEffectSprite.Get(gameObject).gameObject.SetActive(false);
                } else {
                    _tileLevelEffectSprite.Get(gameObject).gameObject.SetActive(true);
                    _tileLevelEffectSprite.Get(gameObject).sprite 
                        = TileMagic.Data(_tile.TileMagic.Type).tileLevelEffectSprite[_tile.Level - 1];
                }
                yield return null;
            }
        }

        private void OnEnable() {
            if (_updateLevelUI != null) {
                StartCoroutine(_updateLevelUI);
            } else {
                _updateLevelUI = UpdateLevelUI();
                StartCoroutine(_updateLevelUI);
            }
        }

        private void OnDisable() {
            if (_updateLevelUI != null) {
                StopCoroutine(_updateLevelUI);
            }
        }

        private void ShowExpChangeAnimation() {
            float expRatio = _tile.Exp / (float)Constants.GameSetting.Tile.LevelUpExp[_tile.Level];
            float newScaleX = Mathf.Clamp(expRatio, 0, 1);

            (_expBar.Get(gameObject).transform as RectTransform).DOScaleX(newScaleX, 0.3f).SetEase(Ease.OutBack);
            //(transform as RectTransform).DOPunchScale(new Vector3(1.03f, 1.03f, 0), 0.3f, 1, 0f);
        }
    }
}
