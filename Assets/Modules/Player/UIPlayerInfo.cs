using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

        private void Start()
        {
            Init();
        }
        public void Init()
        {
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
    }
}

