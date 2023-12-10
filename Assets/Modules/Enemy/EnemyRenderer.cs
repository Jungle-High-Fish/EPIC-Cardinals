using System.Diagnostics;
using Cardinals.Enums;
using Cardinals.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.Enemy
{
    /// <summary>
    /// 적의 스프라이트를 관리하는 UI 스크립트 
    /// </summary>
    public class EnemyRenderer : MonoBehaviour
    {
        private BaseEnemy BaseEnemy { get; set; }
        
        private ComponentGetter<Transform> _renderer
            = new ComponentGetter<Transform>(TypeOfGetter.ChildByName, "Renderer");
        
        private bool _hasBerserk = false;

        private GameObject _normalRenderer;
        private GameObject _berserkRenderer;

        [SerializeField] private Transform _renderParentTr;

        public void Init(BaseEnemy enemy)
        {
            BaseEnemy = enemy;
            BaseEnemy.DieEvent += Destroy;
            
            InstantiateRenderPrefabs(enemy.EnemyData);

            enemy.Renderers = GetComponentsInChildren<SpriteRenderer>();

            Vector3 vector = enemy.EnemyData.enemyType switch
            {
                EnemyType.TweTwe => new Vector2(0, .5f) ,
                EnemyType.PicPic => new Vector2(0, 0.83f),
                EnemyType.Pazizizizic => new Vector2(0, .5f),
                _ => Vector2.zero
            };

            _renderParentTr.position += vector;
        }

        public void FlipX(bool flipX)
        {
            foreach (var render in BaseEnemy.Renderers)
            {
                render.flipX = flipX;
            }
        }

        private void InstantiateRenderPrefabs(EnemyDataSO enemyData) {
            GameObject normal = Instantiate(enemyData.prefab, _renderer.Get(gameObject).transform);
            normal.name = "Normal";
            _normalRenderer = normal;
            _normalRenderer.SetActive(true);

            if (enemyData.berserkPrefab == null) return;

            _hasBerserk = true;
            GameObject berserk = Instantiate(enemyData.berserkPrefab, _renderer.Get(gameObject).transform);
            berserk.name = "Berserk";
            berserk.SetActive(false);
            BaseEnemy.BerserkModeEvent += () =>
            {
                _normalRenderer.SetActive(false);
                berserk.SetActive(true);
            };
        }
        
        private void Destroy()
        {
            BaseEnemy = null;
            Destroy(gameObject);
        }
    }
}