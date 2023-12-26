using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cardinals.Enums;
using System;
using System.Linq;
using Cardinals.Board;
using Cardinals.BoardEvent;
using Cardinals.BoardObject;
using Cardinals.Buff;
using Cardinals.Enemy;
using Sirenix.OdinInspector;
using Util;
using Cardinals.Tutorial;
using TMPro;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

namespace Cardinals.Game {
    
    public class StageController : MonoBehaviour {
        public int Index => _stage.Index;
        public Board.Board Board => _board;
        public Player Player => _player;
        public CardManager CardManager => _cardManager;
        public DiceManager DiceManager => _diceManager;
        public BaseEvent CurEvent => _curEvent;
        public List<BaseEnemy> Enemies {
            get {
                if (_curEvent == null) return null;
                
                if (_curEvent is BattleEvent battleEvent)
                    return GameManager.I.CurrentEnemies.ToList();
                else if (_curEvent is TutorialEvent tutorialEvent)
                    return tutorialEvent.Enemies;
                else return null;
            }
        }

        public event Action OnEventStart;
        
        private Stage _stage;
    
        private Transform _enemyParentTransform;
        
        private Transform _coreTransform;
        private Board.Board _board;
        private Player _player;
        private CardManager _cardManager;
        private DiceManager _diceManager;
        private BaseEvent _curEvent;

        private RewardBox _rewardBox;
        private StartFlag _startFlag;

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

        public StartFlag StartFlag {
            get {
                if (_startFlag == null) {
                    GameObject prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Stage_StartFlag);
                    _startFlag = Instantiate(prefab).GetComponent<StartFlag>();
                    _startFlag.Init();
                }

                return _startFlag;
            }
        }

        public List<IMovingBoardObject> BoardObjects { get; set; } = new();
        
        public IEnumerator Load(Stage stage) 
        {
            _stage = stage;

            InstantiateBaseObjs();
            
            InstantiateGround();

            yield return _board.SetBoard(stage.BoardData);

            if (GameManager.I.SaveSystem.CurrentSaveFileData == null) {
                SetCardSystem();
                yield return PlacePlayer();
                
                var newSave = GameManager.I.SaveSystem.GenerateAutoSaveFile(
                    GameManager.I.RuntimeStageList.ToArray(), 
                    Player, 
                    DiceManager
                );

                GameManager.I.SaveSystem.Save(newSave);
            } else {
                SetCardSystem(GameManager.I.SaveSystem.CurrentSaveFileData.GetDiceList());
                yield return PlacePlayer(GameManager.I.SaveSystem.CurrentSaveFileData);
                yield return LoadTileData(GameManager.I.SaveSystem.CurrentSaveFileData);
            }
        }
        
        public IEnumerator Flow() {
            bool UIStageInitialized = false;
            void SetUIStageInitialized() => UIStageInitialized = true;
            var UIStageVisitCoroutine = StartCoroutine(GameManager.I.UI.UIStage.Init(_stage, SetUIStageInitialized));
            while (!UIStageInitialized) {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                    UIStageInitialized = true;
                    StopCoroutine(UIStageVisitCoroutine);
                    GameManager.I.UI.UIStage.ImmediateInit(_stage);
                }

                yield return null;
            }

            yield return GameManager.I.UI.UIStage.Visit();
            
            // 축복 선택
            //yield return SelectBlessFlow();
            
            bool isStartEvent = true;

            // 다음 사건을 읽음
            while (_stage.MoveNext())
            {
                GameManager.I.SaveSystem.CurrentSaveFileData.UpdateData(
                    Player,
                    GameManager.I.Stage.DiceManager,
                    _stage,
                    GameManager.I.CurrentStageIndex
                );
                var saveResult = GameManager.I.SaveSystem.SaveCurrentSaveFileData();
                // TODO: 세이브 실패 시 처리

                // 현재 사건에 따른 이벤트 플로우 수행
                using var evt = _stage.Current as Game.BaseEvent;
                _curEvent = evt;
                
                bool eventLoaded = false;
                void SetEventLoaded() => eventLoaded = true;
                var eventLoadCoroutine = StartCoroutine(_curEvent.On(isStartEvent, SetEventLoaded));

                while (!eventLoaded) {
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                        eventLoaded = true;
                        StopCoroutine(eventLoadCoroutine);
                        _curEvent.ImmediateOn(isStartEvent);
                    }

                    yield return null;
                }

