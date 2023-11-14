using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.Enums {

    public enum TileTypes {
        Empty,
        Attack,
        Defence
    }
    
    public enum StageEventType
    {
        Empty,
        Battle,
    }

    public enum EnemyType
    {
        Ignore,
        One, 
        Two,
        Three,
        Four,
        Boss
    }
    
    public enum EnemyActionType
    {
        Empty,
        Attack,
        Defense,
        AreaAttack,
        Buff, // 버프 / 디버프
        Magic, // 마법 ?
        Unknown, // 알 수 없음 ?
    }

    public enum BuffType
    {
        Empty,
        Burn,
        Weak,
        ElectricShock, // 감전
        Poison,
        Fracture // 골절
    }

    public enum BuffCountDecreaseType : int
    {
        Empty = 0,
        Turn,   // 턴 마다 감소
        Event,  // 한 사건 동안 유지 (이후 해제)
        Stage   // 한 게임 내내 유지 (이후 해제)
    }
}