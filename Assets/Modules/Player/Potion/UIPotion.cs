using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals
{
    public class UIPotion : MonoBehaviour
    {
        [SerializeField] private Image _potionIcon;
        private int _index;
        public void Init(int index,string name, Potion potion)
        {
            potion.DeletePotionEvent += DeletePotionUI;
            _index = index;
            _potionIcon.sprite = ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_Potion + name);
        }


        public void DeletePotionUI(Potion potion)
        {
            potion.DeletePotionEvent -= DeletePotionUI;
            Destroy(gameObject);
        }

        public void UsePotionUI()
        {
            GameManager.I.Player.PlayerInfo.UsePotion(_index);
        }
    }

}
