using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.BoardEvent.Shop
{
    public class UIShop : MonoBehaviour, IDescriptionInstTrInfo
    {
        [Header("Component")] 
        [SerializeField] private Transform _artifactParentTr;
        [SerializeField] private Transform _potionParentTr;
        private GameObject _shopItemPrefab;
        GameObject ShopItemPrefab
        {
            get
            {
                _shopItemPrefab ??= ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Product);
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
            for (int i = 0; i < count; i++)
            {
                Artifact artifact = null; // [TODO] 아티펙트 지정 (현재 소지하지않은 아티팩트에서..)
                
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
                Potion potion = null; // [TODO] 포션 지정 (랜덤)
                
                var obj = Instantiate(_shopItemPrefab, _artifactParentTr);
                obj.GetComponent<UIProduct>().Init(potion, () => BuyPotion(potion));
            }
        }

        public void On()
        {
            gameObject.SetActive(true);   
        }

        bool BuyArtifact(Artifact artifact)
        {
            if (CheckPrice(artifact))
            {
                GameManager.I.Stage.AddArtifact();
                GameManager.I.Stage.UseGold(artifact.Price);
            }

            return false;
        }

        bool BuyPotion(Potion potion)
        {
            if (CheckPrice(potion))
            {
                if (true)// [TODO] 가방이 차 있는지 추가로 체크 필요
                {
                    GameManager.I.Stage.AddPotion();
                    GameManager.I.Stage.UseGold(potion.Price);
                }
            }
            
            return false;
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
                Debug.Log("돈이 부족하여 구매할 수 없음");
                result = false;
            }

            return result;
        }
    }
    
    // TEMP
    public class Artifact : IProduct
    {
        public string Name { get; }
        public Sprite Sprite { get; }
        public string Description { get; }
        public int Price { get; }
    }

    public class Potion  : IProduct
    {
        public string Name { get; }
        public Sprite Sprite { get; }
        public string Description { get; }
        public int Price { get; }
    }
}