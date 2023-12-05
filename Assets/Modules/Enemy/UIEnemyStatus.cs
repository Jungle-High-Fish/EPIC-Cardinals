using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Cardinals.Enemy;
using Cardinals.Entity.UI;
using Cardinals.Enums;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Cardinals.UI
{
    public class UIEnemyStatus : UIEntityStatus
    {
        [Header("Pattern")]
        [SerializeField] private Transform _patternTr;
        [SerializeField] private Image _patternIconImg;
        [SerializeField] private TextMeshProUGUI _patternCountTMP;
        
        public override void Init(BaseEntity entity)
        {
            base.Init(entity);

            BaseEnemy enemy = entity as BaseEnemy;
            enemy.UpdatePatternEvent += UpdatePattern;
            enemy.DieEvent += Destroy;
            
            // 위치 설정
            SetContentSize();
            SetPatternObjPos(enemy);
        }
        /// <summary>
        /// 공통으로 지원하는 UI들의 위치를 조정합니다.
        /// </summary>
        protected void SetContentSize()
        {
            var spriteRenderer = _entity.Renderers.FirstOrDefault();

            if (spriteRenderer == null) 
                return;
            
            // Sprite의 텍스처 너비를 구합니다
            float spriteWidth = spriteRenderer.sprite.rect.width;
            float spriteHeightHalf = spriteRenderer.sprite.rect.height / 4;

            var pos = _statusTr.localPosition;
            pos.y -= spriteHeightHalf;

            var enemy = _entity as BaseEnemy;
            if (enemy != null)
            {
                // 예외 처리로 값 감산
                pos += enemy.EnemyData.enemyType switch
                {
                    EnemyType.One => new Vector3(0, -30),
                    EnemyType.Three1 or EnemyType.Three2 => new Vector3(0, -30),
                    _ => Vector2.zero
                };
            }
            
            // 스테이터스 오브젝트 위치 설정
            _statusTr.localPosition = pos;
            
            // 체력바 설정
            _maxHPRect.sizeDelta = new Vector2( spriteWidth, _maxHPRect.sizeDelta.y); // 크기
            
            // 버프 최대 갯수 설정
            _buffListArea.GetComponent<GridLayoutGroup>().constraintCount = (int) spriteWidth / 36;
            
            _statusTr.GetComponent<GridSizeUpdator>().Resizing();
        }

        /// <summary>
        /// 몬스터에서만 지원하는 UI들의 위치를 조정합니다.
        /// </summary>
        private void SetPatternObjPos(BaseEnemy enemy)
        {
            var spriteRenderer = _entity.Renderers.FirstOrDefault();
            
            // Sprite의 텍스처 높이를 구합니다
            float spriteHeightHalf = spriteRenderer.sprite.rect.height / 2;

            var pos = _patternTr.localPosition;
            pos.y = spriteHeightHalf;
            
            // 예외 처리로 값 감산
            pos += enemy.EnemyData.enemyType switch
            {
                EnemyType.One => new Vector3(0, -60),
                EnemyType.Three1 or EnemyType.Three2 => new Vector3(0, -60),
                _ => Vector2.zero
            };
                
            _patternTr.GetComponent<RectTransform>().position += pos;
            _patternTr.localPosition = pos;
        }
        
        private void UpdatePattern(Pattern pattern)
        {
            // 이펙트
            _patternTr.localScale = Vector3.zero;
            _patternTr.DOScale(1, .5f).SetEase(Ease.InOutElastic);
            
            // 설정
            var key = Constants.FilePath.Resources.Enemy_Pattern + pattern.Type.ToString();
            _patternIconImg.sprite = EnemyPatternIconDict[key];

            if (CheckSetWeakDamage(pattern.Type)) // 약화 시, 데미지 반절 적용
            {
                if (pattern.Value != null && pattern.Value != 0)
                {
                    float dam = (float) pattern.Value / 2;
                    _patternCountTMP.text = $"{(int) Math.Floor(dam)}";
                }
            }
            else
            {
                _patternCountTMP.text = $"{pattern.Value}";
            }
            
            _patternTr.GetComponent<GridSizeUpdator>().Resizing();
        }

        private bool CheckSetWeakDamage(EnemyActionType type)
        {
            return _entity.CheckBuffExist(BuffType.Weak) &&
                   (type == EnemyActionType.Attack ||
                    type == EnemyActionType.AreaAttack);
        }

        private void UseIcon()
        {
            _patternTr.DOScale(2, .5f).SetEase(Ease.InOutElastic)
                .OnComplete(() => _patternTr.localScale = Vector3.one);
            _patternIconImg.DOFade(.5f, .5f)
                .OnComplete(() => _patternIconImg.color = Color.white);
        }

        private Dictionary<string, Sprite> EnemyPatternIconDict =>
            ResourceLoader.LoadSpritesInDirectory(Constants.FilePath.Resources.Enemy_Pattern);

        // public void SetBubbleDirection(bool isRightTail = false)
        // {
        //     if (isRightTail)
        //     {
        //         Sprite sprite =
        //             ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_UI_Enemy_Ballon_RightTail);
        //         _patternIconImg.sprite = sprite;
        //         
        //         // 말풍선 위치 수정
        //         var pos = _patternIconImg.rectTransform.anchoredPosition;
        //         _patternIconImg.rectTransform.anchoredPosition = new Vector2(-pos.x, pos.y);
        //     }
        // }
        
        private void Destroy()
        {
            _entity =  null;
            Destroy(gameObject);
        }
    }
}