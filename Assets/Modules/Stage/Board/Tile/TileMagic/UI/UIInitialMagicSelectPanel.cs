using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Util;
using Cardinals.Enums;
using System;

namespace Cardinals.UI {
    public class UIInitialMagicSelectPanel: MonoBehaviour {
        private ComponentGetter<Transform> _magicSlotArea = new ComponentGetter<Transform>(
            TypeOfGetter.ChildByName,
            "Magic Slot List"
        );

        private List<UILevelUpMagicSlot> _magicSlots = new List<UILevelUpMagicSlot>();

        public void Init() {
            gameObject.SetActive(false);
        }

        public IEnumerator Set(List<TileMagicType> tileMagicTypes, Action<TileMagicType> onClickMagicSlot) {
            Clear();

            gameObject.SetActive(true);

            yield return new WaitForSeconds(0.3f);
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

        private void Clear() {
            foreach (var magicSlot in _magicSlots) {
                Destroy(magicSlot.gameObject);
            }

            _magicSlots.Clear();
        }
    }
}

