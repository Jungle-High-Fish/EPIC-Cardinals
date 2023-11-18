using Cardinals.Game;
using Cardinals.UI;
using UnityEngine;
using Util;

namespace Cardinals
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UIStage _uiStage;
        public UIStage UIStage => _uiStage;
        
        private UIRewardPanel _uiRewardPanel;

        public UIRewardPanel UIRewardPanel
        {
            get
            {
                if (_uiRewardPanel == null)
                {
                    GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_RewardPanel);
                    _uiRewardPanel = Instantiate(prefab, transform).GetComponent<UIRewardPanel>();
                    _uiRewardPanel.Init();
                }

                return _uiRewardPanel;
            }
        }

    }
}