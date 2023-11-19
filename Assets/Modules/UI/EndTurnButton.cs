using System.Collections;
using System.Collections.Generic;
using Cardinals;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
	public void EndTurn() {
        GameManager.I.Player.EndTurn();
        GameManager.I.Stage.CurEvent.IsSelect = true;
    }
}