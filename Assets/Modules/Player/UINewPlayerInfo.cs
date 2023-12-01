using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Util;

namespace Cardinals.UI.Description
{
    public class UINewPlayerInfo : MonoBehaviour
    {
        [Header("Component")]
        [SerializeField] private TextMeshProUGUI _moneyTMP;
        [SerializeField] private Transform _artifactParentTr;
        [SerializeField] private Transform _potionParentTr;
        [SerializeField] private Transform _blessParentTr;

        private GameObject IconPrefab => ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Sprites_UI_IconPrefab);
        
        public void Start()
        {
            var player = GameManager.I.Player;
            player.PlayerInfo.UpdateGoldEvent += UpdateGold;
            // player.UpdateHpEvent += UpdateHp;
            // player.AddBuffEvent += AddBuff;
            // player.UpdateDefenseEvent += UpdateDefense;
        }

        void UpdateGold(int value)
        {
            _moneyTMP.text = value.ToString();
        }
        
        public void UpdateBless(BlessType type)
        {
            var sprite = ResourceLoader.LoadSO<BlessDataSO>(Constants.FilePath.Resources.SO_BlessData + type).patternSprite;
            var obj = InstantiateIcon(sprite, _blessParentTr);
            obj.AddComponent<BlessDescription>().Init(type); // 설명창 추가
        }
        
        public void UpdatePotion(PotionType type)
        {
            var sprite = ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_Potion + type);
            var obj = InstantiateIcon(sprite, _potionParentTr);
            obj.AddComponent<PotionDescription>().Init(type); // 설명창 추가
        }
        
        public void UpdateArtifact(ArtifactType type)
        {
            var sprite = ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_Artifact + type);
            var obj = InstantiateIcon(sprite, _artifactParentTr);
            obj.AddComponent<ArtifactDescription>().Init(type); // 설명창 추가
        }

        private GameObject InstantiateIcon(Sprite sprite, Transform parent)
        {
            var obj = Instantiate(IconPrefab, parent);
            obj.GetComponent<UIIcon>().Init(sprite);

            return obj;
        }
    }

   
}