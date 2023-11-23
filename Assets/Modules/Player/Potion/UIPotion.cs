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

            if (potionType == PotionType.Empty) return;
            
            _potionDataSO = ResourceLoader.LoadSO<PotionDataSO>(
                Constants.FilePath.Resources.SO_PotionData + potionType
            );
            _potionIcon.Get(gameObject).sprite = _potionDataSO.sprite;

            _potionButton.Get(gameObject).onClick.AddListener(UsePotionUI);
        }

        public void Set(Potion potion)
        {
            if (potion == null || potion.PotionType == PotionType.Empty) {
                _potionDataSO = null;
                _potionIcon.Get(gameObject).sprite = null;
                _potionIcon.Get(gameObject).color = Color.clear;
                return;
            }

            _potionDataSO = ResourceLoader.LoadSO<PotionDataSO>(
                Constants.FilePath.Resources.SO_PotionData + potion.PotionType
            );
            _potionIcon.Get(gameObject).sprite = _potionDataSO.sprite;
            _potionIcon.Get(gameObject).color = Color.white;
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
