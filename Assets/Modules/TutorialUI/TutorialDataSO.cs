using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Cardinals.Tutorial {
    [CreateAssetMenu(fileName = "TutorialData", menuName = "Cardinals/Tutorial Data", order = 0)]
    public class TutorialDataSO: ScriptableObject
    {
        public string TutorialName;
        public string Title;
        public QuestData[] Quests;

        public int[] Cards;
    }

    [Serializable]
    public class QuestData {
        public string Title;

        public string Description;

        [OnValueChanged("SetCardList"), Range(1, 5)]
        public int TargetQuests;

        public TutorialQuestType QuestType;

        [OnValueChanged("SetCardList")]
        public bool HasCardSequence;

        [ShowIf("HasCardSequence")]
        public CardUseQuestData[] NeedCardSequence;

        public void SetCardList() {
            if (HasCardSequence) {
                NeedCardSequence = new CardUseQuestData[TargetQuests];
            }
        }

        public static QuestData EndTurnQuest() {
            return new QuestData {
                Title = "턴 종료",
                Description = "턴을 종료하세요.",
                TargetQuests = 1,
                QuestType = TutorialQuestType.EndTurn,
                HasCardSequence = false
            };
        }

        [Serializable]
        public class CardUseQuestData {
            public int CardNumber;
            public MouseState HowToUse;
        }
    }
}
