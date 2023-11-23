using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Cardinals.BoardEvent.Tile
{
    public class UITypeItem : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField][ReadOnly] private TileMagicType _type;
        
        [Header("Component")]
        private Image _typeIconImg;
        private Button _selectBTN;

        private void Awake()
        {
            _typeIconImg = GetComponentInChildren<Image>();
            _selectBTN = GetComponentInChildren<Button>();
            _selectBTN.onClick.AddListener(SelectType);
        }

        public void Init(TileMagicType type)
        {
            _type = type;
            _typeIconImg.sprite = null; // [TODO] 설정 필요
        }

        private void SelectType()
        {
            GameManager.I.UI.UITileEvent.SelectedMagicType = _type;
        }
    }
}
