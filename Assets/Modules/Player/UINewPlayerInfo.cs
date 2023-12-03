using System.Collections;
using System.Collections.Generic;
using Cardinals.Board;
using Cardinals.Enums;
using Cardinals.Game;
using Modules.Utils;
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

        [Header("Component")]
        [SerializeField] private TextMeshProUGUI _moneyTMP;
        [SerializeField] private Transform _artifactParentTr;
        [SerializeField] private Transform _potionParentTr;
        [SerializeField] private Transform _blessParentTr;

        private GameObject IconPrefab => ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Sprites_UI_IconPrefab);
        
        private ComponentGetter<UIEndTurnButton> _endTurnButton 
            = new ComponentGetter<UIEndTurnButton>(TypeOfGetter.Child);
        private ComponentGetter<UITileInfo> _tileInfo 
            = new ComponentGetter<UITileInfo>(TypeOfGetter.Child);

        private ComponentGetter<UIMapButton> _mapButton 
            = new ComponentGetter<UIMapButton>(TypeOfGetter.Child);
        
        public void Init() {
            _tileInfo.Get(gameObject).gameObject.SetActive(false);
            _endTurnButton.Get(gameObject).Deactivate();
            _mapButton.Get(gameObject).Init();
            _mapButton.Get(gameObject).Deactivate();
        }

        public void Set()
        {
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

            _mapButton.Get(gameObject).Activate();
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
    }

   
}