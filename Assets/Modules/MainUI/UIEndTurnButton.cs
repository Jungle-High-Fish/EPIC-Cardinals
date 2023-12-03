using System.Collections;
using System.Collections.Generic;
using Cardinals;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class UIEndTurnButton : MonoBehaviour
{   
    private ComponentGetter<Button> _button 
        = new ComponentGetter<Button>(TypeOfGetter.Child);

    public void Init() {
        _button.Get(gameObject).onClick.AddListener(EndTurn);
    }

	public void EndTurn() {
        GameManager.I.Next();
        GameManager.I.Stage.CurEvent.IsSelect = true;
    }

    public void Activate() {
        _button.Get(gameObject).interactable = true;
    }

    public void Deactivate() {
        _button.Get(gameObject).interactable = false;
    }
}