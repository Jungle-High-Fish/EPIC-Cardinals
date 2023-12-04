using System.Collections;
using System.Collections.Generic;
using Cardinals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class UIEndTurnButton : MonoBehaviour
{   
    private ComponentGetter<Button> _button 
        = new ComponentGetter<Button>(TypeOfGetter.Child);
    private ComponentGetter<TextMeshProUGUI> _text 
        = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.Child);

    public void Init() {
        _button.Get(gameObject).onClick.AddListener(EndTurn);
        _text.Get(gameObject).text = "턴 종료";
    }

	public void EndTurn() {
        GameManager.I.Next();
        GameManager.I.Stage.CurEvent.IsSelect = true;
    }

    public void Activate(bool isForNextEvent=false) {
        _button.Get(gameObject).interactable = true;

        if (isForNextEvent) {
            _text.Get(gameObject).text = "다음 이벤트";
        } else {
            _text.Get(gameObject).text = "턴 종료";
        }
    }

    public void Deactivate() {
        _button.Get(gameObject).interactable = false;
    }
}