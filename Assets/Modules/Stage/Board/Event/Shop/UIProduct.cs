using System;
using Cardinals.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cardinals.BoardEvent.Shop
{
    public class UIProduct : MonoBehaviour, IDescription
    {
        private IProduct _item;
        
        [Header("Component")]
        [SerializeField] private Image _iconImg;
        [SerializeField] private TextMeshProUGUI _priceTMP;
        [SerializeField] private Button _buyBTN;
        [SerializeField] private GameObject _boughtObj;

        private bool _isBought;

        public bool IsBought
        {
            get => _isBought;
            set
            {
                _isBought = value;
                if (_isBought)
                {
                    _boughtObj.SetActive(true);
                    _buyBTN.interactable = false;
                }
            }
        }

        private Func<bool> _buyFunc;
        
        #region IDescription

        public string Name => _item?.Name;
        public string Description => _item?.Description;
        public Sprite IconSprite => null;
        public Transform InstTr => null;
        public Color Color { get; }
        public string Key { get; }

        #endregion

        private void Awake()
        {
            _buyBTN.onClick.AddListener(Buy);
        }

        public void Init(IProduct item, Func<bool> buyFunc)
        {
            _item = item;
            _buyFunc = buyFunc;
            
            _iconImg.sprite = _item.Sprite;
            _priceTMP.text = $"{_item.Price}";
            _boughtObj.SetActive(false);
        }
        
        private void Buy()
        {
            if (!IsBought)
            {
                if (_buyFunc())
                {
                    IsBought = true;
                }
            }
        }

        private void OnDestroy()
        {
            _item = null;
            _buyFunc = null;
        }
    }
}