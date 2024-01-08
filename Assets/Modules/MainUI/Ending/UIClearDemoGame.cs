using Steamworks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Image = UnityEngine.UI.Image;

namespace Cardinals
{
    public class UIClearDemoGame : MonoBehaviour
    {
        [Header("Component")]
        [SerializeField] private TextMeshProUGUI _greetingsTMP;
        [SerializeField] private Image _illustImg;
        [SerializeField] private Button _titleBTN;

        public void Start()
        {
            _titleBTN.onClick.AddListener(GameManager.I.GoToTitle);
        }

        public void Init()
        {
            _greetingsTMP.text = "안녕하세요~ 하이피쉬입니다.\n앞으로 저희의 행보를 잘 지켜봐 달라규~";
            _illustImg.sprite = ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_UI_Ending_ClearIllust);
        }

        public void On()
        {
            GameManager.I.SteamHandler.TriggerAchievement("First_Clear");
            GameManager.I.GoToCredit();
        }
    }
}