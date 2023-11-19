using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private bool _isBless1;
    public bool IsBless1 // 그을린 상처
    {
        get => _isBless1;
        set
        {
            _isBless1 = value;
        }
    }
    private bool _isBless2;
    public bool IsBless2 // 메테오
    {
        get => _isBless2;
        set
        {
            _isBless2 = value;
        }
    }
    private bool _isBless3;
    public bool IsBless3 // 잔잔한 수면
    {
        get => _isBless3;
        set
        {
            _isBless3 = value;
        }
    }
    private bool _isBless4;
    public bool IsBless4 // 범람
    {
        get => _isBless4;
        set
        {
            _isBless4 = value;
        }
    }
    private bool _isBless5;
    public bool IsBless5    // 바위 잔해
    {
        get => _isBless5;
        set
        {
            _isBless5 = value;
        }
    }
    private bool _isBless6;
    public bool IsBless6 // 깨지지 않는 바위
    {
        get => _isBless6;
        set
        {
            _isBless6 = value;
        }
    }
}
