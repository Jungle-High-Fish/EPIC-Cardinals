using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cardinals;
using Cardinals.Enums;
using Cardinals.Game;
using Unity.VisualScripting;
using UnityEngine;

public class RewardBox : MonoBehaviour
{
    private readonly List<Reward> _rewards = new();

    public void Init()
    {
        Disable();
    }
    
    /// <summary>
    /// 전투 종료 시, 구체화된 보상을 설정
    /// </summary>
    public void Set(IEnumerable<Reward> rewards)
    {
        gameObject.SetActive(true);
        
        // 기존 항목들 제거
        _rewards.ForEach(r => r.Remove());
        
        foreach (var r in rewards)
        {
            r.Value = r.Type switch
            {
                RewardType.Card => 0,
                RewardType.Potion => 0,
                RewardType.Artifact => 0,
                _ => r.Value
            };
            
            _rewards.Add(r);

            r.DeleteEvent += () =>
            {
                _rewards.Remove(r);
                CheckEmpty();
            };
        }
        
        GameManager.I.UI.UIRewardPanel.Set(_rewards);
    }

    /// <summary>
    /// 보상을 선택 완료하여 화면에서 보상 상자 치우기
    /// </summary>
    public void Disable()
    {
        gameObject.SetActive(false);
    }

    int GetRandomPotion()
    {
        return 0;
    }
    int GetRandomCard()
    {
        return 0;
    }
    
    int[] GetRandomArtifact(int count)
    {
        return new[]{0};
    }

    /// <summary>
    /// 모든 보상을 수령하는 경우 수행
    /// - 패널 닫기, 보물상자 제거
    /// </summary>
    void CheckEmpty()
    {
        if (_rewards.Count == 0)
        {
            GameManager.I.UI.UIRewardPanel.gameObject.SetActive(false);
            Disable();
        }
    }
    
    void OnMouseDown()
    {
        GameManager.I.UI.UIRewardPanel.On();
    }
}
