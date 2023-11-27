using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Cardinals.Constants;

namespace Cardinals.Enemy
{
    /// <summary>
    /// 적의 정보(이름, 체력, 버프)를 관리하는 UI 스크립트
    /// </summary>
    public class UIEnemyInfo : MonoBehaviour
    {
        private BaseEnemy BaseEnemy { get; set; }

        [Header("Info")]
        [SerializeField] private TextMeshProUGUI _nameTMP;
        
        [Header("HP")]
        [SerializeField] private TextMeshProUGUI _hpTMP;
        [SerializeField] private RectTransform _maxHPRect;
        [SerializeField] private RectTransform _curHPRect;
        [SerializeField] private float _curHPEndPosX;

        [Header("Defense")]
        [SerializeField] private TextMeshProUGUI _defenseTMP;

        [Header("Buff")]
        [SerializeField] private Transform _buffTr;
        [SerializeField] private GameObject _buffPrefab;

        [Header("Pattern")]
        [SerializeField] private GameObject _patternObj;
        private Image _patternIconImg;
        private TextMeshProUGUI _patternCountTMP;
        private Image _bubbleImg;
        

        public void Init(BaseEnemy enemy)
        {
            BaseEnemy = enemy;

            // 액션 정보 표시를 위해 EnemyRenderer에 있는 컴포넌트 연결 
            var renderer = enemy.GetComponentInChildren<EnemyRenderer>();
            _patternIconImg = renderer.PatternIconImg;
            _patternCountTMP = renderer.PatternCountTMP;
            _bubbleImg = renderer.BubbleImg;
            
            // 정보 설정
            _nameTMP.text = BaseEnemy.Name;
            UpdateHp(BaseEnemy.Hp, BaseEnemy.MaxHp);
            
            // [TODO] Pattern Object 위치 설정 필요
            
            BaseEnemy.DieEvent += Destroy;
            BaseEnemy.AddBuffEvent += AddBuff;
            BaseEnemy.UpdatePatternEvent += UpdatePattern;
            BaseEnemy.UpdateHpEvent += UpdateHp;
            BaseEnemy.UpdateDefenseEvent += UpdateDefense;
        }

        private void UpdateDefense(int defense)
        {
            _defenseTMP.text = defense.ToString();
        }

        /// <summary>
        /// 적의 체력바 UI를 업데이트
        /// </summary>
        private void UpdateHp(int hp, int maxHp)
        {
            float curHPPosX = Mathf.Lerp(_curHPEndPosX, 0, (float)hp / maxHp);
            _curHPRect.localPosition = new Vector3(curHPPosX, 0, 0);
            _hpTMP.text = $"{hp}/{maxHp}";
        }
        
        /// <summary>
        /// 버프 아이콘을 추가
        /// </summary>
        private void AddBuff(BaseBuff buff)
        {
            Instantiate(_buffPrefab, _buffTr).GetComponent<UIBuff>().Init(buff);
        }
        
        private void UpdatePattern(Pattern pattern)
        {
            var key = Constants.FilePath.Resources.Enemy_Pattern + pattern.Type.ToString();
            _patternIconImg.sprite = EnemyPatternIconDict[key];
            _patternCountTMP.text = $"{pattern.Value}";
        }

        private Dictionary<string, Sprite> EnemyPatternIconDict =>
            ResourceLoader.LoadSpritesInDirectory(Constants.FilePath.Resources.Enemy_Pattern);

        public void SetBubbleDirection(bool isRightTail = false)
        {
            if (isRightTail)
            {
                Sprite sprite =
                    ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_UI_Enemy_Ballon_RightTail);
                _bubbleImg.sprite = sprite;
                
                // 말풍선 위치 수정
                var pos = _bubbleImg.rectTransform.anchoredPosition;
                _bubbleImg.rectTransform.anchoredPosition = new Vector2(-pos.x, pos.y);
            }
        }
        private void Destroy()
        {
            BaseEnemy = null;
            Destroy(gameObject);
        }
    }
}