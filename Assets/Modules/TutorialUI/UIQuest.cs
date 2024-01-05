using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.Tutorial;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Util;

namespace Cardinals.UI {
    public class UIQuest: MonoBehaviour {
        public TutorialQuestType QuestType => _questData.QuestType;

        private ComponentGetter<TextMeshProUGUI> _questTitle
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "QuestInfo/QuestTextArea/QuestTitle");
        private ComponentGetter<TextMeshProUGUI> _questDescription
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "QuestInfo/QuestTextArea/QuestDescription");
        private ComponentGetter<TextMeshProUGUI> _questCount
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "QuestInfo/QuestCount");
        
        private QuestData _questData;
        private int _currentCount;

        public void Init(in QuestData questData) {
            SetTextsTransparent();
            SetTextsOpaque(0.3f);

            _questData = questData;
            _currentCount = 0;

            _questTitle.Get(gameObject).SetLocalizedText(questData.Title);
            _questDescription.Get(gameObject).SetLocalizedText(questData.Description);
            _questCount.Get(gameObject).text = $"{_currentCount}/{questData.TargetQuests}";
        }

        public bool Achieve(int count) {
            
            _currentCount += count;
            _questCount.Get(gameObject).text = $"{_currentCount}/{_questData.TargetQuests}";

            if (_currentCount >= _questData.TargetQuests) {
                GameManager.I.Sound.TutorialClear();
                _currentCount = _questData.TargetQuests;

                _questTitle.Get(gameObject).color = Color.gray;
                _questDescription.Get(gameObject).color = Color.gray;
                _questCount.Get(gameObject).color = Color.gray;

                _questTitle.Get(gameObject).DOStrikethrough(0.3f);
                transform.DOPunchScale(Vector3.one * 0.7f, 1, 0);
                return true;
            } else {
                return false;
            }
        }

        private void SetTextsTransparent() {
            _questTitle.Get(gameObject).DOFade(0, 0);
            _questDescription.Get(gameObject).DOFade(0, 0);
            _questCount.Get(gameObject).DOFade(0, 0);
        }

        private void SetTextsOpaque(float t) {
            _questTitle.Get(gameObject).DOFade(1f, t);
            _questDescription.Get(gameObject).DOFade(1f, t);
            _questCount.Get(gameObject).DOFade(1f, t);
        }
    }
}

