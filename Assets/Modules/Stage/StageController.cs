using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cardinals.Enums;
using System;
using Cardinals.Enemy;
using Util;

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
    }

}
