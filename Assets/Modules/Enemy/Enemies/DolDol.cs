﻿using Cardinals.Enums;
using Cardinals.Game;
using Modules.Entity.Buff;
using System;
using UnityEngine;
using Util;

namespace Cardinals.Enemy
{
    public class DolDol : BaseEnemy
    {
        private Sprite _dieSprite;
        public override void Init(EnemyDataSO enemyData)
        {
            DieEvent += () =>
            {
                Renderers[0].sprite = enemyData.spec1Sprite;
                Renderers[1].gameObject.SetActive(false);
            };
            
            base.Init(enemyData);

            Patterns = new[]
            {
                new Pattern(EnemyActionType.Attack, 10),
                new Pattern(EnemyActionType.Defense, 8),
                new Pattern(EnemyActionType.Attack, 5)
            };
            
            Rewards = new Reward[]
            {
                new(RewardType.Gold, 100)
            };

        }

        public override void PostInit()
        {
            // 버프 생성자에서 플레이어 한 바퀴 돌 때 마다, 피격 받도록 이벤트 추가 및 죽음시 해지하도록 설정
            AddBuff(new RotationRate(this, 10));
        }

        public override void Hit(int damage, TileMagicType type)
        {
            Debug.Log("디버프에 의해 무효화 됨 (해당 이펙트 출력)");
        }

        public void EffectiveHit(int damage)
        {
            base.Hit(damage);
        }

        public override void Flip(bool filpX, Quaternion quat = default)
        {
            // 해당 몬스터는 플레이어 위치에 따른 플립을 수행하지 않음
        }
    }
}