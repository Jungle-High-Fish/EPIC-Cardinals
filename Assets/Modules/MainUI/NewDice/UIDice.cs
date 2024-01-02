using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI.NewDice
{
    public class UIDice : MonoBehaviour
    {
        public Dice Data { get; private set; }
        
        [Header("Info")]
        [SerializeField] private Image _mainImg;
        [SerializeField] private UINewDiceInfo _diceInfo;
        [SerializeField] private TextMeshProUGUI _diceNameTMP;
        [SerializeField] private Transform _valuesTr;
        
        public void Init(Dice dice)
        {
            Data = dice;
            
            Sprite sprite = ResourceLoader.LoadSprite($"Dice/Dice_{dice.DiceType}_{dice.DiceNumbers[5]}");

            _diceNameTMP.text = $"{dice.DiceType} Dice";
            _mainImg.sprite = sprite;

            for (int i = 0, cnt = _valuesTr.childCount; i < cnt; i++)
            {
                sprite = ResourceLoader.LoadSprite($"Dice/Dice_{dice.DiceType}_{dice.DiceNumbers[i]}");
                _valuesTr.GetChild(i).GetComponent<Image>().sprite = sprite;
            }
            if (transform.parent.gameObject.name == "CurrentDices") return;
            _diceInfo.Init(dice);
        }
    }
}