using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.Game;
using Cardinals.UI;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals
{
    public class UIPotion: MonoBehaviour, IDescription
    {
        public string Name => _potionDataSO.potionName;
        public string Description => _potionDataSO.description;
        public Sprite IconSprite => _potionDataSO.sprite;
        public Transform InstTr => transform;
        public Color Color { get; }
        public string Key { get; }

        private PotionDataSO _potionDataSO;
        private int _index;

        private ComponentGetter<Image> _potionIcon = new ComponentGetter<Image>(
            TypeOfGetter.ChildByName,
            "Icon"
        );

        private ComponentGetter<Button> _potionButton 
            = new ComponentGetter<Button>(TypeOfGetter.This);

        public void Init(int index, PotionType potionType=PotionType.Empty)
        {
            _index = index;
            _potionButton.Get(gameObject).onClick.AddListener(() => {
                UsePotionUI();
            });

            if (potionType == PotionType.Empty) return;
            
            _potionDataSO = ResourceLoader.LoadSO<PotionDataSO>(
                Constants.FilePath.Resources.SO_PotionData + potionType
            );
            _potionIcon.Get(gameObject).sprite = _potionDataSO.sprite;
        }

        public void Set(Potion potion)
        {
            if (potion == null || potion.PotionType == PotionType.Empty) {
                _potionDataSO = null;
                _potionIcon.Get(gameObject).sprite = null;
                _potionIcon.Get(gameObject).color = Color.clear;

                _potionButton.Get(gameObject).interactable = false;
                return;
            }

            _potionDataSO = ResourceLoader.LoadSO<PotionDataSO>(
                Constants.FilePath.Resources.SO_PotionData + potion.PotionType
            );
            _potionIcon.Get(gameObject).sprite = _potionDataSO.sprite;
            _potionIcon.Get(gameObject).color = Color.white;

            _potionButton.Get(gameObject).interactable = true;
        }

        public void DeletePotionUI(Potion potion)
        {
            Destroy(gameObject);
        }

        public void UsePotionUI()
        {
            GameManager.I.Player.PlayerInfo.UsePotion(_index);
        }
    }

}
