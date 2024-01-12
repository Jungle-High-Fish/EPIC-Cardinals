using Cardinals.Enums;
using System;
using System.Linq;
using Cardinals.UI.Description;
using Modules.Utils;
using Sirenix.Utilities;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Util;

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

        private bool _hasInit = false;
        private bool _updateOpt = false;

        public void Init(IDescription description)
        {
            _baseDescription = description; 
            
            // 폰트 설정
            if (GameManager.I.Localization.IsJapanese) {
                _nameTMP.font = ResourceLoader.LoadFont(Constants.FilePath.Resources.Fonts_ShipporiGothicB2);
                _descriptionTMP.font = ResourceLoader.LoadFont(Constants.FilePath.Resources.Fonts_ShipporiGothicB2);

                // _titleText.Get(gameObject).fontSize = _titleText.Get(gameObject).fontSize * 0.7f;

                
            }
            
            // 이름 및 설명 설정
            _nameTMP.text = description.Name;
            _descriptionTMP.text = description.Description;
            
            // 아이콘 설정
            Sprite icon = description.IconSprite;
            _iconImg.sprite = icon;
            _iconImg.gameObject.SetActive(icon != null);

            // 테두리 색상 설정
            Color color = description.Color == default ? Constants.Common.Colors.CardinalsBlack : description.Color;
            _nameTMP.color = color;
            _borderObjImg.color = color;

            transform.AddComponent<GridSizeUpdator>().Resizing();

            _hasInit = true;
            
        
            if (description is BlessDescription ||
                description is MagicDescription ||
                (description is BuffDescription && description.Key.Contains(BuffType.Burn.ToString())))
            {
                _updateOpt = true;
            }
        }

        private void Update() {
            if (!_hasInit) return;
            
            if (_updateOpt)
            {
                _descriptionTMP.text = _baseDescription.Description;
            }
        }
    }
}