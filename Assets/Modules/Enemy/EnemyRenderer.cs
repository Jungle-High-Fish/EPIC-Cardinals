using System.Collections;
using System.Diagnostics;
using System.Linq;
using Cardinals.Enums;
using Cardinals.Game;
using MoreMountains.Feedbacks;
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
        
        [SerializeField]private Material _material;

        private ComponentGetter<Transform> _renderer
            = new ComponentGetter<Transform>(TypeOfGetter.ChildByName, "Renderer");

        private ComponentGetter<EnemySlicer> _slicer
            = new ComponentGetter<EnemySlicer>(TypeOfGetter.This);
        
        private bool _hasBerserk = false;

        private GameObject _normalRenderer;
        private GameObject _berserkRenderer;

        #region Effects
        public MMF_Player HitMMF => _hitMFF;
        [SerializeField] private MMF_Player _hitMFF;
        #endregion

        [SerializeField] private Transform _renderParentTr;

        public void Init(BaseEnemy enemy)
        {
            BaseEnemy = enemy;
            BaseEnemy.DieEvent += Destroy;

            _slicer.Get(gameObject).Init();
            
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

        private void InstantiateRenderPrefabs(EnemyDataSO enemyData) {
            GameObject normal = Instantiate(enemyData.prefab, _renderer.Get(gameObject).transform);
            normal.name = "Normal";
            _normalRenderer = normal;
            _normalRenderer.GetComponent<SpriteRenderer>().material = new Material(_material);
            _normalRenderer.SetActive(true);

            if (enemyData.berserkPrefab == null) return;

            _hasBerserk = true;
            GameObject berserk = Instantiate(enemyData.berserkPrefab, _renderer.Get(gameObject).transform);
            berserk.name = "Berserk";
            berserk.GetComponent<SpriteRenderer>().material = new Material(_material);
            berserk.SetActive(false);
            BaseEnemy.BerserkModeEvent += () =>
            {
                _normalRenderer.SetActive(false);
                berserk.SetActive(true);
            };
        }
        
        private void Destroy()
        {
            BaseEnemy.enabled = false;
            BaseEnemy = null;
            StartCoroutine(DestroyCoroutine());
        }

        private IEnumerator DestroyCoroutine()
        { 
            GameManager.I.UI.UIAntiTouchPanel.SetActive(true);
            yield return _slicer.Get(gameObject).Slice();
            Destroy(gameObject);
            GameManager.I.UI.UIAntiTouchPanel.SetActive(false);
        }
    }
}