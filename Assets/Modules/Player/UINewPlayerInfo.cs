using System.Collections;
using System.Collections.Generic;
using Cardinals.Board;
using Cardinals.Enums;
using Cardinals.Game;
using Cardinals.Tutorial;
using DG.Tweening;
using Modules.Utils;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI.Description
{
    public class UINewPlayerInfo : MonoBehaviour
    {
        public UIEndTurnButton EndTurnButton => _endTurnButton.Get(gameObject);
        public UITileInfo TileInfo => _tileInfo.Get(gameObject);
        public UITurnRoundStatus TurnRoundStatus => _turnRoundStatus.Get(gameObject);

        [Header("Component")]
        [SerializeField] private TextMeshProUGUI _moneyTMP;
        [SerializeField] private Transform _artifactParentTr;
        [SerializeField] private Transform _potionParentTr;
        [SerializeField] private Transform _blessParentTr;

        [Header("TextObject")]
        [SerializeField] private TextMeshProUGUI _turnTMP;
        [SerializeField] private TextMeshProUGUI _lapTMP;
        [SerializeField] private TextMeshProUGUI _blessTMP;
        [SerializeField] private TextMeshProUGUI _potionTMP;
        [SerializeField] private TextMeshProUGUI _currenttileTMP;


        private GameObject IconPrefab => ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Sprites_UI_IconPrefab);
        
        private ComponentGetter<UIEndTurnButton> _endTurnButton 
            = new ComponentGetter<UIEndTurnButton>(TypeOfGetter.Child);
        private ComponentGetter<UITileInfo> _tileInfo 
            = new ComponentGetter<UITileInfo>(TypeOfGetter.Child);

        private ComponentGetter<UITurnRoundStatus> _turnRoundStatus 
            = new ComponentGetter<UITurnRoundStatus>(TypeOfGetter.Child);

        private ComponentGetter<UIMapButton> _mapButton 
            = new ComponentGetter<UIMapButton>(TypeOfGetter.Child);

        [SerializeField] private Button _settingButton;

        private void Start()
        {
            _turnTMP.text = GameManager.I.Localization[LocalizationEnum.UI_INGAME_TURN];
            _lapTMP.text = GameManager.I.Localization[LocalizationEnum.UI_INGAME_LAP];
            _blessTMP.text = GameManager.I.Localization[LocalizationEnum.UI_INGAME_BLESS];
            _potionTMP.text = GameManager.I.Localization[LocalizationEnum.UI_INGAME_POTION];
            _currenttileTMP.text = GameManager.I.Localization[LocalizationEnum.UI_INGAME_CURRENTTILE];
        }

        public void Init() {
            _tileInfo.Get(gameObject).gameObject.SetActive(false);
            _endTurnButton.Get(gameObject).Deactivate();
            _settingButton.onClick.AddListener(() => GameManager.I.OnSettingButton());
            // _mapButton.Get(gameObject).Init();
            // _mapButton.Get(gameObject).Deactivate();
        }

        public void Set()
        {
            ShowPanel();

            if (GameManager.I.Stage == null) return;
            if (GameManager.I.Stage.Player == null) return;
            
            var player = GameManager.I.Player;
            
            GameManager.I.Player.PlayerInfo.AddBlessEvent -= UpdateBlessUI;
            GameManager.I.Player.PlayerInfo.AddBlessEvent += UpdateBlessUI;

            GameManager.I.Player.PlayerInfo.AddArtifactEvent -= UpdateArtifactUI;
            GameManager.I.Player.PlayerInfo.AddArtifactEvent += UpdateArtifactUI;

            GameManager.I.Player.PlayerInfo.AddPotionEvent -= UpdatePotionUI;
            GameManager.I.Player.PlayerInfo.AddPotionEvent += UpdatePotionUI;
            GameManager.I.Player.PlayerInfo.DeletePotionEvent -= UpdatePotionUI;
            GameManager.I.Player.PlayerInfo.DeletePotionEvent += UpdatePotionUI;

            GameManager.I.Player.PlayerInfo.UpdateGoldEvent -= UpdateMoneyUI;
            GameManager.I.Player.PlayerInfo.UpdateGoldEvent += UpdateMoneyUI;
            
            GetComponent<ContentSizeFitter>().Update();
            InstantiatePotionUI();

            _endTurnButton.Get(gameObject).Init();
            _tileInfo.Get(gameObject).InitOnTile();

            _turnRoundStatus.Get(gameObject).Set(0, 0);
        }

        void UpdateMoneyUI(int value)
        {
            _moneyTMP.text = value.ToString();
        }
        
        void UpdateBlessUI(BlessType type)
        {
            var data = ResourceLoader.LoadSO<BlessDataSO>(Constants.FilePath.Resources.SO_BlessData + type);
            var sprite = data.patternSprite;
            var obj = InstantiateIcon(sprite, _blessParentTr, TileMagic.Data(data.relatedMagicType).elementColor);
            obj.AddComponent<BlessDescription>().Init(type); // 설명창 추가

            GameManager.I.Player.PlayerInfo.BlessEventDict[type] += obj.GetComponent<UIIcon>().EffectBless;
        }

        private List<UIPotion> _potionList = new();
        private void InstantiatePotionUI() {
            
            for (int i = 0; i < Constants.GameSetting.Player.MaxPotionCapacity; i++) {
                var obj = InstantiateIcon(null, _potionParentTr);
                var potionUI = obj.AddComponent<UIPotion>();
                potionUI.Init(i);
                _potionList.Add(potionUI);
            }
        }

        private void UpdatePotionUI(int index, Potion potion)
        {
            _potionList[index].Set(potion);//GameManager.I.Stage.Player.PlayerInfo.PotionList[index]);
        }
        
        void UpdateArtifactUI(Artifact artifact)
        {
            var obj = InstantiateIcon(artifact.Sprite, _artifactParentTr);
            obj.AddComponent<ArtifactDescription>().Init(artifact.Type); // 설명창 추가
        }

        private GameObject InstantiateIcon(Sprite sprite, Transform parent, Color? innerColor = null)
        {
            var obj = Instantiate(IconPrefab, parent);

            if (sprite == null)
            {
                obj.GetComponent<UIIcon>().Init();
            }
            else obj.GetComponent<UIIcon>().Init(sprite, innerColor);

            return obj;
        }

        private void ShowPanel() {
            gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
            var rect = transform as RectTransform;
            var curPos = rect.anchoredPosition;
            rect.anchoredPosition = new Vector2(curPos.x + rect.sizeDelta.x, curPos.y);
            rect.DOAnchorPosX(curPos.x, 1f).SetEase(Ease.OutQuart).OnComplete(() => {
                SetPosition();
            });
        }

        private void SetPosition() {
            var rect = transform as RectTransform;
            rect.MatchHeigthRightSide();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 370);
        }

        
    }
}