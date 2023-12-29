using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cardinals.Enums;
using Util;

namespace Cardinals
{
    public class UINewDiceInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("DiceInfo")]
        [SerializeField] private Image _infoPanel;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _info;

        [Header("DiceBuffInfo")]
        [SerializeField] private GameObject _buffInfoPanel;
        [SerializeField] private TextMeshProUGUI _buffTitle;
        [SerializeField] private TextMeshProUGUI _buffInfo;

        [SerializeField] private GridSizeUpdator _sizeUpdator;
        private Dice _dice;

        public void Init(Dice dice)
        {
            UpdateDiceInfo(dice);
            _dice = dice;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            SetDescriptionUIHovered();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetDescriptionUIRestored();
        }
        public void SetDescriptionUIHovered()
        {
            _infoPanel.gameObject.SetActive(true);

            if (_dice.DiceBuffType != BuffType.Empty)
            {
                _buffInfoPanel.SetActive(true);
            }

            _sizeUpdator.Resizing();
        }

        public void SetDescriptionUIRestored()
        {
            _infoPanel.gameObject.SetActive(false);
            _buffInfoPanel.SetActive(false);
        }
        private void UpdateDiceInfo(Dice dice)
        {
            DiceType type = dice.DiceType;
            DiceDataSO data = DiceDataSO.Data(type);
            _title.text = TMPUtils.CustomParse(data.title, true);
            _title.color = data.elementColor;
            _info.text = TMPUtils.CustomParse(data.information, true);
            _infoPanel.color = data.elementColor;

            BuffDataSO buffData = BuffDataSO.Data(dice.DiceBuffType);
            if (buffData != null)
            {
                string buffIcon = $"<debuff={dice.DiceBuffType.ToString()}> ";
                _buffTitle.text = TMPUtils.CustomParse(buffData.buffName, true);
                _buffInfo.text = buffData.Description;
            }
        }
    }
}

