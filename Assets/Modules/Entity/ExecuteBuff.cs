using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ExecuteBuff : MonoBehaviour
{
    [SerializeField] private Image _img;
    [SerializeField] private float _effectDuration;
    public void Init(Sprite sprite)
    {
        _img.sprite = sprite;
        
        _img.DOFade(0, _effectDuration);
        transform.DOPunchScale(Vector3.one, _effectDuration, 2, .1f)
            .OnComplete(() => { Destroy(gameObject); });
    }
}