using System;
using System.Collections;
using System.Linq;
using Cardinals.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Cardinals.BoardEvent.Roulette
{
    public class UIRoulette : MonoBehaviour
    {
        private Roulette _roulette;
        [Header("Component")]
        [SerializeField] private Button _spinBTN;
        [SerializeField] private GameObject _resultObj;
        [SerializeField] private TextMeshProUGUI _resultTMP;

        [Header("Roulette Reward Value")]
        [SerializeField] private int _reducedHpValue;
        [SerializeField] private int _drawCardCount;
        [SerializeField] private int _getGoldValue;
        
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
            GameManager.I.UI.UIEndTurnButton.Deactivate();
            gameObject.SetActive(true);
            _resultObj.SetActive(false);
            Roulette.Init();
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
                    Debug.Log("이제 룰렛의 Draw Card는 어떻게 되는 것일까?");
                    // GameManager.I.Stage.DrawCard(_drawCardCount);
                    break;
                case BoardEventRoulette.GetGold:
                    GameManager.I.Stage.AddGold(_getGoldValue);
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
                    GameManager.I.Stage.HitPlayer(_reducedHpValue);
                    break;
                case BoardEventRoulette.GetRandomDice:
                    var dice = GameManager.I.Stage.GetRewardDice(EnemyGradeType.Elite);
                    GameManager.I.UI.UINewDicePanel.Init(dice, null);
                    break;
                case BoardEventRoulette.DamageToEnemy:
                    var list = GameManager.I.CurrentEnemies.ToList();
                    var target = list[Random.Range(0, list.Count)];
                    GameManager.I.Player.Attack(target, 10);
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
            GameManager.I.UI.UIEndTurnButton.Activate();
            gameObject.SetActive(false);
        }
    }
}
