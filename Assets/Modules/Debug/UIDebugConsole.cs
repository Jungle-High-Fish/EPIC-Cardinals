using Cardinals.Log;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.Test {
    public class UIDebugConsole : MonoBehaviour
    {
        private ComponentGetter<TMP_InputField> _inputField 
            = new(TypeOfGetter.Child);
        private ComponentGetter<TMP_Text> _logText 
            = new(TypeOfGetter.ChildByName, "Log Area/Viewport/Content/Text");
        
        private bool _hasInit = false;

        private int _logEndIndex = 0;

        public void Init() {
            _hasInit = true;

            _logText.Get(gameObject).text = "";
            _inputField.Get(gameObject).text = "";

            _inputField.Get(gameObject).onEndEdit.RemoveAllListeners();
            _inputField.Get(gameObject).onEndEdit.AddListener(OnInput);
        }

        private void OnInput(string cmd) {
            Debug.Log(cmd);
            _inputField.Get(gameObject).text = "";
        }

        private void Update() {
            if (!_hasInit) return;

            if (GameManager.I == null) return;

            if (GameManager.I.LogManager.LogCount > _logEndIndex) {
                PrintLog(GameManager.I.LogManager.Logs, _logEndIndex);
                _logEndIndex = GameManager.I.LogManager.LogCount;
            }
        }

        private void PrintLog(List<LogManager.LogData> logList, int startIndex) {
            string logText = _logText.Get(gameObject).text;

            Dictionary<LogType, Color> logColorDict = new Dictionary<LogType, Color>() {
                { LogType.Log, Color.white },
                { LogType.Warning, Color.yellow },
                { LogType.Error, Color.red },
                { LogType.Exception, Color.red },
                { LogType.Assert, Color.red }
            };

            for (int i = startIndex; i < logList.Count; i++) {
                logText += $"{i}: ";
                logText += $"<b><color=#{ColorUtility.ToHtmlStringRGB(logColorDict[logList[i].type])}>{logList[i].type}</color></b>";
                logText += $" - {DateTime.Now.ToString("hh:mm:ss")}\n";
                logText += $"{logList[i].message}\n";
                logText += "------------\n";
            }

            _logText.Get(gameObject).text = logText;

            gameObject.SetActive(false);
            gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
        }
    }
}