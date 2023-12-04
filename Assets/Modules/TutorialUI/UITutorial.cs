using System.Collections;
using System.Collections.Generic;
using Cardinals.Tutorial;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Util;

namespace Cardinals.UI {
    public class UITutorial: MonoBehaviour {
        private ComponentGetter<TextMeshProUGUI> _tutorialTitle
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Title");
        private ComponentGetter<RectTransform> _questArea
            = new ComponentGetter<RectTransform>(TypeOfGetter.ChildByName, "QuestArea");
        
        private List<UIQuest> _quests = new List<UIQuest>();

        [Button]
        public void Init(TutorialDataSO tutorialData) {
            gameObject.SetActive(true);

            Clear();

            _tutorialTitle.Get(gameObject).text = tutorialData.Title;

            Vector2 newSize = new Vector2(
                (transform as RectTransform).sizeDelta.x,
                tutorialData.Quests.Length * 85 + 110f
            );

            IEnumerator InitQuests(float t) {
                foreach (var quest in tutorialData.Quests) {
                    var questUI = InstantiateQuest(quest);
                    _quests.Add(questUI);

                    yield return new WaitForSeconds(t);
                }
            }

            StartCoroutine(InitQuests(0.2f));
            (transform as RectTransform).DOSizeDelta(newSize, 0.2f * tutorialData.Quests.Length).SetEase(Ease.OutCubic);
        }

        [Button]
        public (bool hasClear, int nextIdx) AchieveQuest(int index, int count) {
            bool clear = _quests[index].Achieve(count);
            if (clear) {
                if (index == _quests.Count - 1) {
                    return (true, -1);
                } else {
                    return (true, index + 1);
                }
            } else {
                return (false, index);
            }
        }

        public void ShowEndTurnQuest(bool isForNextEvent) {
            Vector2 newSize = new Vector2(
                (transform as RectTransform).sizeDelta.x,
                (transform as RectTransform).sizeDelta.y + 85
            );

            IEnumerator ShowQuest(float t) {
                yield return new WaitForSeconds(t);
                var questUI = InstantiateQuest(QuestData.EndTurnQuest());
                _quests.Add(questUI);

                GameManager.I.UI.UIEndTurnButton.Activate(isForNextEvent);
            }

            (transform as RectTransform).DOSizeDelta(newSize, 0.3f).SetEase(Ease.OutCubic);
            StartCoroutine(ShowQuest(0.25f));
        }

        private void Clear() {
            foreach (var quest in _quests) {
                Destroy(quest.gameObject);
            }
            _quests.Clear();
        }

        private UIQuest InstantiateQuest(QuestData quest) {
            var questObj = Instantiate(
                ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_TutorialQuest), 
                _questArea.Get(gameObject)
            );
            var questUI = questObj.GetComponent<UIQuest>();
            questUI.Init(quest);

            return questUI;
        }
    }
}

