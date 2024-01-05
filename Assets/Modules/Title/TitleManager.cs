using Cardinals.Title;
using Cardinals.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Cardinals
{
    public class TitleManager : MonoBehaviour
    {
        public UILoading LoadingUI => _loadingUI;

        [Header("Title Button")] [SerializeField] private Transform _titleTr;
        [SerializeField] private Button _newGameBTN;
        [SerializeField] private Button _loadGameBTN;
        [SerializeField] private Button _optionBTN;
        [SerializeField] private Button _exitBTN;
        
        [Header("Character Select")]
        [SerializeField] private Transform _characterSelecterTr;
        [SerializeField] private Button _startBTN;
        [SerializeField] private Button _backBTN;
        [SerializeField] private float _zoomInPosY;
        [SerializeField] private float _zoomInCameraSize;
        [SerializeField] private Transform _charactersTr;

        [SerializeField] private TMP_Text _characterDiceSelecterText;
        [SerializeField] private Transform _diceSelecterTr;

        [SerializeField] private TitleTutorialCheck _titleTutorialCheck;

        [Header("New Title Info ")]
        [SerializeField] private TileMaker _tileMaker;
        [SerializeField] private PlayerControlInTitle _playerControlInTitle;
        public PlayerControlInTitle PlayerControlInTitle => _playerControlInTitle;

        [Header("Loading UI")]
        [SerializeField] private UILoading _loadingUI;
        
        
        private int curCharIdx = 0;

        private void Start()
        {
            _newGameBTN.onClick.AddListener(ChoiceCharacter);
            _newGameBTN.GetComponentInChildren<TextMeshProUGUI>().text
                = GameManager.I.Localization[LocalizationEnum.UI_MAIN_NEW_START];

            _loadGameBTN.onClick.AddListener(() =>
            {
                GameManager.I.Sound.TitleButtonClick();
                GameManager.I.UI.SaveFileLoaderPanel.Show(
                    GameManager.I.SaveSystem.GetLocalSaveFileList(),
                    GameManager.I.SaveSystem.GetCloudSaveFileList()
                    );
            });
            _loadGameBTN.GetComponentInChildren<TextMeshProUGUI>().text 
                = GameManager.I.Localization[LocalizationEnum.UI_MAIN_CONTINUE];

            _startBTN.onClick.AddListener(NewGame);
            _startBTN.GetComponentInChildren<TextMeshProUGUI>().text 
                = GameManager.I.Localization[LocalizationEnum.UI_MAIN_START];

            _exitBTN.onClick.AddListener(Exit);
            _exitBTN.GetComponentInChildren<TextMeshProUGUI>().text 
                = GameManager.I.Localization[LocalizationEnum.UI_EXIT_GAME];

            _optionBTN.onClick.AddListener(OpenOption);
            _optionBTN.GetComponentInChildren<TextMeshProUGUI>().text 
                = GameManager.I.Localization[LocalizationEnum.UI_GAMESETTING_TITLE];

            _backBTN.onClick.AddListener(() => {
                GameManager.I.Sound.TitleButtonClick();
                Title();
                });

            _backBTN.GetComponentInChildren<TextMeshProUGUI>().text 
                = GameManager.I.Localization[LocalizationEnum.UI_MAIN_BACK];

            _characterDiceSelecterText.text 
                = GameManager.I.Localization[LocalizationEnum.UI_INIT_DICE_SELECT];

            _titleTutorialCheck.Init();

            Dice[] dices = new Dice[]
            {
            new Dice(new List<int>(){ 1,1,2,2,3,3},Enums.DiceType.Normal),
            new Dice(new List<int>(){ 1,1,2,2,3,3},Enums.DiceType.Normal),
            new Dice(new List<int>(){ 1,1,2,2,3,3},Enums.DiceType.Normal),
            new Dice(new List<int>(){ 3,3,4,4,5,5},Enums.DiceType.Normal),
            new Dice(new List<int>(){ 3,3,4,4,5,5},Enums.DiceType.Normal),
            };

            for (int i = 0; i < _diceSelecterTr.childCount; i++)
            {
                _diceSelecterTr.GetChild(i).GetComponent<DiceTitleUI>().UpdateDiceUIinTitle(dices[i]);
            }
            SetTileView(true);

            _tileMaker.Init();
            _playerControlInTitle.StartFlow();
        }

        void CameraSetting(bool zoomIn = false)
        {
            Camera.main.orthographicSize = zoomIn ? _zoomInCameraSize : 5;
            
            var curPos = Camera.main.transform.position;
            curPos.x = zoomIn ? 4 : 0;
            curPos.y = zoomIn ? _zoomInPosY : 10;
            Camera.main.transform.position = curPos;
        }
        
        void Title() => SetTileView(true);
        void ChoiceCharacter() => SetTileView(false);
        
        void SetTileView(bool isTitleMode)
        {
            _titleTr.gameObject.SetActive(isTitleMode);
            if (isTitleMode == false)
            {
                GameManager.I.Sound.TitleButtonClick();
            }
            CameraSetting(!isTitleMode);
            _characterSelecterTr.gameObject.SetActive(!isTitleMode);

            
        }

        public void NextCharacter(bool right)
        {
            GameManager.I.Sound.TitleButtonClick();
            _charactersTr.GetChild(curCharIdx).gameObject.SetActive(false);
            
            curCharIdx += right ? 1 : -1;
            if (curCharIdx < 0) curCharIdx = _charactersTr.childCount - 1;
            curCharIdx %= _charactersTr.childCount;
            _charactersTr.GetChild(curCharIdx).gameObject.SetActive(true);
        }

        public void OpenOption()
        {
            GameManager.I.Sound.TitleButtonClick();
            GameManager.I.UI.GameSettingUI.Show();
        }

        void NewGame()
        {
            GameManager.I.Sound.TitleButtonClick();
            _titleTutorialCheck.Show(() => {
                GameManager.I.Sound.TitleButtonClick();
                GameManager.I.GameStart(skipTutorial: false);
            }, () => {
                GameManager.I.Sound.TitleButtonClick();
                GameManager.I.GameStart(skipTutorial: true);
            });
        }

        void Exit()
        {
            GameManager.I.Sound.TitleButtonClick();
            Application.Quit();
        }

        [Button]
        public void OnCredit()
        {
            GetComponent<CreditController>().ShowCredit();
        }
    }
}

