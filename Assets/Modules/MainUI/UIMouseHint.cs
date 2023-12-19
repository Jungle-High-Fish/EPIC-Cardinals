using System.Collections;
using System.Collections.Generic;
using Cardinals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class UIMouseHint : MonoBehaviour
{      
    [SerializeField] private Sprite sprite1;
    [SerializeField] private Sprite sprite2;

    private ComponentGetter<Image> _image 
        = new ComponentGetter<Image>(TypeOfGetter.This);

    public void Show() {
        gameObject.SetActive(true);
        _image.Get(gameObject).sprite = sprite1;
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    private void Update() {
        var canvasRectTransform = GameManager.I.UI.MainUICanvas.transform as RectTransform;
        transform.position = Input.mousePosition + new Vector3(0.5f, 0.5f);
    }

    private IEnumerator ChangeSprite() {
        while (true) {
            _image.Get(gameObject).sprite = sprite1;
            yield return new WaitForSeconds(0.5f);
            _image.Get(gameObject).sprite = sprite2;
            yield return new WaitForSeconds(0.5f);
        }
    }
}