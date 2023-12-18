using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cardinals
{
    public class UIPlayerResultPanel : MonoBehaviour
    {
        [Header("data")]
        private string _turnCntHeaderText;
        private string _diceRollingCountHeaderText;
        private string _enemyExecuteCountHeaderText;
        private string _playTimeHeaderText;
        
        
        [Header("Header")]
        [SerializeField] private TextMeshProUGUI _turnCountHeaderTMP;
        [SerializeField] private TextMeshProUGUI _diceRollingCountHeaderTMP;
        [SerializeField] private TextMeshProUGUI _enemyExecuteCountHeaderTMP;
        [SerializeField] private TextMeshProUGUI _playTimeHeaderTMP;
        
        [Header("Component")]
        [SerializeField] private TextMeshProUGUI _turnCountTMP;
        [SerializeField] private TextMeshProUGUI _diceRollingCountTMP;
        [SerializeField] private TextMeshProUGUI _enemyExecuteCountTMP;
        [SerializeField] private TextMeshProUGUI _playTimeTMP;
        [SerializeField] private Button _titleBTN;
        [SerializeField] private Button _retryBTN;
        public void Start()
        {
            _titleBTN.onClick.AddListener(GameManager.I.MoveTitleScene);
            _retryBTN.onClick.AddListener(GameManager.I.Retry);
        }

        public void Init()
        {
            _turnCntHeaderText           = "돈 바퀴 수";
            _diceRollingCountHeaderText  = "주사위 사용한 횟수";
            _enemyExecuteCountHeaderText = "몬스터 처치 수";
            _playTimeHeaderText          = "플레이 타임";
        }
        
        public IEnumerator Set(int turnCnt, int diceRollCnt, int enemyExecuteCnt, ulong playTimeBySecond)
        {
            // 초기화
            _titleBTN.gameObject.SetActive(false);
            _retryBTN.gameObject.SetActive(false);

            gameObject.SetActive(true);
            _turnCountHeaderTMP.text         = string.Empty;
            _turnCountTMP.text               = string.Empty;
            _diceRollingCountHeaderTMP.text  = string.Empty;
            _diceRollingCountTMP.text        = string.Empty;
            _enemyExecuteCountHeaderTMP.text = string.Empty;
            _enemyExecuteCountTMP.text       = string.Empty;
            _playTimeHeaderTMP.text          = string.Empty;
            _playTimeTMP.text                = string.Empty;
            
            // 시작
            yield return new WaitForSeconds(.1f);
            
            _turnCountHeaderTMP.text         = _turnCntHeaderText;
            _turnCountTMP.text               = turnCnt.ToString();
            yield return new WaitForSeconds(.5f);
            _diceRollingCountHeaderTMP.text  = _diceRollingCountHeaderText;
            _diceRollingCountHeaderTMP.GetComponent<TypewriterByCharacter>().StartShowingText();
            _diceRollingCountTMP.text        = diceRollCnt.ToString();
            yield return new WaitForSeconds(.5f);
            _enemyExecuteCountHeaderTMP.text = _enemyExecuteCountHeaderText;
            _enemyExecuteCountTMP.text       = enemyExecuteCnt.ToString();
            yield return new WaitForSeconds(.5f);
            _playTimeHeaderTMP.text          = _playTimeHeaderText;
            _playTimeTMP.text                = $"{ playTimeBySecond / 60 }:{ playTimeBySecond % 60 }";
            
            yield return new WaitForSeconds(2f);
            _titleBTN.gameObject.SetActive(true);
            _retryBTN.gameObject.SetActive(true);
        }
    }
}