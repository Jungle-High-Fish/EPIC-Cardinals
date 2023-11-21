using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Board;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    public class UITileSelection: MonoBehaviour {
        private ComponentGetter<TextMeshProUGUI> _titleText
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.Child, "Title");
        private ComponentGetter<TextMeshProUGUI> _descriptionText
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.Child, "Description");
        
        private ComponentGetter<Button> _confirmButton
            = new ComponentGetter<Button>(TypeOfGetter.Child, "Button Area/Confirm Button");
        private ComponentGetter<Button> _cancelButton
            = new ComponentGetter<Button>(TypeOfGetter.Child, "Button Area/Cancel Button");

        private bool _hasInitialized = false;
        private Func<List<Tile>> _selectedTilesFunc; 

        public void Init(
            Func<List<Tile>> selectedTilesFunc, 
            Action onConfirm, 
            Action onCancel, 
            string title="", 
            string description=""
        ) {
            _hasInitialized = true;
            gameObject.SetActive(true);
            
            _selectedTilesFunc = selectedTilesFunc;
            
            _titleText.Get(gameObject).text = title;
            _descriptionText.Get(gameObject).text = description;

            _confirmButton.Get(gameObject).onClick.AddListener(() => {
                Close();
                onConfirm?.Invoke();
            });

            _cancelButton.Get(gameObject).onClick.AddListener(() => {
                Close();
                onCancel?.Invoke();
            });
        }

        private void Update() {
            if (_hasInitialized == false) return;

            if (_selectedTilesFunc().IsNullOrEmpty()) {
                _confirmButton.Get(gameObject).interactable = false;
            } else {
                _confirmButton.Get(gameObject).interactable = true;
            }
        }

        private void Close() {
            _hasInitialized = false;
            gameObject.SetActive(false);
        }
    }
}