using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.Board;
using Cardinals.Game;
using UnityEngine;
using Util;

namespace Cardinals.UI {
    public class UITileDescriptionHandler: MonoBehaviour {
        private Tile _tile;
        private bool _isOnTile;

        private List<UITileDescription> _descriptionPanels 
            = new List<UITileDescription>();

        public void Init(Tile tile, bool isOnTile) {
            _tile = tile;
            _isOnTile = isOnTile;

            ClearDescriptionPanel();

            if (_tile.TileCurse.IsActive) {
                _descriptionPanels.Add(AddCurseDescription());
            }

            if (_tile.IsSealed) {
                _descriptionPanels.Add(AddSealedDescription());
            }

            if (_tile.HasEvent) {
                if (_tile.TileEvent.EventType != BoardEventType.Empty) {
                    _descriptionPanels.Add(AddEventDescription());
                }
            }

            if (_tile.TileMagic != null) {
                TileMagicDataSO data = TileMagic.Data(_tile.TileMagic.Type);
                _descriptionPanels.Add(AddMagicDescription(data, _tile.TileMagic.Level));

                if (data.hasBuffEffect) {
                    BuffDataSO buffData = BuffDataSO.Data(data.buffType);
                    _descriptionPanels.Add(AddBuffDescription(buffData));
                }
            }

            gameObject.SetActive(false);
        }

        public IEnumerator ShowPanels() {
            gameObject.SetActive(true);

            float gap = 10f;

            for (int i = 0; i < _descriptionPanels.Count; i++) {
                if (i == 0) {
                    _descriptionPanels[i].Show(0);
                    continue;
                }
                _descriptionPanels[i].Show(
                    (_descriptionPanels[i-1].transform as RectTransform).anchoredPosition.y, 
                    _descriptionPanels[i-1].Height + gap, 
                    0.3f
                );
                yield return new WaitForSeconds(0.3f);
            }
        }

        public void HidePanels() {
            foreach (UITileDescription panel in _descriptionPanels) {
                panel.gameObject.SetActive(false);
            }

            gameObject.SetActive(false);
        }

        private void ClearDescriptionPanel() {
            foreach (UITileDescription panel in _descriptionPanels) {
                Destroy(panel.gameObject);
            }
            _descriptionPanels.Clear();
        }

        private UITileDescription InstantiateDescriptionPanel() {
            GameObject prefab;
            if (_isOnTile) {
                prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_OnTileDescription);
            } else {
                prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_TileDescription);
            }
                
            GameObject panel = Instantiate(prefab, transform);

            if (_isOnTile) {
                return panel.GetComponent<UIOnTileDescription>();
            }
            return panel.GetComponent<UITileDescription>();
        }

        private UITileDescription AddCurseDescription() {
            TileCurseUIDataSO data = TileCurseData.Data(_tile.TileCurse.Data.CurseType);

            string title = data.curseName;
            string description = data.curseDescription;
            Sprite icon = data.sprite;

            UITileDescription panel = InstantiateDescriptionPanel();
            (panel.transform as RectTransform).MatchWidthUpper(transform as RectTransform);
            panel.SetDescription(title, description, icon, false);

            return panel;
        }

        private UITileDescription AddSealedDescription() {
            string title = "봉인된 타일";
            string description = "봉인된 타일에서는 행동이 불가능합니다.";
            Sprite icon = null;

            UITileDescription panel = InstantiateDescriptionPanel();
            (panel.transform as RectTransform).MatchWidthUpper(transform as RectTransform);
            panel.SetDescription(title, description, icon, false);

            return panel;
        }

        private UITileDescription AddEventDescription() {
            BoardEventDataSO data = BoardEventDataSO.Data(_tile.TileEvent.EventType);

            string title = data.eventName;
            string description = data.eventDescription;
            Sprite icon = data.sprite;

            UITileDescription panel = InstantiateDescriptionPanel();
            (panel.transform as RectTransform).MatchWidthUpper(transform as RectTransform);
            panel.SetDescription(title, description, icon, true);

            return panel;
        }

        private UITileDescription AddMagicDescription(TileMagicDataSO data, int level) {
            string title = data.elementName;
            if (data.magicType != TileMagicType.Attack && data.magicType != TileMagicType.Defence) {
                title += $"<b><size=60%><cspace=-0.1em> Lv.{level}</cspace></size></b>";
            }
            string description = TMPUtils.GetTextWithLevel(
                TMPUtils.CustomParse(data.mainMagicDescription),
                level,
                data.elementColor
            );
            Sprite icon = data.uiSprite;

            UITileDescription panel = InstantiateDescriptionPanel();
            (panel.transform as RectTransform).MatchWidthUpper(transform as RectTransform);
            panel.SetDescription(title, description, icon, true, data.elementColor);

            return panel;
        }

        private UITileDescription AddBuffDescription(BuffDataSO data) {
            string title = data.buffName;
            string description = data.Description;
            Sprite icon = data.sprite;

            UITileDescription panel = InstantiateDescriptionPanel();
            (panel.transform as RectTransform).MatchWidthUpper(transform as RectTransform);
            panel.SetDescription(title, description, icon, true);

            return panel;
        }
    }
}
