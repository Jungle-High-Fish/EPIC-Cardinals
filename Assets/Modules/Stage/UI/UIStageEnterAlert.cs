using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Util;

namespace Cardinals.Game {
    public class UIStageEnterAlert: MonoBehaviour {
        private ComponentGetter<TextMeshProUGUI> _stageNameText
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Name");
        
        public IEnumerator Init(Stage stage) {
            
            // 폰트 설정
            if (GameManager.I.Localization.IsJapanese || GameManager.I.Localization.IsChinese) {
                _stageNameText.Get(gameObject).font = ResourceLoader.LoadFont(Constants.FilePath.Resources.Fonts_Soft_JP);
            }
            else
            {
                _stageNameText.Get(gameObject).font = ResourceLoader.LoadFont(Constants.FilePath.Resources.Fonts_Soft_Default);
            }
            
            _stageNameText.Get(gameObject).SetText(string.Empty);
            gameObject.SetActive(true);
            _stageNameText.Get(gameObject).SetLocalizedText(stage.Name);
            yield return new WaitForSeconds(1f);
            

            yield return new WaitForSeconds(3f);
        }
    }
}

