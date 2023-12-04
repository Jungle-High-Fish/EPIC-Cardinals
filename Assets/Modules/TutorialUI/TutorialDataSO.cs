using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.Tutorial {
    [CreateAssetMenu(fileName = "TutorialData", menuName = "Cardinals/Tutorial Data", order = 0)]
    public class TutorialDataSO: ScriptableObject
    {
        public string TutorialName;
        public string Title;
        public QuestData[] Quests;
    }

    [Serializable]
    public class QuestData {
        public string Title;
        public string Description;
        public int TargetQuests;
    }
}
