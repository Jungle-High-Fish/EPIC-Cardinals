using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    private bool _isBlessFire1;
    public bool IsBlessFire1 // 그을린 상처
    {
        get => _isBlessFire1;
        set
        {
            _isBlessFire1 = value;
        }
    }
    private bool _isBlessFire2;
    public bool IsBlessFire2 // 메테오
    {
        get => _isBlessFire2;
        set
        {
            _isBlessFire2 = value;
        }
    }
    private bool _isBlessWater1;
    public bool IsBlessWater1 // 잔잔한 수면
    {
        get => _isBlessWater1;
        set
        {
            _isBlessWater1 = value;
        }
    }
    private bool _isBlessWater2;
    public bool IsBlessWater2 // 범람
    {
        get => _isBlessWater2;
        set
        {
            _isBlessWater2 = value;
        }
    }
    private bool _isBlessEarth1;
    public bool IsBlessEarth1 // 바위 잔해
    {
        get => _isBlessEarth1;
        set
        {
            _isBlessEarth1 = value;
        }
    }
    private bool _isBlessEarth2;
    public bool IsBlessEarth2 // 깨지지 않는 바위
    {
        get => _isBlessEarth2;
        set
        {
            _isBlessEarth2 = value;
        }
    }

    public int Gold { get; private set; }
    public void AddGold(int value)
    {
        
    }

    public void UseGold(int value)
    {
        
    }
}
