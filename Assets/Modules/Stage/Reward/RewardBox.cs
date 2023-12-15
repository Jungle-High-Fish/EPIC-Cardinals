using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cardinals;
using Cardinals.Enums;
using Cardinals.Game;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RewardBox : MonoBehaviour
{
    private readonly List<Reward> _rewards = new();

    public void Init()
    {
        Disable();
    }

    void ClearList()
    {
        // 기존 항목들 제거
        for (int i = _rewards.Count - 1; i >= 0; i--)
        {
            _rewards[i].Remove();
        }
    }
    
    /// <summary>
    /// 전투 종료 시, 구체화된 보상을 설정
    /// </summary>
    public void Set(IEnumerable<Reward> rewards)
    {
        ClearList();
        gameObject.SetActive(true);
        foreach (var r in rewards)
        {
            switch (r.Type)
            {
                case RewardType.Potion:
                    r.Value = GetRandomPotion();
                    break;
                case RewardType.Artifact:
                    r.Value = GetRandomArtifact();
                    break;
                case RewardType.RandomDice:
                    r.Data = GameManager.I.Stage.GetRewardDice((EnemyGradeType)r.Value);
                    break;
                default: break;
            }
            
            _rewards.Add(r);

            r.DeleteEvent += () =>
            {
                _rewards.Remove(r);
                CheckEmpty();
            };
        }
        
        GameManager.I.UI.UIRewardPanel.Set(_rewards);

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 1)
            .SetEase(Ease.OutElastic);
    }

    /// <summary>
    /// 몬스터 등급별로 보상이 고정되어 사용
    /// </summary>
    /// <param name="grade"></param>
    public void SetByGrade(EnemyGradeType grade)
    {
        List<Reward> rewards = new();
        switch (grade)
        {
            case EnemyGradeType.Common :
                rewards.Add(new (RewardType.Gold, 2));
                rewards.Add(new (RewardType.RandomDice, (int)grade));
                break;
            case EnemyGradeType.Elite :
                rewards.Add(new (RewardType.Gold, 5));
                rewards.Add(new (RewardType.Potion));
                rewards.Add(new (RewardType.RandomDice, (int)grade));
                break;
            case EnemyGradeType.Boss :
                rewards.Add(new (RewardType.Gold, 8));
                rewards.Add(new (RewardType.Potion));
                rewards.Add(new (RewardType.RandomDice, (int)grade ));
                break;
        }

        Set(rewards);
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
        return Random.Range(1, Enum.GetNames(typeof(PotionType)).Length);
    }
    
    int GetRandomCard()
    {
        return 0;
    }
    
    // int[] GetRandomArtifact(int count)
    // {
    //     return new[]{0};
    // }
    
    int GetRandomArtifact()
    {
        return Random.Range(1, Enum.GetNames(typeof(ArtifactType)).Length);
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
