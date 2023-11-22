using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using UnityEngine;
using Util;

namespace Cardinals.Board
{
    public class BlessTotem : MonoBehaviour
    {
        private BlessType _baseBless;
        [Header("Component")]
        private SpriteRenderer _totemRenderer;

        public void Awake()
        {
            _totemRenderer = GetComponentInChildren<SpriteRenderer>();
        } 

        public void Init(BlessType bless)
        {
            _baseBless = bless;

            var data = ResourceLoader.LoadSO<BlessDataSO>(Constants.FilePath.Resources.SO_BlessData + bless);
            _totemRenderer.sprite = data.totemSprite;
        }

        public void OnMouseDown()
        {
            GameManager.I.Stage.SelectedBless = _baseBless;
        }
    }
}
