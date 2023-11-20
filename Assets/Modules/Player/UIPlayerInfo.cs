using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Util;
using Sirenix.OdinInspector;
using Cardinals.Enums;

namespace Cardinals
{
    public class UIPlayerInfo : MonoBehaviour
    {
        [SerializeField] private Player _player;

        [Header("HP")]
        [SerializeField] private TextMeshProUGUI _hpTMP;
        [SerializeField] private RectTransform _maxHPRect;
        [SerializeField] private RectTransform _curHPRect;

        [Header("Buff")]
        [SerializeField] private Transform _buffTr;
        [SerializeField] private GameObject _buffPrefab;

        [Header("Panel")]
        [SerializeField] private GameObject _playerInfoPanel;
        [SerializeField] private float _panelMoveDistance;

        [Header("Potion")]
        [SerializeField] private Transform _potionTr;
        private bool _isPanelOpen;
        
        private void Start()
        {
            Init();
        }
        public void Init()
        {
            _player = GameManager.I.Player;
            _player.UpdateHpEvent += UpdateHp;
            _player.AddBuffEvent += AddBuff;
            _player.PlayerInfo.AddPotionEvent += AddPotion;
        }

        private void UpdateHp(int hp, int maxHp)
        {
            float percent = (float)hp / maxHp;
            _curHPRect.localScale = new Vector3(percent, 1, 1);
            _hpTMP.text= $"{hp}/{maxHp}";
        }

        private void AddBuff(BaseBuff baseBuff)
        {
            Instantiate(_buffPrefab, _buffTr);
        }
        
        private void AddPotion(int index, Potion potion)
        {
            GameObject potionUI =
                        GameObject.Instantiate(ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UIPotion), _potionTr);
            potionUI.GetComponent<UIPotion>().Init(index, potion.Name, potion);
        }

        public void OpenPanel()
        {
            if (_isPanelOpen)
            {
                _playerInfoPanel.transform.DOMoveX(-_panelMoveDistance, 0.3f).SetEase(Ease.InOutCubic);
                _isPanelOpen = false;
            }
            else
            {
                _playerInfoPanel.transform.DOMoveX(_panelMoveDistance, 0.3f).SetEase(Ease.InOutCubic);
                _isPanelOpen = true;
            }
        }

        [Button]
        public void TestPotion(PotionType potionType)
        {
            _player.PlayerInfo.AddPotion(potionType);
        }
    }
}

