using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cardinals.BoardEvent.Roulette
{
    public class UIRoulette : MonoBehaviour
    {
        private Roulette _roulette;
        [SerializeField] private Button _spinBTN;
        [SerializeField] private GameObject _resultObj;
        [SerializeField] private TextMeshProUGUI _resultTMP;

        Roulette Roulette
        {
            get
            {
                _roulette ??= GetComponent<Roulette>();
                return _roulette;
            }
        }

        private void Awake()
        {
            _spinBTN.onClick.AddListener(B_Spin);
        }

        public void Init()
        {
            gameObject.SetActive(true);
            _resultObj.SetActive(false);
        }

        private void B_Spin()
        {
            Roulette.Spin(Get);
        }

        private void Get(RoulettePieceDataSO data)
        {
            Debug.Log(data.type);
            switch (data.type)
            {
                case BoardEventRoulette.DrawCard:
                    GameManager.I.Stage.DrawCard(2);
                    break;
                case BoardEventRoulette.GetGold:
                    GameManager.I.Stage.AddGold(2);
                    break;
                case BoardEventRoulette.RandomArtifact:
                    GameManager.I.Stage.GetArtifact(0, 1,1);
                    break;
                case BoardEventRoulette.RandomCard:
                    GameManager.I.Stage.GetCardRange(1, 4);
                    break;
                case BoardEventRoulette.RandomTileGradeUp:
                    Debug.Log("랜덤 타일을 강화합니다. (동작 X)");
                    // [TODO] 타일과 기능 연결 필요
                    break;
                case BoardEventRoulette.ReducedHp:
                    GameManager.I.Stage.HitPlayer(5);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _resultObj.SetActive(true);
            _resultTMP.text = data.description;
            StartCoroutine(Close());
        }

        IEnumerator Close()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }
}
