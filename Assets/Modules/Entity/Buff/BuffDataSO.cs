using System.Collections;
using System.Collections.Generic;
using Cardinals;
using Cardinals.Enums;
using Cardinals.Constants;
using UnityEngine;

using Util;

[CreateAssetMenu(fileName = "Buff Data", menuName = "Cardinals/Buff Data")]
public class BuffDataSO : ScriptableObject
{
    public BuffType type;
    
    [Tooltip("버프 차감 방식")]
    public BuffCountDecreaseType decreaseType;
    
    public string buffName;
    public string description;
    public Sprite sprite;

    public Sprite effectSprite;

    public static BuffDataSO Data(BuffType type) {
        return ResourceLoader.LoadSO<BuffDataSO>(
            Cardinals.Constants.FilePath.Resources.SO_BuffData + type
        );
    }
}
