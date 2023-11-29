using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cardinals.Enums;
using Cardinals.Game;
using Cardinals.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Random = UnityEngine.Random;

namespace Cardinals.BoardEvent.Shop
{
    public class UIShop : MonoBehaviour, IDescriptionInstTrInfo
    {
        [Header("Component")] 
        [SerializeField] private Transform _artifactParentTr;
        [SerializeField] private Transform _potionParentTr;
        private GameObject _shopItemPrefab;
        
        [Header("MessageBox")] 
        [SerializeField] private GameObject _msgBoxObj;
        [SerializeField] private TextMeshProUGUI _msgTMP;
        GameObject ShopItemPrefab
        {
            get
            {
                _shopItemPrefab ??= ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_BoardEvent_Product);
                return _shopItemPrefab;
            }
        }
        [SerializeField] private Button _closeBTN;
        
        [Header("IDescriptionIstTrInfo")]
        [SerializeField] private Transform _descriptionInstallTr;
        public Transform DescriptionInstTr => _descriptionInstallTr;

        public void Awake()
        {
            _closeBTN.onClick.AddListener(() => { gameObject.SetActive(false); });
        }

        [Button]
        public void Init(int artifactCnt = 2, int potionCnt = 3)
        {
            _msgBoxObj.SetActive(false);
            Clear();
            SetArtifact(artifactCnt);
            SetPotion(potionCnt);
            On();
        }

        /// <summary>
        /// 기존 항목들 제거
        /// </summary>
        void Clear()
        {
            _artifactParentTr.DestroyChildren();
            _potionParentTr.DestroyChildren();
        }


        /// <summary>
        /// 아티펙트 진열 (기존에 없는 항목으로 지정 필요)
        /// </summary>
        void SetArtifact(int count)
        {
            List<ArtifactType> list = new();
            for (int idx = 1, cnt = Enum.GetNames(typeof(ArtifactType)).Length; idx < cnt; idx++)
            {
                ArtifactType artifact = (ArtifactType)idx;
                if (!GameManager.I.Player.PlayerInfo.CheckArtifactExist((ArtifactType)idx))
                {
                    list.Add(artifact);
                }
            }
            
            for (int i = 0; i < count && i < list.Count(); i++)
            {
                var idx = Random.Range(0, list.Count());
                var artifact = EnumHelper.GetArtifact(list[idx]);
                list.RemoveAt(idx);
                
                var obj = Instantiate(ShopItemPrefab, _artifactParentTr);
                 obj.GetComponent<UIProduct>().Init(artifact, () => BuyArtifact(artifact));
            }
        }
        
        /// <summary>
        /// 포션 진열
        /// </summary>
        void SetPotion(int count)
        {
            for (int i = 0; i < count; i++)
            {
                PotionType potionType =  (PotionType)Random.Range(1, Enum.GetNames(typeof(PotionType)).Length);
                Potion potion = EnumHelper.GetPotion(potionType);
                
                var obj = Instantiate(_shopItemPrefab, _potionParentTr);
                obj.GetComponent<UIProduct>().Init(potion, () => BuyPotion(potion));
            }
        }

        public void On()
        {
            GameManager.I.UI.UIPlayerInfo.OpenPanel(); // 플레이어 창도 같이 띄워준다.
            gameObject.SetActive(true);   
        }

        bool BuyArtifact(Artifact artifact)
        {
            bool result = false;
            
            if (CheckPrice(artifact))
            {
                StartCoroutine(NotiMessage($"{artifact.Name} 구매 완료"));
                GameManager.I.Player.PlayerInfo.AddArtifact(artifact.Type);
                GameManager.I.Player.PlayerInfo.UseGold(artifact.Price);
                result = true;
            }

            return result;
        }

        bool BuyPotion(Potion potion)
        {
            bool result = false;
            
            if (CheckPrice(potion))
            {
                bool checkNullSlot = GameManager.I.Player.PlayerInfo.PotionList.Any(p => p == null);
                
                if (checkNullSlot)
                {
                    StartCoroutine(NotiMessage($"{potion.Name} 구매 완료"));
                    GameManager.I.Player.PlayerInfo.AddPotion(potion.PotionType);
                    GameManager.I.Player.PlayerInfo.UseGold(potion.Price);
                    result = true;
                }
                else
                {
                    StartCoroutine(NotiMessage("가방이 가득차 구매할 수 없습니다."));
                }
            }
            
            return result;
        }

        /// <summary>
        /// 현재 구매 가능한지 체크
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool CheckPrice(IProduct item)
        {
            bool result = true;
            
            if (GameManager.I.Stage.Player.PlayerInfo.Gold < item.Price)
            {
                StartCoroutine(NotiMessage("돈이 부족하여 구매할 수 없음"));
                result = false;
            }

            return result;
        }

        IEnumerator NotiMessage(string text, float showTime = 1.5f)
        {
            _msgTMP.text = text;
            _msgBoxObj.SetActive(true);
            yield return new WaitForSeconds(showTime);
            _msgBoxObj.SetActive(false);
        }
    }
}