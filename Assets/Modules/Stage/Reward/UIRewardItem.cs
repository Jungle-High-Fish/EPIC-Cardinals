
using System;
using Cardinals.Enums;
using Cardinals.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI
{
    public class UIRewardItem : MonoBehaviour
    {
        private Reward baseReward;

        [Header("Component")]
        [SerializeField] private Image _iconImg;
        [SerializeField] private TextMeshProUGUI _nameTMP;

        public void Init(Reward reward)
        {
            baseReward = reward;

            Sprite sprite = null;
            string text = null;
            
            switch (reward.Type)
            {
                case RewardType.Gold:
                    sprite = ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_UI_Coin); 
                    text = $"{baseReward.Value} Gold";
                    break;
                case RewardType.Potion:
                    var potion = EnumHelper.GetPotion((PotionType) baseReward.Value);
                    sprite = potion.Sprite;
                    text = potion.Name;
                    break;
                case RewardType.Artifact:
                    var artifact = EnumHelper.GetArtifact((ArtifactType) baseReward.Value);
                    sprite = artifact.Sprite;
                    text = artifact.Name;
                    break;
                case RewardType.RandomDice:
                    sprite = ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_UI_Dice); 
                    text = $"Dice";
                    break;
            }
            
            _iconImg.sprite = sprite;
            _nameTMP.text = text;


            baseReward.DeleteEvent += () => { Destroy(gameObject); };
            
            GetComponent<Button>().onClick.AddListener(Get);
        }

        void Get()
        {
            switch (baseReward.Type)
            {
                case RewardType.Gold:
                    GameManager.I.Stage.AddGold(baseReward.Value);
                    break;
                case RewardType.Potion:
                    GameManager.I.Stage.Player.PlayerInfo.AddPotion((PotionType) baseReward.Value);
                    break;
                case RewardType.Artifact:
                    GameManager.I.Stage.Player.PlayerInfo.AddArtifact((ArtifactType) baseReward.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            baseReward.Remove();
            baseReward = null;
        }
    }
}