                yield return evt.Flow(this);

                if (!evt.IsClear)
                {
                    GameManager.I.GameOver();
                    yield break;
                }

                isStartEvent = false;
            }

            GameManager.I.GameClear();
        }

        public void OnEventStartCall() {
            OnEventStart?.Invoke();
        }

        public BlessType SelectedBless { private get; set; }
        /// <summary>
        /// 랜덤한 축복을 제공하는 플로우 입니다.
        /// </summary>
        public IEnumerator SelectBlessFlow()
        {
            // 현재 가지고 있지 않은 축복 2개 선택    
            List<BlessType> canGetBlesses = new();

            for (int i = 1, cnt = Enum.GetNames(typeof(BlessType)).Length; i < cnt; i++)
            {
                BlessType type = (BlessType)i;
                if (!Player.PlayerInfo.CheckBlessExist(type)) // 배우지 않은 축복들을 선택지로 추가
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

            yield return new WaitForSeconds(0.3f);

            prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Stage_Totem);
            Instantiate(prefab, new Vector3(1, 0.6f, 1), Quaternion.identity).GetComponent<BlessTotem>().Init(bless2);
            
            // 사용자 선택 대기
            SelectedBless = default;
            yield return new WaitUntil(() => SelectedBless != default);

            var objs = FindObjectsOfType<BlessTotem>();

            List<bool> hasDismiss = new List<bool>(objs.Length) {false, false};
            void setDismiss(int idx) => hasDismiss[idx] = true;

            for (int i = objs.Length - 1; i >= 0; i--)
            {
                Debug.Log($"Dismiss {i}");
                StartCoroutine(objs[i].Dismiss(SelectedBless, i, setDismiss));
            }
            yield return new WaitUntil(() => hasDismiss.All(x => x));
            for (int i = objs.Length - 1; i >= 0; i--)
            {
                Destroy(objs[i].gameObject);
            }
            
            Player.PlayerInfo.GetBless(SelectedBless);

            yield return new WaitForSeconds(1f);
        }

        public BaseEnemy InstantiateEnemy(EnemyDataSO enemyData, Vector3 position) {
            var enemyType = EnumHelper.GetEnemyInstanceType(enemyData.enemyType);
            
            GameObject enemyRendererPrefab 
                = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_EnemyRenderer);
            
            GameObject enemyRenderer = Instantiate(enemyRendererPrefab, _enemyParentTransform);
            
            BaseEnemy enemyComp = enemyRenderer.AddComponent(enemyType) as BaseEnemy;
            
            enemyComp.Init(enemyData);
            enemyRenderer.GetComponent<EnemyRenderer>().Init(enemyComp);
            
            enemyRenderer.transform.position = position + new Vector3(0, 2, 0);

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
            //_enemyInfoController = FindObjectOfType<EnemyInfoController>();
            
            GameObject BoardObj = new GameObject($"@{Constants.Common.InstanceName.Board}");
            BoardObj.transform.position = Vector3.zero;
            _board = BoardObj.AddComponent<Board.Board>();

            GameObject coreObj = new GameObject($"@{Constants.Common.InstanceName.Core}");
            _coreTransform = coreObj.transform;
            _coreTransform.position = Vector3.zero;
        }

        private IEnumerator PlacePlayer(SaveFileData saveFileData = null) {
            GameObject playerPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Player);
            GameObject playerObj = GameObject.Instantiate(playerPrefab);
            _player = playerObj.GetComponent<Player>();

            _player.Init();
            
            
            // UI 추가
            GameManager.I.UI.UINewPlayerInfo.Set(); // 창에 이벤트 연결
            GameManager.I.UI.InstantiatePlayerStatusUI(); // Status 창 연결

            if (saveFileData != null) {
                _player.SetData(
                    saveFileData.PotionList, 
                    saveFileData.BlessList, 
                    saveFileData.Coin, 
                    saveFileData.MaxHP, 
                    saveFileData.HP
                );
            }

            yield return playerObj.GetComponent<Player>().PlaceOnTile(
                saveFileData == null ? _board.GetStartTile() : _board[saveFileData.OnTileIndex]
            );
        }

        private void SetCardSystem(List<(int[], DiceType)> initialDiceList = null) {
            GameObject cardManagerObj = new GameObject($"@{Constants.Common.InstanceName.CardManager}");
            cardManagerObj.transform.position = Vector3.zero;
            _cardManager = cardManagerObj.AddComponent<CardManager>();
            _cardManager.transform.SetParent(_coreTransform);
           // _cardManager.gameObject.SetActive(false);
            _cardManager.Init();

            GameObject diceManagerObj = new GameObject($"@{Constants.Common.InstanceName.DiceManager}");
            diceManagerObj.transform.position = Vector3.zero;
            _diceManager = diceManagerObj.AddComponent<DiceManager>();
            _diceManager.transform.SetParent(_coreTransform);

            GameManager.I.UI.SetCardSystemUI();
            _diceManager.Init(initialDiceList);
        }

        private IEnumerator LoadTileData(SaveFileData saveFileData) {
            var tileDataList = saveFileData.TileSaveDataList;

            bool[] hasLoaded = new bool[tileDataList.Count];
            for (int i = 0; i < hasLoaded.Length; i++) hasLoaded[i] = false;

            for (int i = 0; i < tileDataList.Count; i++) {
                int t = i;
                StartCoroutine(_board[i].SetMagicSaveData(tileDataList[i], () => { hasLoaded[t] = true; }));
            }

            yield return new WaitUntil(() => hasLoaded.All(x => x));
        }

        /// <summary>
        /// [축복] 메테오 소환
        /// </summary>
        [Button]
        public void Meteor()
        {
            var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Player_Skill_Meteor);
            Instantiate(prefab);
        }
        
        #region Quick-Acess Method

        public void DrawCard(int count) => FindAnyObjectByType<CardManager>().Draw(count);
        public void HitPlayer(int damage) => Player.Hit(damage);
        
        public void AddGold(int value) => Player.PlayerInfo.AddGold(value);
        public void UseGold(int value) => Player.PlayerInfo.UseGold(value);

        public void GetCardRange(int minValue, int maxValue, int count = 1)
        {
            
        }

        public void GetCardArr(int[] values, int count = 1)
        {
            
        }

        public void AddRandomArtifact()
        {
            int idx = Random.Range(1, Enum.GetNames(typeof(ArtifactType)).Length);
            var artifact = (ArtifactType)idx;
            Player.PlayerInfo.AddArtifact(artifact);
        }

        [Button]
        public PotionType GetRandomPotion()
        {
            var list = Enum.GetValues(typeof(PotionType)).Cast<PotionType>().Where(x => (int)x > 0).ToList();
            int idx = Random.Range(0, list.Count());
            var potion = list[idx];
            
            return potion;
        }
        
        public PotionType AddRandomPotion()
        {
            var potion = GetRandomPotion();
            Player.PlayerInfo.AddPotion(potion);
            return potion;
        }

        [Button]
        public void AddBuff()
        {
            Enemies.FirstOrDefault().AddBuff(new Burn(1));
            // Enemies.FirstOrDefault().AddBuff(new Slow());
            // Enemies.FirstOrDefault().AddBuff(new Weak(1));
            // Enemies.FirstOrDefault().AddBuff(new ElectricShock());
            // Enemies.FirstOrDefault().AddBuff(new HealBuff(5));
            // Enemies.FirstOrDefault().AddBuff(new Poison(5));
            Player.AddBuff(new Burn(1));
            Player.AddBuff(new Slow());
            // Player.AddBuff(new Weak(1));
            Player.AddBuff(new ElectricShock());
            // Player.AddBuff(new HealBuff(5));
            // Player.AddBuff(new Poison(5));
        }
        #endregion

        // private int _boardEventIdx;
        // [Button]
        // public void GenerateBoardEvent()
        // {
        //     var tileAction = Board.GetCanSetEventTileEventAction();
        //     if (tileAction != null)
        //     {
        //         // 이벤트 설정                
        //         int enumLength = Enum.GetNames(typeof(BoardEventType)).Length;
        //         _boardEventIdx = Math.Max(1, (++_boardEventIdx % enumLength));
        //         var type = (BoardEventType)_boardEventIdx;
        //         
        //         tileAction.Set(type);
        //     }
        //     else Debug.Log("보드 내 이벤트 생성 가능한 코너 타일이 존재하지 않아, 이벤트가 생성되지 못했습니다.");
        // }
        
        [Button]
        public void GenerateNewBoardEvent()
        {
            var tiles = Board.TileSequence
                .Where(t => t != Player.OnTile && // 현재 플레이어가 서 있지 않은
                            !BoardObjects.Any(o => o is BaseBoardObject && t == o.OnTile )) // 이벤트가 존재하지 않는 ..
                .ToList();
            
            if (tiles.Any())
            {
                // 이벤트를 생성할 타일 설정
                Tile tile = tiles[Random.Range(0, tiles.Count())];
                
                // 이벤트 설정                
                int enumLength = Enum.GetNames(typeof(NewBoardEventType)).Length;
                var evt = Random.Range(1, enumLength);
                var evtType = (NewBoardEventType)evt;
                
                // 오브젝트 생성 및 컴프넌트 설정
                var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_BoardEventObject);
                var obj = Instantiate(prefab);

                Type type = EnumHelper.GetNewBoardEventType(evtType);
                obj.AddComponent(type);
                obj.GetComponent<BaseBoardObject>().Init(tile, evtType.ToString());
            }
            else Debug.Log("보드 내 이벤트 생성 가능한 타일이 존재하지 않아, 이벤트가 생성되지 못했습니다.");
        }

        public Dice GetRewardDice(EnemyGradeType grade)
        {
            // 눈
            var dices = ResourceLoader.LoadSO<RewardDiceSO>(Constants.FilePath.Resources.SO_Dice_Reward + grade);
            var idx = Random.Range(0, dices.numbers.Length);
            var number = dices.numbers[idx].Select(n => n - '0');
            
            // 타입
            var type = (DiceType) Random.Range((int)DiceType.Normal, (int)DiceType.Earth + 1);
            
            return new Dice(number.ToList(), type);
        }

        #region TestCode
        [Button]
        public void GenEnemySkill_TB()
        {
            // 썬더 볼트
            var list = GameManager.I.Stage.Board.GetCursedTilesList()?.ToList();
            if (list != null)
            {
                var index = Random.Range(0, list.Count);
                var tile = list[index];

                tile.SetCurse(TileCurseType.ThunderBolt, 2);
            }
        }

        [Button]
        public void GetBerserkTB()
        {
            List<Tile> tiles = new();
            
            // 각 라인에서 타일 지정
            for (int i = 0; i < 4; i++)
            {
                var list = GameManager.I.Stage.Board.GetBoardEdgeTileSequence(i, false)
                    .Where(t => t.TileState == TileState.Normal)
                    .Where(t => GameManager.I.Player.OnTile != t).ToList();

                if (list.Count > 0)
                {
                    var idx = Random.Range(0, list.Count);
                    tiles.Add(list[idx]);
                }
            }
            
            // 3개 이하로 줄이기
            for (int i = tiles.Count; i > 3; i--)
            {
                int idx = Random.Range(0, tiles.Count());
                tiles.RemoveAt(idx);
            }
            
            // 저주 적용
            foreach (var tile in tiles)
            {
                tile.SetCurse(TileCurseType.ThunderBolt, 3);
            }
        }

        [Button]
        public void TestEnemySkill_Fireball()
        {
            var list = GameManager.I.Stage.Board.GetCursedTilesList()?.ToList();
            
            if (list != null)
            {
                var index = Random.Range(0, list.Count);
                var tile = list[index];
                
                tile.SetCurse(TileCurseType.Fireball, 2);
            }
        }

        [Button]
        public void Test_GetBless(BlessType blessType)
        {
            Player.PlayerInfo.GetBless(blessType);
        }

        [Button]
        public void Test_SetEvent(BoardEventType eventType)
        {
            if (eventType != default)
            {
                switch (eventType)
                {
                    case BoardEventType.Tile :
                        GameManager.I.UI.UITileEvent.Init();
                        break;
                    case BoardEventType.Shop :
                        GameManager.I.UI.UIShop.Init();
                        break;
                    case BoardEventType.Roulette:
                        GameManager.I.UI.UIRoulette.Init();
                        break;
                    case BoardEventType.Number:
                        GameManager.I.UI.UIDiceEvent.Init();
                        break;
                    default: break;
                }
            }
        }

        [Button]
        public void Test_Potion(PotionType potionType)
        {
            GameManager.I.Player.PlayerInfo.AddPotion(potionType);
        }

        [Button]
        public void Test_Artifact(ArtifactType artifactType)
        {
            GameManager.I.Player.PlayerInfo.AddArtifact(artifactType);
        }

        [Button]
        public void Test_AddMoney(int num)
        {
            GameManager.I.Player.PlayerInfo.AddGold(num);
        }
        #endregion
    }

}
