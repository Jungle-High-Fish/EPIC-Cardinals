using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SelectedEffect : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Button _btn;
    
    [Header("Info")]
    [SerializeField][ReadOnly] private bool _isSelected;
    public bool IsSelected
    {
        set
        {
            if(_isSelected != value)
            {
                _isSelected = value;
                _effectEvent?.Invoke(_isSelected);
                OnSelectedEvent?.Invoke(_isSelected);
            }
        }
    }

    private Action<bool> _effectEvent;
    public Action<bool> OnSelectedEvent;
    
    [Header("Effect")]
    [SerializeField] private bool _outline;

    void Start()
    {
        // 버튼 설정
        _btn.onClick.AddListener(B_Click);
        
        // 설정에 따른 이벤트 구독
        if(_outline) _effectEvent += SetOutline;

        // 기본 값
        IsSelected = false;
    }

    void B_Click()
    {
        IsSelected = !_isSelected;
    }

    void SetOutline(bool value)
    {
        GetComponent<Outline>().enabled = value;
    }
}
