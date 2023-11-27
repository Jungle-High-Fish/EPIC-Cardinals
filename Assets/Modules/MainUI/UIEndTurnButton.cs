using System.Collections;
using System.Collections.Generic;
using Cardinals;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class UIEndTurnButton : MonoBehaviour
{   
    private ComponentGetter<Button> _button 
        = new ComponentGetter<Button>(TypeOfGetter.This);

    public void Init() {
        _button.Get(gameObject).onClick.AddListener(EndTurn);
    }

	public void EndTurn() {
        GameManager.I.Player.EndTurn();
        GameManager.I.Stage.CurEvent.IsSelect = true;
    }

    public void Activate() {
        gameObject.SetActive(true);
    }

    public void Deactivate() {
        gameObject.SetActive(false);
    }
}