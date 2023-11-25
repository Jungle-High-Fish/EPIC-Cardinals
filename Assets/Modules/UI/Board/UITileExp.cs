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

        private Tile _tile;

        private IEnumerator _updateExpBar;
        private bool _hasInit = false;
        
        public void Init(Tile tile) {
            _hasInit = true;

            _tile = tile;
            _expBar.Get(gameObject).transform.localScale = new Vector3(0, 1, 1);

            if (_updateExpBar == null) {
                _updateExpBar = UpdateExpBar();
                StartCoroutine(_updateExpBar);
            }
        }

        public IEnumerator UpdateExpBar() {
            while (!_hasInit) {
                yield return null;
            }

            while (true) {
                float expRatio = _tile.Exp / (float)Constants.GameSetting.Tile.LevelUpExp[_tile.Level];
                _expBar.Get(gameObject).transform.localScale = new Vector3(expRatio, 1, 1);
                _expBar.Get(gameObject).color = TileMagic.Data(_tile.TileMagic.Type).elementColor;
                yield return null;
            }
        }

        private void OnEnable() {
            if (_updateExpBar != null) {
                StartCoroutine(_updateExpBar);
            } else {
                _updateExpBar = UpdateExpBar();
                StartCoroutine(_updateExpBar);
            }
        }

        private void OnDisable() {
            if (_updateExpBar != null) {
                StopCoroutine(_updateExpBar);
            }
        }
    }
}
