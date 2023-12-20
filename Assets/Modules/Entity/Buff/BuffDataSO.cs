using System.Collections;
using System.Collections.Generic;
using Cardinals;
using Cardinals.Enums;
using Cardinals.Constants;
using UnityEngine;

using Util;
using Cardinals.Game;
using Cardinals.Board;

[CreateAssetMenu(fileName = "Buff Data", menuName = "Cardinals/Buff Data")]
public class BuffDataSO : ScriptableObject
{
    public string Description {
        get {
            if (
                GameManager.I.Player != null && 
                GameManager.I.Player.PlayerInfo.CheckBlessExist(BlessType.BlessFire1)
            ) {
                int round = (GameManager.I.Stage.CurEvent is BattleEvent) ? (GameManager.I.Stage.CurEvent as BattleEvent).Round : 0;
                return TMPUtils.GetTextWithBless(
                    TMPUtils.CustomParse(description),
                    new Dictionary<BlessType, (string text, Color color)> {
                        { BlessType.BlessFire1, ($"+ {round * 2}", TileMagic.Data(TileMagicType.Fire).elementColor) }
                    }
                );
            } else {
                return TMPUtils.GetTextWithBless(
                    TMPUtils.CustomParse(description),
                    new Dictionary<BlessType, (string text, Color color)>()
                );
            }
        }
    }

    public BuffType type;
    
    [Tooltip("버프 차감 방식")]
    public BuffCountDecreaseType decreaseType;
    
    public string buffName;
    [SerializeField] private string description;
    public Sprite sprite;

    public Sprite effectSprite;

    public static BuffDataSO Data(BuffType type) {
        return ResourceLoader.LoadSO<BuffDataSO>(
            Cardinals.Constants.FilePath.Resources.SO_BuffData + type
        );
    }
}
