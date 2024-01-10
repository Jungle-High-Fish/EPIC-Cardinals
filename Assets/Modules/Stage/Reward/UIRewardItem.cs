
using System;
using Cardinals.Enums;
using Cardinals.Game;
using Cardinals.UI.Description;
using TMPro;
using Unity.VisualScripting;
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
                    var pType = (PotionType)baseReward.Value;
                    var potion = EnumHelper.GetPotion(pType);
                    sprite = potion.Sprite;
                    text = TMPUtils.LocalizedText(potion.Name);
                    transform.AddComponent<PotionDescription>().Init(pType);
                    break;
                case RewardType.Artifact:
                    var aType = (ArtifactType)baseReward.Value;
                    var artifact = EnumHelper.GetArtifact(aType);
                    sprite = artifact.Sprite;
                    text = artifact.Name;
                    transform.AddComponent<ArtifactDescription>().Init(aType);
                    break;
                case RewardType.RandomDice:
                    sprite = ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_UI_Dice); 
                    text = $"Dice";
                    break;
                case RewardType.NextStageMap:
                    sprite = ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_UI_Icon_Map); 
                    text = $"Clear?";
                    break;
            }
            
            _iconImg.sprite = sprite;
            _nameTMP.text = text;


            baseReward.UI = this;
            
            GetComponent<Button>().onClick.AddListener(Get);
        }

        void Get()
        {
            switch (baseReward.Type)
            {
                case RewardType.Gold:
                    GameManager.I.Stage.AddGold(baseReward.Value);
                    PostGet();
                    break;
                case RewardType.Potion:
                    if (GameManager.I.Stage.Player.PlayerInfo.AddPotion((PotionType) baseReward.Value))
                    {
                        PostGet();
                    }
                    else
                    {
                        GetComponentInParent<UIRewardPanel>().PrintMessage(GameManager.I.Localization[LocalizationEnum.UI_ITEM_NO_MORE_ITEM]);
                    }
                    break;
                case RewardType.Artifact:
                    GameManager.I.Stage.Player.PlayerInfo.AddArtifact((ArtifactType) baseReward.Value);
                    PostGet();
                    break;
                case RewardType.RandomDice:
                    GameManager.I.UI.UINewDicePanel.Init((Dice)baseReward.Data, PostGet);
                    break;
                case RewardType.NextStageMap:
                    GameManager.I.UI.UIClearDemoGame.On();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void PostGet()
        {
            GameManager.I.Stage.RewardBox.RemoveItem(baseReward);
        }

        public void Remove()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}