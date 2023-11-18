
using Cardinals.Enums;
using Cardinals.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            
            _iconImg.sprite = null; // [TODO] 아이콘 지정 필요
            _nameTMP.text = reward.Type switch
            {
                RewardType.Gold => $"{reward.Value} Gold",
                RewardType.Potion => $"Potion({reward.Value})",
                RewardType.Card => $"{reward.Value} Card",
                RewardType.Artifact => $"Artifact({reward.Value})",
                _ => "??"
            };

            baseReward.DeleteEvent += () => { Destroy(gameObject); };
            
            GetComponent<Button>().onClick.AddListener(Get);
        }
        
        void Get()
        {
            Debug.Log($"{baseReward.Type} ({baseReward.Value}) 아이템이 추가 (사실은 안됨)");
            // [TODO] GameManager.I.Player.PlayerInfo // 인벤에 추가
            
            baseReward.Remove();
            baseReward = null;
        }
    }
}