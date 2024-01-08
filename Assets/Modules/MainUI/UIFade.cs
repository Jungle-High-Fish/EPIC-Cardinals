using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIFade : MonoBehaviour
{
    [SerializeField] private Image _image;
    public IEnumerator FadeIn()
    {
        DontDestroyOnLoad(gameObject);
        Color trasparentBlack = new Color(0, 0, 0, 0);
        _image.color = trasparentBlack;
        _image.DOFade(1, 0.7f);
        yield return new WaitForSeconds(0.7f);
    }

    public IEnumerator FadeOut()
    {
        _image.color = Color.black;
        _image.DOFade(0, 0.7f);
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }
}
