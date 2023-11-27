using System;
using System.Collections;
using Cardinals.Utils;
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
                    GameManager.I.Stage.AddRandomArtifact();
                    break;
                // case BoardEventRoulette.RandomCard:
                //     GameManager.I.Stage.GetCardRange(1, 4);
                //     break;
                case BoardEventRoulette.RandomPotion:
                    GameManager.I.Stage.AddRandomPotion();
                    break;
                case BoardEventRoulette.RandomTileGradeUp:
                    var tile = GameManager.I.Stage.Board.GetRandomTile(false);
                    tile.TileMagic.GainExpToNextLevel();
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
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
        }
    }
}
