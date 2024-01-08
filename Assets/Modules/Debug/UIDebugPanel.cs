using Cardinals.Enums;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.Test {
    public class UIDebugPanel : MonoBehaviour
    {
        public bool IsShow => _isShow;

        private ComponentGetter<Button> _button 
            = new(TypeOfGetter.ChildByName, "Debug Panel/Close Button");
        private ComponentGetter<RectTransform> _debugComponentsParent 
            = new(TypeOfGetter.ChildByName, "Debug Panel/Scroll View/Viewport/Debug List");
        private ComponentGetter<UIDebugConsole> _debugConsole 
            = new(TypeOfGetter.ChildByName, "Debug Console Panel");

        private List<IDebugComponent> _debugComponents = new();
        private bool _isShow = false;

        public void Init() {
            _button.Get(gameObject).onClick.RemoveAllListeners();
            _button.Get(gameObject).onClick.AddListener(() => {
                Hide();
            });

            _debugConsole.Get(gameObject).Init();

            AddButton("메테오", () => {
                GameManager.I.Stage.Meteor();
            });

            AddDropdown("몬스터 교체", typeof(EnemyType), (Enum e) => {
                GameManager.I.Stage.Test_ChangeEnemy((EnemyType) e);
            });

            AddDropdown("이벤트 추가", typeof(NewBoardEventType), (Enum e) => {
                GameManager.I.Stage.GenerateNewBoardEvent((NewBoardEventType) e);
            });

            AddDropdown("포션 추가", typeof(PotionType), (Enum e) => {
                GameManager.I.Stage.Player.PlayerInfo.AddPotion((PotionType) e);
            });

            AddDropdown("축복 추가", typeof(BlessType), (Enum e) => {
                GameManager.I.Stage.Player.PlayerInfo.GetBless((BlessType) e);
            });

            AddDropdown("버프 추가", typeof(BuffType), (Enum e) => {
                GameManager.I.Stage.Player.AddBuff(EnumHelper.GetBuffByType((BuffType) e));
            });

            AddDropdown("적 버프 추가", typeof(BuffType), (Enum e) => {
                GameManager.I.Stage.Enemies.ForEach(en => en.AddBuff(EnumHelper.GetBuffByType((BuffType) e)));
            });

            AddInputField("이동", (string s) => {
                StartCoroutine(GameManager.I.Stage.Player.MoveTo(int.Parse(s), 0.4f));
            });

            AddInputField("돈 추가", (string s) => {
                GameManager.I.Stage.Player.PlayerInfo.AddGold(int.Parse(s));
            });

            AddInputField("힐", (string s) => {
                GameManager.I.Stage.Player.Heal(int.Parse(s));
            });

            AddInputField("데미지", (string s) => {
                GameManager.I.Stage.Player.Hit(int.Parse(s));
            });

            AddInputField("적 데미지", (string s) => {
                GameManager.I.Stage.Enemies.ForEach(e => e.Hit(int.Parse(s)));
            });

            AddInputField("적 힐", (string s) => {
                GameManager.I.Stage.Enemies.ForEach(e => e.Heal(int.Parse(s)));
            });

            AddInputField("경험치 1", (string s) => {
                GameManager.I.Stage.Board[int.Parse(s)].TileMagic.GainExp(1);
            });

            AddButton("도전과제 초기화", () => {
                SteamUserStats.ResetAll(true);
            });

            Hide();
        }

        public void Show() {
            _isShow = true;
            gameObject.SetActive(true);
        }

        public void Hide() {
            _isShow = false;
            gameObject.SetActive(false);
        }

        private void AddDropdown(string title, Type type, Action<Enum> action) {
            var dropdown = Instantiate(
                ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_DebugDropdown), 
                _debugComponentsParent.Get(gameObject)
            );
            dropdown.GetComponent<UIDebugDropdown>().Init(title, type, action);
            _debugComponents.Add(dropdown.GetComponent<UIDebugDropdown>());
        }

        private void AddInputField(string title, Action<string> action) {
            var inputField = Instantiate(
                ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_DebugInputField), 
                _debugComponentsParent.Get(gameObject)
            );
            inputField.GetComponent<UIDebugInputField>().Init(title, action);
            _debugComponents.Add(inputField.GetComponent<UIDebugInputField>());
        }

        private void AddButton(string title, Action action) {
            var button = Instantiate(
                ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_DebugButton), 
                _debugComponentsParent.Get(gameObject)
            );
            button.GetComponent<UIDebugButton>().Init(title, action);
            _debugComponents.Add(button.GetComponent<UIDebugButton>());
        }
    }
}