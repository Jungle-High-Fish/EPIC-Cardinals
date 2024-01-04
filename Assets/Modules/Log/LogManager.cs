using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.Log {
    public class LogManager
    {
        public struct LogData {
            public LogType type;
            public string message;
        }

        private List<LogData> _logs = new List<LogData>();

        public void Init() {
            Application.logMessageReceived -= OnLogMessageReceived;
            Application.logMessageReceived += OnLogMessageReceived;
        }

        private void OnLogMessageReceived(string condition, string stackTrace, LogType type) {
            _logs.Add(new LogData() {
                type = type,
                message = condition
            });
        }

        public void Log(string message, LogType type=LogType.Log) {
            _logs.Add(new LogData() {
                type = type,
                message = message
            });

            #if UNITY_EDITOR
            if (type == LogType.Error) {
                Debug.LogError(message);
            }
            else if (type == LogType.Warning) {
                Debug.LogWarning(message);
            }
            else {
                Debug.Log(message);
            }
            #endif
        }
    }
}
