using System.Collections;
using System.Collections.Generic;
using Cardinals;
using Cardinals.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff Data", menuName = "Cardinals/Buff Data")]
public class BuffDataSO : ScriptableObject
{
    public BuffType type;
    
    [Tooltip("버프 차감 방식")]
    public BuffCountDecreaseType decreaseType;
    
    public string buffName;
    public string description;
    public Sprite sprite;
}
