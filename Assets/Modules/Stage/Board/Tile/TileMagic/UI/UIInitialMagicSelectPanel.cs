using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Util;
using Cardinals.Enums;
using System;
using Unity.VisualScripting;

namespace Cardinals.UI {
    public class UIInitialMagicSelectPanel: MonoBehaviour {
        private ComponentGetter<Transform> _magicSlotArea = new ComponentGetter<Transform>(
            TypeOfGetter.ChildByName,
            "Magic Slot List"
        );
        private ComponentGetter<Button> _refreshButton = new ComponentGetter<Button>(
            TypeOfGetter.ChildByName,
            "Button Panel/Button"
        );
        private ComponentGetter<Image> _refreshButtonImage = new ComponentGetter<Image>(
            TypeOfGetter.ChildByName,
            "Button Panel/Button/Image"
        );

        private List<UILevelUpMagicSlot> _magicSlots = new List<UILevelUpMagicSlot>();

        public void Init() {
            gameObject.SetActive(false);
        }
        
        bool _hasRefreshed = false;

        public IEnumerator Set(
            List<TileMagicType> tileMagicTypes, 
            Action<TileMagicType> onClickMagicSlot,
            Func<List<TileMagicType>> onClickRefresh
        ) {
            Clear();
            _hasRefreshed = false;

            gameObject.SetActive(true);
            _refreshButton.Get(gameObject).onClick.RemoveAllListeners();
            SetRefreshButton(false);

            yield return new WaitForSeconds(0.3f);
            yield return InitMagicSlots(tileMagicTypes, onClickMagicSlot);

            if (GameManager.I.Stage.Player.PlayerInfo.Gold < 2) {
                SetRefreshButton(false);
            } else {
                SetRefreshButton(true);
            }
            _refreshButton.Get(gameObject).onClick.AddListener(() => {
                if (_hasRefreshed) return;
                _hasRefreshed = true;
                Clear();
                SetRefreshButton(false);

                StartCoroutine(InitMagicSlots(onClickRefresh(), onClickMagicSlot));
            });
        }

        private void Clear() {
            foreach (var magicSlot in _magicSlots) {
                Destroy(magicSlot.gameObject);
            }

            _magicSlots.Clear();
        }

        private IEnumerator InitMagicSlots(List<TileMagicType> tileMagicTypes, Action<TileMagicType> onClickMagicSlot) {
            foreach (var tileMagicType in tileMagicTypes) {
                var magicSlot = ResourceLoader.LoadPrefab(
                    Constants.FilePath.Resources.Prefabs_UI_MagicSlot
                );
                
                var magicSlotObj = Instantiate(magicSlot, _magicSlotArea.Get(gameObject));
                var magicSlotComp = magicSlotObj.GetComponent<UILevelUpMagicSlot>();
                magicSlotComp.Init(tileMagicType, (magicType) => {
                    gameObject.SetActive(false); // 선택 시 UI 비활성화
                    onClickMagicSlot(magicType);
                });
                _magicSlots.Add(magicSlotComp);

                yield return new WaitForSeconds(0.2f);
            }
        }

        private void SetRefreshButton(bool interactable) {
            _refreshButton.Get(gameObject).interactable = interactable;

            Color color = _refreshButtonImage.Get(gameObject).color;
            color.a = interactable ? 1f : 0.3f;
            _refreshButtonImage.Get(gameObject).color = color;
        }
    }
}

