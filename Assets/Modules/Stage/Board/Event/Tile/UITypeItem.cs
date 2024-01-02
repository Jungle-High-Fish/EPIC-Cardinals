using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.Game;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.BoardEvent.Tile
{
    public class UITypeItem : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField][ReadOnly] private TileMagicType _type;
        private TileMagicDataSO _data;
        
        [Header("Component")]
        private Image _typeIconImg;
        private Button _selectBTN;

        public void Init(TileMagicType type)
        {
            // 컴포넌트 설정
            _typeIconImg = GetComponentInChildren<Image>();
            _selectBTN = GetComponentInChildren<Button>();
            _selectBTN.onClick.AddListener(SelectType);
            
            // 데이타 설정
            _type = type;
            _data = ResourceLoader.LoadSO<TileMagicDataSO>(Constants.FilePath.Resources.SO_MagicData + _type);
            _typeIconImg.sprite = _data.sprite;
        }

        private void SelectType()
        {
            GameManager.I.UI.UITileEvent.SelectedMagicType = _type;
        }
    }
}
