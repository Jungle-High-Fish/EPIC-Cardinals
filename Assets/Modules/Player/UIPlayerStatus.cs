using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Entity.UI;
using Cardinals.Enums;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI
{
    public class UIPlayerStatus : MonoBehaviour
    {
        private Player _player;
        
        [Header("HP")]
        [SerializeField] private TextMeshProUGUI _hpTMP;
        [SerializeField] private RectTransform _maxHPRect;
        [SerializeField] private RectTransform _curHPRect;
        [SerializeField] private float _curHPEndPosX;

        [Header("Defense")]
        [SerializeField] private GameObject _defenseObj;
        [SerializeField] private TextMeshProUGUI _defenseTMP;

        [Header("Buff")]
        [SerializeField] private GameObject _buffPrefab;
        [SerializeField] private Transform _buffListArea;

        [Header("Action")]
        [SerializeField] private Transform _actionTr;
        [SerializeField] private Image _actionIconImg;
        
        [Header("Bubble")]
        [SerializeField] private Bubble _bubble;
        public void Init()
        {
            _player = GameManager.I.Player;
            _player.UpdateHpEvent += UpdateHp;
            _player.AddBuffEvent += AddBuff;
            _player.UpdateDefenseEvent += UpdateDefense;
            _player.UpdateActionEvent += UpdateAction;

            _player.Bubble = _bubble;
            _bubble.SetBubble(_player.BubbleText.start);
            
            UpdateHp(_player.Hp, _player.MaxHp);
            UpdateAction();
        }

        public void Update()
        {
            var player = GameManager.I.Player;
            if (player != null)
            {
                transform.position = Camera.main.WorldToScreenPoint(player.transform.position) ;
            }
        }

        private void UpdateHp(int hp, int maxHp)
        {
            Vector3 vector3 = _curHPRect.localScale;
            vector3.x = (float) hp / maxHp;
            _curHPRect.localScale = vector3;
            
            _hpTMP.text = $"{hp}/{maxHp}";
            _maxHPRect.DOPunchScale(new Vector3(.5f, .5f, 1), .1f, 1)
                      .OnComplete(() => {_maxHPRect.localScale = Vector3.one; });
        }

        private void UpdateDefense(int defense)
        {
            if (defense != 0)
            {
                _defenseObj.SetActive(true);
                _defenseObj.transform.DOPunchScale(new Vector3(.5f, .5f, 1), .3f, 2)
                                     .OnComplete(() => {_defenseObj.transform.localScale = Vector3.one; });
                _defenseTMP.text = defense.ToString();
            }
            else
            {
                if (_defenseObj.activeSelf)
                {
                    _defenseObj.transform.DOScale(0, .5f)
                        .SetEase(Ease.InOutElastic)
                        .OnComplete(() =>
                        {
                            _defenseObj.transform.localScale = Vector3.one;
                            _defenseObj.SetActive(false);
                        });
                }
            }
        }
        
        private void AddBuff(BaseBuff baseBuff)
        {
            Instantiate(_buffPrefab, _buffListArea).GetComponent<UIBuff>().Init(baseBuff);
        }


        private void UpdateAction(PlayerActionType type = PlayerActionType.None)
        {
            if (type == PlayerActionType.None || type == PlayerActionType.Cancel)
            {
                _actionTr.gameObject.SetActive(false);
            }
            else
            {
                _actionTr.gameObject.SetActive(true);
                _actionIconImg.sprite =
                    ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_Player_Action + type);

                _actionTr.localScale = Vector3.one;
                // _actionTr.DOPunchScale(new Vector3(.5f, .5f, 1), .3f, 2).SetEase(Ease.InBounce);
            }
        }
    }
}