using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cardinals.Board;
using Cardinals.Enums;
using Cardinals.Tutorial;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI {
    public class UIMagicLevelUpPanel: MonoBehaviour {
        private ComponentGetter<UIInitialMagicSelectPanel> _initialMagicSelectPanel
            = new ComponentGetter<UIInitialMagicSelectPanel>(
                TypeOfGetter.ChildByName,
                "Initial Magic Select Panel"
            );

        private ComponentGetter<UIPostMagicSelectPanel> _postMagicSelectPanel
            = new ComponentGetter<UIPostMagicSelectPanel>(
                TypeOfGetter.ChildByName,
                "Post Magic Select Panel"
            );

        private ComponentGetter<TextMeshProUGUI> _titleText = new ComponentGetter<TextMeshProUGUI>(
            TypeOfGetter.ChildByName,
            "Title Panel/Title Text"
        );

        public void Init() {
            gameObject.SetActive(false);

            _initialMagicSelectPanel.Get(gameObject).Init();
            _postMagicSelectPanel.Get(gameObject).Init();
        }

        public (Func<IEnumerator> Requester, Func<(TileMagicType newMagic, int newLevel)> Result) RequestTileLevelUp(
            TileMagicType originalMagicType,
            int originalLevel
        ) {
            if (originalLevel == 0) {
                return ReqeustInitialLevelUp();
            } else {
                return RequestPostLevelUp(originalMagicType, originalLevel);
            }
        }

        private (Func<IEnumerator> Requester, Func<(TileMagicType newMagic, int newLevel)> ResultFunc) ReqeustInitialLevelUp(
            TileMagicType excludeMagicType = TileMagicType.None
        ) {
            bool requestHandled = false;
            TileMagicType resultMagicType = TileMagicType.None;

            List<TileMagicType> excludes = new List<TileMagicType>() { 
                TileMagicType.None, 
                TileMagicType.Attack, 
                TileMagicType.Defence,
                TileMagicType.Wind,
            };
            
            if (excludeMagicType != TileMagicType.None) {
                excludes.Add(excludeMagicType);
            }

            List<TileMagicType> targetMagicTypes = PickMagicTypes(
                Constants.GameSetting.Player.InitialMagicPickNum, 
                excludes   
            );

            void OnClickMagicSlot(TileMagicType magicType) {
                requestHandled = true;
                resultMagicType = magicType;

                if (GameManager.I.Stage.CurEvent is TutorialEvent tutorialEvent) {
                    tutorialEvent.CheckMagicSelectQuest();
                }
            }

            IEnumerator Requester() {
                _titleText.Get(gameObject).text = "Enchantment!";
                TextAnimation(_titleText.Get(gameObject));

                gameObject.SetActive(true);
                _initialMagicSelectPanel.Get(gameObject).gameObject.SetActive(true);
                _postMagicSelectPanel.Get(gameObject).gameObject.SetActive(false);

                _initialMagicSelectPanel.Get(gameObject).Set(targetMagicTypes, OnClickMagicSlot);
                yield return new WaitUntil(() => requestHandled);
                gameObject.SetActive(false);
            }

            (TileMagicType newMagic, int newLevel) Result() {
                return (resultMagicType, 1);
            }

            return (Requester, Result);
        }

        private (Func<IEnumerator> Requester, Func<(TileMagicType newMagic, int newLevel)> ResultFunc) RequestPostLevelUp( 
            TileMagicType originalMagicType,
            int originalLevel
        ) {
            bool levelUpSelected = false;
            bool changeSelected = false;
            TileMagicType resultMagicType = TileMagicType.None;
            Func<(TileMagicType newMagic, int newLevel)> resultFunc = LevelUpResult;

            void OnClickLevelUp(TileMagicType magicType) {
                levelUpSelected = true;
                resultMagicType = magicType;
            }

            void OnClickChange() {
                changeSelected = true;
            }

            IEnumerator Requester() {
                _titleText.Get(gameObject).text = "Enchant Evolve!";
                TextAnimation(_titleText.Get(gameObject));
                
                gameObject.SetActive(true);
                _initialMagicSelectPanel.Get(gameObject).gameObject.SetActive(false);
                _postMagicSelectPanel.Get(gameObject).gameObject.SetActive(true);

                _postMagicSelectPanel.Get(gameObject).Set(
                    originalLevel,
                    originalMagicType,
                    OnClickLevelUp,
                    OnClickChange
                );
                yield return new WaitUntil(() => levelUpSelected || changeSelected);
                
                if (levelUpSelected) {
                    gameObject.SetActive(false);
                    yield break;
                }

                if (changeSelected) {
                    var initialLevelUp = ReqeustInitialLevelUp(originalMagicType);
                    resultFunc = initialLevelUp.ResultFunc;
                    yield return initialLevelUp.Requester();
                }
            }

            (TileMagicType newMagic, int newLevel) LevelUpResult() {
                return (resultMagicType, originalLevel + 1);
            }

            (TileMagicType newMagic, int newLevel) Result() {
                return resultFunc();
            }

            return (Requester, Result);
        }

        private List<TileMagicType> PickMagicTypes(int pickNum, List<TileMagicType> excludes) {
            List<TileMagicType> result = new List<TileMagicType>();

            List<TileMagicType> allMagicTypes = Enum.GetValues(typeof(TileMagicType)).Cast<TileMagicType>().ToList();

            foreach (var exclude in excludes) {
                allMagicTypes.Remove(exclude);
            }

            for (int i = 0; i < pickNum; i++) {
                int randomIndex = UnityEngine.Random.Range(0, allMagicTypes.Count);
                result.Add(allMagicTypes[randomIndex]);
                allMagicTypes.RemoveAt(randomIndex);
            }

            return result;
        }

        private void TextAnimation(TextMeshProUGUI text) {
            float yPos = text.rectTransform.anchoredPosition.y;
            text.rectTransform.anchoredPosition = 
                new Vector2(
                    text.rectTransform.anchoredPosition.x,
                    yPos + 100
                );
            text.rectTransform.DOAnchorPosY(
                yPos,
                0.5f
            ).SetEase(Ease.OutBack);
        }
    }
}

