using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Cardinals
{
    public class TitleManager : MonoBehaviour
    {
        [Header("Title Button")] [SerializeField] private Transform _titleTr;
        [SerializeField] private Button _newGameBTN;
        [SerializeField] private Button _optionBTN;
        [SerializeField] private Button _exitBTN;
        
        [Header("Character Select")]
        [SerializeField] private Transform _characterSelecterTr;
        [SerializeField] private Button _startBTN;
        [SerializeField] private Button _backBTN;
        [SerializeField] private float _zoomInPosY;
        [SerializeField] private float _zoomInCameraSize;
        [SerializeField] private Transform _charactersTr;
        private int curCharIdx = 0;
        
        private void Start()
        {
            _newGameBTN.onClick.AddListener(ChoiceCharacter);
            _startBTN.onClick.AddListener(NewGame);
            _exitBTN.onClick.AddListener(Exit);
            _optionBTN.onClick.AddListener(OpenOption);
            _backBTN.onClick.AddListener(Title);

            SetTileView(true);
        }

        void CameraSetting(bool zoomIn = false)
        {
            Camera.main.orthographicSize = zoomIn ? _zoomInCameraSize : 5;
            
            var curPos = Camera.main.transform.position;
            curPos.y = zoomIn ? _zoomInPosY : 1;
            Camera.main.transform.position = curPos;
        }
        
        void Title() => SetTileView(true);
        void ChoiceCharacter() => SetTileView(false);
        
        void SetTileView(bool isTitleMode)
        {
            _titleTr.gameObject.SetActive(isTitleMode);
            CameraSetting(!isTitleMode);
            _characterSelecterTr.gameObject.SetActive(!isTitleMode);
        }

        public void NextCharacter(bool right)
        {
            _charactersTr.GetChild(curCharIdx).gameObject.SetActive(false);
            
            curCharIdx += right ? 1 : -1;
            if (curCharIdx < 0) curCharIdx = _charactersTr.childCount - 1;
            curCharIdx %= _charactersTr.childCount;
            _charactersTr.GetChild(curCharIdx).gameObject.SetActive(true);
        }

        public void OpenOption()
        {
            GameManager.I.UI.GameSettingUI.Show();
        }

        void NewGame()
        {
            GameManager.I.GameStart();
        }

        void Exit()
        {
            Application.Quit();
        }
    }
}

