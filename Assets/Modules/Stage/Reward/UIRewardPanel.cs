using Cardinals.Enums;
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
                obj.GetComponent<UIRewardItem>().Init(reward);
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

        private bool _selectedCancel;
        public IEnumerator ActivePanelWaitCancel(IEnumerable<Reward> rewards)
        {
            ClearItem();
            Set(rewards);
            yield return new WaitForSeconds(.05f);
            On();

            _selectedCancel = false;
            yield return new WaitUntil(() => _selectedCancel || _rewardsTr.childCount == 0);
            
            gameObject.SetActive(false);
            ClearItem();
        }

        private void ClearItem()
        {
            for (int i = _rewardsTr.childCount - 1; i >= 0; i--)
            {
                Destroy(_rewardsTr.GetChild(i).gameObject);
            }
        }

        public void Off()
        {
            _selectedCancel = true;
        }

        public void UpdateSize()
        {
            GetComponent<GridSizeUpdator>().Resizing();
        }


        public IEnumerator GetRandomPotionEvent()
        {
            var potion =  GameManager.I.Stage.GetRandomPotion();
            var reward = new Reward(type: RewardType.Potion, value: (int)potion);
            yield return GameManager.I.UI.UIRewardPanel.ActivePanelWaitCancel(new List<Reward>(){reward});
        }
    }

}
