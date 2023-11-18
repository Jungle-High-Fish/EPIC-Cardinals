using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cardinals.Enums;
using System;
using Cardinals.Enemy;
using Util;

namespace Cardinals.Game {
    
    public class StageController : MonoBehaviour {
        public BaseEvent CurEvent => _curEvent;
        public Board.Board Board => _board;
        
        private Stage _stage;
    
        private Transform _enemyParentTransform;
        private Transform _enemyUIParentTransform;

        private Board.Board _board;

        private BaseEvent _curEvent;
        
        public IEnumerator LoadStage(Stage stage) {
            _stage = stage;
            _stage.Init(-1);

            InstantiateBaseObjs();

            yield return _board.SetBoard(stage.BoardData);

            PlacePlayer();
        }

        public IEnumerator StageFlow() {
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

        public BaseEnemy InstantiateEnemy(EnemyType type) {
            (string name, int hp) enemyData;
            var enemy = EnemyFactory.GetEnemy(type, out enemyData);

            GameObject UIEnemyInfoPrefab 
                = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UIEnemyInfo);
            GameObject enemyRendererPrefab 
                = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_EnemyRenderer);

            GameObject enemyRenderer = GameObject.Instantiate(enemyRendererPrefab, _enemyParentTransform);
            BaseEnemy enemyComp = enemyRenderer.AddComponent(enemy) as BaseEnemy;
            enemyComp.Init();
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
            GameObject EnemyUIParentTransformObj = new GameObject($"@{Constants.Common.InstanceName.EnemyUI}", typeof(RectTransform));
            EnemyUIParentTransformObj.transform.SetParent(CanvasTransform);
            RectTransform rectTransform = EnemyUIParentTransformObj.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            _enemyUIParentTransform = EnemyUIParentTransformObj.transform;

            GameObject BoardObj = new GameObject($"@{Constants.Common.InstanceName.Board}");
            BoardObj.transform.position = Vector3.zero;
            _board = BoardObj.AddComponent<Board.Board>();
        }

        private void PlacePlayer() {
            GameObject playerPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Player);
            GameObject playerObj = GameObject.Instantiate(playerPrefab);

            Transform CanvasTransform = GameObject.Find("PlayerCanvas").transform;
            GameObject playerUIPrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UIPlayerInfo);
            GameObject playerUIObj = GameObject.Instantiate(playerUIPrefab, CanvasTransform);

            _board.PlacePieceToTile(playerObj.GetComponent<Player>(), _board.GetStartTile());
        }
    }

}
