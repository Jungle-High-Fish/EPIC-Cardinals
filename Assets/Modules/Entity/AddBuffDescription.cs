using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddBuffDescription : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Image _iconImg;
    [SerializeField] private TextMeshProUGUI _TMP;

    public void Init(BuffDataSO data)
    {
        _iconImg.sprite = data.sprite;
        _TMP.text = data.buffName;

        GetComponent<GridSizeUpdator>().Resizing();
        
        _TMP.text = data.buffName;
        transform.DOLocalMoveY(100, 1.5f)
            .OnComplete(() => Destroy(gameObject));
    }
}
