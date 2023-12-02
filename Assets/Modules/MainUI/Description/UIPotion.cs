using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.Game;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Cardinals.UI.Description
{
    public class UIPotion : MonoBehaviour
    {
        private Potion _curPotion;
        private Button _button;
        private int _index;
        
        void Start()
        {
            _button = transform.AddComponent<Button>();
            _button.onClick.AddListener(Use);
        }

        public void Init(int index)
        {
            _index = index;
            GetComponent<UIIcon>().Init(null);
        }
        

        public void Set(Potion potion)
        {
            _curPotion = potion;
            transform.AddComponent<PotionDescription>().Init(potion.PotionType);
            GetComponent<UIIcon>().Init(potion.Sprite);
        }

        void Use()
        {
            if (_curPotion != null)
            {
                GameManager.I.Player.PlayerInfo.UsePotion(_index);
            
                GetComponent<UIIcon>().Init(null);
            
                var descs =  GetComponents<PotionDescription>();
                for (int i = descs.Length - 1; i >= 0; i--)
                {
                    Destroy(descs[i]);
                }
            
                GetComponent<DescriptionConnector>().OffPanel();
            }
            
        }
    }
}
