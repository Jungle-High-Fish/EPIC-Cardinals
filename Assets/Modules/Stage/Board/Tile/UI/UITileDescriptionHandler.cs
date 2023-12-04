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

            if (_tile.TileAction is TileEventAction) {
                if ((_tile.TileAction as TileEventAction).EventType != BoardEventType.Empty) {
                    _descriptionPanels.Add(AddEventDescription());
                }
            }

            if (_tile.TileMagic != null) {
                TileMagicDataSO data = TileMagic.Data(_tile.TileMagic.Type);
                _descriptionPanels.Add(AddMagicDescription(data));

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

        private UITileDescription AddEventDescription() {
            BoardEventDataSO data = BoardEventDataSO.Data((_tile.TileAction as TileEventAction).EventType);

            string title = data.eventName;
            string description = data.eventDescription;
            Sprite icon = data.sprite;

            UITileDescription panel = InstantiateDescriptionPanel();
            (panel.transform as RectTransform).MatchWidthUpper(transform as RectTransform);
            panel.SetDescription(title, description, icon, true);

            return panel;
        }

        private UITileDescription AddMagicDescription(TileMagicDataSO data) {
            string title = data.elementName;
            string description = data.mainMagicDescription;
            Sprite icon = data.uiSprite;

            UITileDescription panel = InstantiateDescriptionPanel();
            (panel.transform as RectTransform).MatchWidthUpper(transform as RectTransform);
            panel.SetDescription(title, description, icon, true, data.elementColor);

            return panel;
        }

        private UITileDescription AddBuffDescription(BuffDataSO data) {
            string title = data.buffName;
            string description = data.description;
            Sprite icon = data.sprite;

            UITileDescription panel = InstantiateDescriptionPanel();
            (panel.transform as RectTransform).MatchWidthUpper(transform as RectTransform);
            panel.SetDescription(title, description, icon, true);

            return panel;
        }
    }
}
