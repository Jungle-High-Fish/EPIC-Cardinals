using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

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
    }
}

