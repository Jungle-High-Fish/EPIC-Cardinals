using Cardinals.Enums;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Game;
using Cardinals.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Util;

namespace Cardinals.UI
{
    public class UIRewardPanel : MonoBehaviour
    {
        [Header("Component")]
        [SerializeField] private Transform _rewardsTr;
        [SerializeField] private GameObject _rewardMsgObj;
        [SerializeField] private TextMeshProUGUI _rewardMsgTMP;
        [SerializeField] private TextMeshProUGUI _cancelBTNTMP;
        
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
            _cancelBTNTMP.text = GameManager.I.Localization.Get(LocalizationEnum.UI_CLOSE);
            
            if (GameManager.I.Localization.IsJapanese) {
                _rewardMsgTMP.font = ResourceLoader.LoadFont(Constants.FilePath.Resources.Fonts_ShipporiGothicB2);
                _cancelBTNTMP.font = ResourceLoader.LoadFont(Constants.FilePath.Resources.Fonts_ShipporiGothicB2);
                _rewardMsgTMP.fontSize *= 0.7f;
            }
            
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
            _rewardMsgObj.SetActive(false);
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
            // 초기화
            GameManager.I.Stage.RewardBox.ClearBox();
            ClearItem();
            
            // 설정
            Set(rewards);
            yield return new WaitForSeconds(.05f);
            On();

            _selectedCancel = false;
            yield return new WaitUntil(() => _selectedCancel || _rewardsTr.childCount == 0);

            ClearItem();
            gameObject.SetActive(false);
        }

        public void ClearItem()
        {
            for (int i = _rewardsTr.childCount - 1; i >= 0; i--)
            {
                _rewardsTr.GetChild(i).GetComponent<UIRewardItem>().Remove();
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

        private float timer = 0;
        public void PrintMessage(string msg)
        {
            _rewardMsgObj.SetActive(true);
            _rewardMsgTMP.text = msg;
            _rewardMsgObj.GetComponent<GridSizeUpdator>().Resizing();
            
            StartCoroutine(OffMsgObj());
        }

        IEnumerator OffMsgObj()
        {
            timer = 1.5f;
            while (timer > 0)
            {
                yield return new WaitForSeconds(.1f);
                timer-= .1f;
            }
            _rewardMsgObj.SetActive(false);
        }

        public void OffMsgPanel()
        {
            timer = 0;
        }
        
    }

}
