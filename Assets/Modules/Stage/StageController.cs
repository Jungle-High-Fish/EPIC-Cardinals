using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cardinals.Enums;
using System;
using System.Linq;
using Cardinals.Board;
using Cardinals.Enemy;
using Sirenix.OdinInspector;
using Util;
using Random = UnityEngine.Random;

namespace Cardinals.Game {
    
    public class StageController : MonoBehaviour {
        public Board.Board Board => _board;
        public Player Player => _player;
        public CardManager CardManager => _cardManager;
        public BaseEvent CurEvent => _curEvent;
        public List<BaseEnemy> Enemies {
            get {
                if (_curEvent == null) return null;
                if (_curEvent is not BattleEvent) return null;
                
                return (_curEvent as BattleEvent).Enemies;
            }
        }
        
        private Stage _stage;
    
        private Transform _enemyParentTransform;
        private Transform _coreTransform;

        private Board.Board _board;
        private Player _player;
        private CardManager _cardManager;
        private BaseEvent _curEvent;

        private RewardBox _rewardBox;

        public RewardBox RewardBox
        {
            get
            {
                if (_rewardBox == null)
                {
                    GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Stage_RewardBox);
                    _rewardBox = Instantiate(prefab).GetComponent<RewardBox>();
                    _rewardBox.Init();
                }
                
                return _rewardBox;
            }
        }
        
        public IEnumerator Load(Stage stage) 
        {
            _stage = stage;
            _stage.Init(-1);

            InstantiateBaseObjs();

            InstantiateGround();

            yield return _board.SetBoard(stage.BoardData);

            SetCardSystem();

            PlacePlayer();
        }

        public IEnumerator Flow() {
            GameManager.I.UI.UIStage.Init(_stage);
            yield return GameManager.I.UI.UIStage.Visit();
            
            // 축복 선택
            yield return SelectBlessFlow();
            
            // 다음 사건을 읽음
            while (_stage.MoveNext())
            {
                // 현재 사건에 따른 이벤트 플로우 수행
                using var evt = _stage.Current as Game.BaseEvent;
                _curEvent = evt;
                
                evt.On();
                
                yield return GameManager.I.UI.UIStage.EventIntro();
                yield return evt.Flow(this);

                if (!evt.IsClear)
                {
                    GameManager.I.GameOver();
                }
            }

            GameManager.I.GameClear();
        }

