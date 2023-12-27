using System.Collections;
using System.Collections.Generic;
using Cardinals.Game;
using Cardinals.UI;
using DG.Tweening;
using UnityEngine;
using Util;

namespace Cardinals.UI
{
    public class UIRewardPanel : MonoBehaviour
    {
        [Header("Component")]
        [SerializeField] private Transform _rewardsTr;
        
        private GameObject _rewardItemPrefab;
    
        private GameObject RewardItemPrefab
        {
            get
            {
                _rewardItemPrefab ??= ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_RewardItem);
                return _rewardItemPrefab;
            }
        }

        public void Init()
        {
            gameObject.SetActive(false);
        }
        
        public void Set(IEnumerable<Reward> rewards)
        {
            foreach (var reward in rewards)
            {
                var obj = Instantiate(RewardItemPrefab, _rewardsTr);
                obj.GetComponent<Cardinals.UI.UIRewardItem>().Init(reward);
            }
        }

        public void On()
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                UpdateSize();
                
                transform.localScale = Vector3.zero;
                transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutCubic);
            }
        }

        public void UpdateSize()
        {
            GetComponent<GridSizeUpdator>().Resizing();
        }
    
        
    }

}
