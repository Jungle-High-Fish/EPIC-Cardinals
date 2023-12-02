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
        
        [SerializeField] private Image _patternIconImg;
        [SerializeField] private TextMeshProUGUI _patternCountTMP;
        [SerializeField] private Image _bubbleImg;
        public Image PatternIconImg => _patternIconImg;
        public TextMeshProUGUI PatternCountTMP => _patternCountTMP;
        public Image BubbleImg => _bubbleImg;
        
        private bool _hasBerserk = false;

        private GameObject _normalRenderer;
        private GameObject _berserkRenderer;

        public void Init(BaseEnemy enemy)
        {
            BaseEnemy = enemy;
            BaseEnemy.DieEvent += Destroy;
            BaseEnemy.ChangeRenderPrefabEvent += ChangePrefab;

            InstantiateRenderPrefabs(enemy.EnemyData);
            ChangePrefab(false);
        }

        private void ChangePrefab(bool isBerserk)
        {
            if (isBerserk) {
                if (_hasBerserk) {
                    _normalRenderer.SetActive(false);
                    _berserkRenderer.SetActive(true);
                }
            } else {
                _normalRenderer.SetActive(true);
                _berserkRenderer?.SetActive(false);
            }
        }

        private void InstantiateRenderPrefabs(EnemyDataSO enemyData) {
            GameObject normal = Instantiate(enemyData.prefab, _renderer.Get(gameObject).transform);
            normal.name = "Normal";
            _normalRenderer = normal;

            if (enemyData.berserkPrefab == null) return;

            _hasBerserk = true;
            GameObject berserk = Instantiate(enemyData.berserkPrefab, _renderer.Get(gameObject).transform);
            berserk.name = "Berserk";
            berserk.SetActive(false);
        }
        
        private void Destroy()
        {
            BaseEnemy = null;
            Destroy(gameObject);
        }
    }
}