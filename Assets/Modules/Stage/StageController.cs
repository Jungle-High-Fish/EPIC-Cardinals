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
        public BaseEvent CurEvent => _curEvent;
        
        private Stage _stage;
    
        private Transform _enemyParentTransform;
        private Transform _enemyUIParentTransform;

        private Board.Board _board;
        private Player _player;
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

            yield return _board.SetBoard(stage.BoardData);

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

            GameObject UIEnemyInfoPrefab 
                = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UIEnemyInfo);
            GameObject enemyRendererPrefab 
                = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_EnemyRenderer);

            GameObject enemyRenderer = GameObject.Instantiate(enemyRendererPrefab, _enemyParentTransform);
            BaseEnemy enemyComp = enemyRenderer.AddComponent(enemyType) as BaseEnemy;
            enemyComp.Init(enemyData);
            enemyRenderer.GetComponent<EnemyRenderer>().Init(enemyComp);
            enemyRenderer.transform.position = enemyRenderer.transform.position + new Vector3(0, 2, 0);

            GameObject UIEnemyInfo = GameObject.Instantiate(UIEnemyInfoPrefab, _enemyUIParentTransform);
            UIEnemyInfo.GetComponent<UIEnemyInfo>().Init(enemyComp);

            return enemyComp;
        }

        private void InstantiateBaseObjs() {
            GameObject EnemyParentTransformObj = new GameObject($"@{Constants.Common.InstanceName.EnemyPlace}");
            _enemyParentTransform = EnemyParentTransformObj.transform;
            
            // 임시로 Enemy UI 캔버스에 생성
            Transform CanvasTransform = GameObject.Find("Canvas").transform;
            GameObject EnemyUIParentTransformObj = new GameObject(
                    $"@{Constants.Common.InstanceName.EnemyUI}", 
                    typeof(RectTransform)
            );
            EnemyUIParentTransformObj.GetComponent<RectTransform>().MatchParent(CanvasTransform as RectTransform);
            _enemyUIParentTransform = EnemyUIParentTransformObj.transform;

            GameObject BoardObj = new GameObject($"@{Constants.Common.InstanceName.Board}");
            BoardObj.transform.position = Vector3.zero;
            _board = BoardObj.AddComponent<Board.Board>();
        }

        private void PlacePlayer() {
            GameObject playerPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Player);
            GameObject playerObj = GameObject.Instantiate(playerPrefab);
            _player = playerObj.GetComponent<Player>();
            _player.Init(15);

            Transform CanvasTransform = GameObject.Find("PlayerCanvas").transform;
            GameObject playerUIPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UIPlayerInfo);
            GameObject playerUIObj = GameObject.Instantiate(playerUIPrefab, CanvasTransform);

            _board.PlacePieceToTile(playerObj.GetComponent<Player>(), _board.GetStartTile());
        }

        #region Quick-Acess Method

        public void DrawCard(int count) => FindAnyObjectByType<CardManager>().Draw(count);
        public void HitPlayer(int damage) => Player.Hit(damage);
        public void AddGold(int value) => Player.PlayerInfo.AddGold(value);
        
        public void GetArtifact(int grade, int choiceCnt, int selectCnt) // grade는 임시임
        {
            
        }

        public void GetCardRange(int minValue, int maxValue, int count = 1)
        {
            
        }

        public void GetCardArr(int[] values, int count = 1)
        {
            
        }
        #endregion
    }

}
