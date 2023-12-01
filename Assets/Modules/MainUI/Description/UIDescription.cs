using System;
using System.Linq;
using Cardinals.UI.Description;
using Modules.Utils;
using Sirenix.Utilities;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Cardinals.UI
{
    public class UIDescription : MonoBehaviour
    {
        private IDescription _baseDescription;
        public IDescription BaseDesc => _baseDescription;
        
        [Header("Component")]
        [SerializeField] private VerticalLayoutGroup[] _verticalLayout;
        [SerializeField] private HorizontalLayoutGroup[] _horizontalLayout;
        [SerializeField] private ContentSizeFitter[] _fitters;
        
        [Header("Data - Component")]
        [SerializeField] private Image _iconImg;
        [SerializeField] private TextMeshProUGUI _nameTMP;
        [SerializeField] private TextMeshProUGUI _descriptionTMP;
        [SerializeField] private Image _borderObjImg;

        private bool _completeClear;
        public void Init(IDescription description)
        {
            _baseDescription = description; 
            
            // 이름 및 설명 설정
            _nameTMP.text = description.Name;
            _descriptionTMP.text = description.Description;
            
            // 아이콘 설정
            Sprite icon = description.IconSprite;
            _iconImg.sprite = icon;
            _iconImg.gameObject.SetActive(icon != null);

            // 테두리 색상 설정
            if (description.Color == default)
            {
                _borderObjImg.color = Constants.Common.Colors.CardinalsBlack;
            }
            else
            {
                _borderObjImg.color = description.Color;
            }
            
            transform.AddComponent<LayoutInitializer>().Clear();
        }
    }
}