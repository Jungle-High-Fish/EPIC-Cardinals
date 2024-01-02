using System.Collections;
using System.Collections.Generic;
using Cardinals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;
public class UIDiceReroll : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rerollTmp;
    void Start()
    {
        _rerollTmp.text = GameManager.I.Localization[LocalizationEnum.UI_DICE_REROLL];
    }


}
