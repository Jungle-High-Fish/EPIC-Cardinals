using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cardinals;
using Cardinals.Enums;
using Cardinals.Game;
using Cardinals.Tutorial;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RewardBox : MonoBehaviour
{
    private List<Reward> _rewards = new();

    public void Init()
    {
        Disable();
    }

    public void ClearBox()
    {
        // 기존 항목들 제거
        for (int i = _rewards.Count - 1; i >= 0; i--)
        {
            RemoveItem(_rewards[i]);
        }
    }

    /// <summary>
    /// 전투 종료 시, 구체화된 보상을 설정
    /// </summary>
    public void Set(IEnumerable<Reward> rewards)
    {
        if (GameManager.I.Stage.CurEvent is TutorialEvent tutorial) {
            GameManager.I.UI.UIEndTurnButton.Activate(true);
            GameManager.I.UI.UIEndTurnButton.Deactivate();
        }

        ClearBox();
        gameObject.SetActive(true);
        foreach (var r in rewards)
        {
            switch (r.Type)
            {
                case RewardType.Potion:
                    r.Value = (int) GameManager.I.Stage.GetRandomPotion();
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
        }
        
        GameManager.I.UI.UIRewardPanel.Set(_rewards);

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 1)
            .SetEase(Ease.OutElastic);
    }

    public void RemoveItem(Reward reward)
    {
        reward.Remove();
        if (_rewards.Contains(reward))
        {
            _rewards.Remove(reward);
        }
        GameManager.I.UI.UIRewardPanel.UpdateSize();
        
        // 패널 닫기
        if (_rewards.Count == 0)
        {
            var panel = GameManager.I.UI.UIRewardPanel.gameObject;
            if(panel.activeSelf) panel.SetActive(false);
            Disable();
        }
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
                rewards.Add(new (RewardType.Gold, 3));
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

    public void Disable()
    {
        gameObject.SetActive(false);
    }
    
    int GetRandomCard()
    {
        return 0;
    }
    
    int GetRandomArtifact()
    {
        return Random.Range(1, Enum.GetNames(typeof(ArtifactType)).Length);
    }
    
    void OnMouseDown()
    {
        if (GameManager.I.Stage.CurEvent is TutorialEvent tutorial) {
            tutorial.CheckRewardSelectQuest();
            GameManager.I.UI.UIEndTurnButton.Activate(true);
        }
        GameManager.I.Sound.BoxOpen();
        GameManager.I.UI.UIRewardPanel.On();
    }
    
}
