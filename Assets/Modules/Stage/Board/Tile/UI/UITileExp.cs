using System.Collections;
using System.Collections.Generic;
using Cardinals.Board;
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

        private Tile _tile;

        private IEnumerator _updateLevelUI;
        private bool _hasInit = false;
        
        public void Init(Tile tile) {
            _hasInit = true;

            _tile = tile;
            _expBar.Get(gameObject).transform.localScale = new Vector3(0, 1, 1);

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
                float expRatio = _tile.Exp / (float)Constants.GameSetting.Tile.LevelUpExp[_tile.Level];
                _expBar.Get(gameObject).transform.localScale = new Vector3(expRatio, 1, 1);
                _expBar.Get(gameObject).color = TileMagic.Data(_tile.TileMagic.Type).elementColor;

                _actionEmblem.Get(gameObject).sprite 
                    = ResourceLoader.LoadSO<TileSymbolsSO>(Constants.FilePath.Resources.SO_TileSymbolsData)[_tile.Type, _tile.Level];
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
    }
}
