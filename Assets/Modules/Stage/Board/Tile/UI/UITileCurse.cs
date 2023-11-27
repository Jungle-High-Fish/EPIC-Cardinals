using System.Collections;
using System.Collections.Generic;
using Cardinals.Board;
using Cardinals.Enums;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    public class UITileCurse: MonoBehaviour {
        private ComponentGetter<Image> _curseEmblem = new ComponentGetter<Image>(
            TypeOfGetter.ChildByName, 
            "Curse Emblem"
        );

        private ComponentGetter<TextMeshProUGUI> _leftTurnText = new ComponentGetter<TextMeshProUGUI>(
            TypeOfGetter.ChildByName, 
            "Left Turn Text"
        );

        private Tile _tile;

        private IEnumerator _updateCurseUI;
        private bool _hasInit = false;
        
        public void Init(Tile tile) {
            _hasInit = true;

            _tile = tile;

            if (_updateCurseUI == null) {
                _updateCurseUI = UpdateCurseUI();
                StartCoroutine(_updateCurseUI);
            }
        }

        public IEnumerator UpdateCurseUI() {
            while (!_hasInit) {
                yield return null;
            }

            while (true) {
                if (_tile.TileCurse.IsActive) {
                    _curseEmblem.Get(gameObject).gameObject.SetActive(true);

                    _curseEmblem.Get(gameObject).sprite 
                        = TileCurseData.Data(_tile.TileCurse.Data.CurseType).sprite;

                    _leftTurnText.Get(gameObject).text = 
                        (_tile.TileCurse.Data.TargetTurn - _tile.TileCurse.PassedTurn).ToString();
                } else {
                    _curseEmblem.Get(gameObject).gameObject.SetActive(false);
                    _leftTurnText.Get(gameObject).text = "";
                }
                yield return null;
            }
        }

        private void OnEnable() {
            if (_updateCurseUI != null) {
                StartCoroutine(_updateCurseUI);
            } else {
                _updateCurseUI = UpdateCurseUI();
                StartCoroutine(_updateCurseUI);
            }
        }

        private void OnDisable() {
            if (_updateCurseUI != null) {
                StopCoroutine(_updateCurseUI);
            }
        }
    }
}
