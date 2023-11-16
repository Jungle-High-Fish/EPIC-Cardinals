using UnityEngine;

namespace Cardinals.Enemy
{
    /// <summary>
    /// 적의 스프라이트를 관리하는 UI 스크립트 
    /// </summary>
    public class EnemyRenderer : MonoBehaviour
    {
        private BaseEnemy BaseEnemy { get; set; }
        
        [Header("Sprite")]
        [SerializeField] private SpriteRenderer _renderer;
        
        public void Init(BaseEnemy enemy)
        {
            BaseEnemy = enemy;
            BaseEnemy.DieEvent += Destroy;
            BaseEnemy.UpdatedSpriteEvent += ChangeSprite;
        }

        private void ChangeSprite(Sprite sprite)
        {
            _renderer.sprite = sprite;
        }
        private void Destroy()
        {
            BaseEnemy = null;
            Destroy(gameObject);
        }
    }
}