        public BlessType SelectedBless { private get; set; }
        /// <summary>
        /// 랜덤한 축복을 제공하는 플로우 입니다.
        /// </summary>
        IEnumerator SelectBlessFlow()
        {
            // 현재 가지고 있지 않은 축복 2개 선택    
            List<BlessType> canGetBlesses = new();

            for (int i = 1, cnt = Enum.GetNames(typeof(BlessType)).Length; i < cnt; i++)
            {
                BlessType type = (BlessType)i;
                if (!Player.PlayerInfo.BlessList.Contains(type)) // 배우지 않은 축복들을 선택지로 추가
                {
                    canGetBlesses.Add(type);
                }
            }

            BlessType bless1 = canGetBlesses[Random.Range(0, canGetBlesses.Count)];
            canGetBlesses.Remove(bless1);
            BlessType bless2 = canGetBlesses[Random.Range(0, canGetBlesses.Count)];
            
            // 화면에 표시
            var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Stage_Totem);
            Instantiate(prefab, new Vector3(-1, 0.6f, -1), Quaternion.identity).GetComponent<BlessTotem>().Init(bless1);
            prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Stage_Totem);
            Instantiate(prefab, new Vector3(1, 0.6f, 1), Quaternion.identity).GetComponent<BlessTotem>().Init(bless2);
            
            // 사용자 선택 대기
            SelectedBless = default;
            yield return new WaitUntil(() => SelectedBless != default);

            var objs = FindObjectsOfType<BlessTotem>();
            for (int i = objs.Length - 1; i >= 0; i--)
            {
                Destroy(objs[i].gameObject);
            }
            
            Player.PlayerInfo.GetBless(SelectedBless);

            yield return new WaitForSeconds(1f);
        }

        public BaseEnemy InstantiateEnemy(EnemyDataSO enemyData) {
            var enemyType = EnumHelper.GetEnemyInstanceType(enemyData.enemyType);
            
            GameObject enemyRendererPrefab 
                = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_EnemyRenderer);

            GameObject enemyRenderer = GameObject.Instantiate(enemyRendererPrefab, _enemyParentTransform);
            BaseEnemy enemyComp = enemyRenderer.AddComponent(enemyType) as BaseEnemy;
            enemyComp.Init(enemyData);
            enemyRenderer.GetComponent<EnemyRenderer>().Init(enemyComp);
            enemyRenderer.transform.position = enemyRenderer.transform.position + new Vector3(0, 2, 0);

            GameManager.I.UI.SetEnemyUI(enemyComp);

            return enemyComp;
        }

        private void InstantiateGround() {
            GameObject groundPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_StageGround);
            GameObject groundObj = GameObject.Instantiate(groundPrefab, transform);
            groundObj.transform.position = Vector3.zero;
        }

        private void InstantiateBaseObjs() {
            GameObject EnemyParentTransformObj = new GameObject($"@{Constants.Common.InstanceName.EnemyPlace}");
            _enemyParentTransform = EnemyParentTransformObj.transform;

            GameObject BoardObj = new GameObject($"@{Constants.Common.InstanceName.Board}");
            BoardObj.transform.position = Vector3.zero;
            _board = BoardObj.AddComponent<Board.Board>();

            GameObject coreObj = new GameObject($"@{Constants.Common.InstanceName.Core}");
            _coreTransform = coreObj.transform;
            _coreTransform.position = Vector3.zero;
        }

        private void PlacePlayer() {
            GameObject playerPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Player);
            GameObject playerObj = GameObject.Instantiate(playerPrefab);
            _player = playerObj.GetComponent<Player>();
            _player.Init(15);

            _board.PlacePieceToTile(playerObj.GetComponent<Player>(), _board.GetStartTile());
            
            GameManager.I.UI.InitPlayerUI();
        }

        private void SetCardSystem() {
            GameObject cardManagerObj = new GameObject($"@{Constants.Common.InstanceName.CardManager}");
            cardManagerObj.transform.position = Vector3.zero;
            _cardManager = cardManagerObj.AddComponent<CardManager>();
            _cardManager.transform.SetParent(_coreTransform);
            _cardManager.Init();

            GameManager.I.UI.SetCardSystemUI();
        }

        #region Quick-Acess Method

        public void DrawCard(int count) => FindAnyObjectByType<CardManager>().Draw(count);
        public void HitPlayer(int damage) => Player.Hit(damage);
        
        public void AddGold(int value) => Player.PlayerInfo.AddGold(value);
        public void UseGold(int value) => Player.PlayerInfo.UseGold(value);
        
        public void GetArtifact(int grade, int choiceCnt, int selectCnt) // grade는 임시임
        {
            
        }

        public void GetCardRange(int minValue, int maxValue, int count = 1)
        {
            
        }

        public void GetCardArr(int[] values, int count = 1)
        {
            
        }

        public void AddArtifact()
        {
        }

        public void AddPotion()
        {
            
        }
        #endregion

        private int _boardEventIdx;
        [Button]
        public void GenerateBoardEvent()
        {
            var tileAction = Board.GetCanSetEventTileEventAction();
            if (tileAction != null)
            {
                // 이벤트 설정                
                int enumLength = Enum.GetNames(typeof(BoardEventType)).Length;
                _boardEventIdx = Math.Max(1, ++_boardEventIdx) % enumLength;
                var type = (BoardEventType)_boardEventIdx;
                
                tileAction.Set(type);
            }
            else Debug.Log("보드 내 이벤트 생성 가능한 코너 타일이 존재하지 않아, 이벤트가 생성되지 못했습니다.");
        }
    }

}
