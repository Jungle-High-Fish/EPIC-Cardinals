using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Cardinals.Board;
using System;
using System.Linq;
using Cardinals.Enums;
using Cardinals.Game;
using Cardinals.Buff;
using Sirenix.Utilities;
using Unity.Mathematics;
using Modules.Entity.Buff;
using MoreMountains.Feedbacks;
using Util;

namespace Cardinals
{
    public class Player : BaseEntity, IBoardPiece
    {
        [Header("초기 값")] [SerializeField] private int _initHp; 
        
        [Header("Other")]
        
        [SerializeField] private Board.Tile _onTile;
        private bool _isDamagedThisTurn;
        [InlineEditor]
        [SerializeField] private PlayerInfo _playerInfo;

        
        
        private Animator _animator;
        protected override Animator Animator => (_animator ??= GetComponentInChildren<Animator>());
        
        public PlayerInfo PlayerInfo => _playerInfo;
        public Tile OnTile => _onTile;

        public Action<PlayerActionType> UpdateActionEvent { get; set; }
        public PlayerActionType CurActionType { get; private set; }

        private Quaternion _defaultRotate;

        public override void Init(int _ = default) {
            base.Init(_initHp);
            _playerInfo = new PlayerInfo();
            
            _defaultRotate = Renderers.First().transform.rotation;
            HomeReturnEvent += () => { GameManager.I.TurnCount++; };
        }

        public void SetData(PotionType[] potionList, BlessType[] blessList, int coin, int maxHp, int hp)
        {
            potionList.ForEach(p => _playerInfo.AddPotion(p));
            blessList.ForEach(b => _playerInfo.GetBless(b));
            _playerInfo.SetGold(coin);
            Hp = hp;
        }

        public override int Hp {
            get => base.Hp;
            protected set
            {
                var calculHp = Math.Min(Math.Max(0, value), MaxHp);

                if (calculHp != _hp)
                {
                    _hp = calculHp;
                    _isDamagedThisTurn = true;
                    UpdateHpEvent?.Invoke(_hp, MaxHp);
                    
                    if (_hp == 0)
                    {
                        Animator.SetTrigger("Die");
                        Bubble.SetBubble(BubbleText.die);
                        DieEvent?.Invoke();
                        GameManager.I.Next();
                     }
                }
            }
        }

        public override IEnumerator OnTurn()
        {
            _isDamagedThisTurn = false;
            
            if (CheckBuffExist(BuffType.Stun))
            {
                // 행동 하지 않음... 스턴 효과 애니메이션 출력?
            }
            else
            {
                GameManager.I.UI.UIEndTurnButton.Activate();

                yield return GameManager.I.WaitNext(); // 플레이어의 [턴 종료] 버튼 선택 대기

                GameManager.I.UI.UIEndTurnButton.Deactivate();
                GameManager.I.Stage.DiceManager.SetDiceSelectable(false);
            }
        }

        public IEnumerator PreEndTurn()
        {
            if (PlayerInfo.CheckBlessExist(BlessType.BlessEarth2) && DefenseCount == 0)
            {
                AddDefenseCount(4);
                GameManager.I.Player.PlayerInfo.BlessEventDict[BlessType.BlessEarth2]?.Invoke();
            }

            yield break;
        }
        public override IEnumerator EndTurn()
        {
            yield return base.EndTurn(); // 버프/디버프 소모
            
            //yield return GameManager.I.Stage.CardManager.EndTurn();
            yield return GameManager.I.Stage.DiceManager.EndTurn();
            GameManager.I.Player.UpdateAction(PlayerActionType.None);
            
            if (PlayerInfo.CheckBlessExist(BlessType.BlessEarth1))
            {
                BlessEarth1();
            }

            ResetDefenseCount();
        }

        public void EndBattle()
        {
            ResetDefenseCount();
        }

        public void BlessWater1()
        {
            GameManager.I.Player.PlayerInfo.BlessEventDict[BlessType.BlessWater1]?.Invoke();
            if (GameManager.I.Stage.CurEvent is BattleEvent battleEvent) {
                Heal(battleEvent.Round);
            }
        }

        void BlessEarth1()
        {
            if (DefenseCount > 0)
            {
                GameManager.I.Player.PlayerInfo.BlessEventDict[BlessType.BlessEarth1]?.Invoke();
                
                var enemies = GameManager.I.CurrentEnemies.ToList();
                for (int i = enemies.Count - 1; i >= 0; i--)
                {
                    if (enemies[i].PrevPattern?.Type == EnemyActionType.Attack)
                    {
                        enemies[i].Hit(DefenseCount);
                    }
                }
            }
        }

        public void SetMaxHP(int value)
        {
            MaxHp = value;
        }

        public void SetTile(Tile tile)
        {
            _onTile = tile;
            //transform.position = tile.transform.position + new Vector3(0, 1.3f, 0);
        }

        public Action HomeReturnEvent { get; set; }

        public IEnumerator PlaceOnTile(Tile tile)
        {
            SetTile(tile);
            yield return _onTile.Arrive(this, true);
        }
      
        public IEnumerator MoveTo(int count,float time)
        {
            _onTile?.Leave(this);
            GameManager.I.UI.UINewPlayerInfo.TileInfo.Hide();

            for(int i = 0; i < count; i++)
            {
                if (_onTile.Next == (GameManager.I.Stage.CurEvent as BattleEvent).RoundStartTile) {
                    HomeReturnEvent?.Invoke();

                    // [축복] 잔잔한 수면: 보드를 한 바퀴 돌 때마다, 돈 바퀴 수 만큼 회복합니다.
                    if (_playerInfo.CheckBlessExist(BlessType.BlessWater1)) {
                        BlessWater1();
                    }
                }

                SetFlipOnMoveByPosition(_onTile.Next);

                Vector3 nextPos = _onTile.Next.transform.position;
                nextPos.y += 1.3f;
                transform.DOJump(nextPos, 2, 1, time).OnComplete(CreateLandingParticle);
                Animator.Play("Jump");

                yield return new WaitForSeconds(time / 2);
                GameManager.I.Sound.PlayerMove();
                yield return new WaitForSeconds(time / 2);

                _onTile = _onTile.Next;
                yield return CheckCollisionBoardObject();
                if (i != count - 1) {
                    _onTile.StepOn(this);
                }
                
            }

            yield return CheckArriveBoardObject();

            yield return _onTile.Arrive(this);

            SetFlipTowardEnemy();
        }
        
        public IEnumerator PrevMoveTo(int count, float time)
        {
            _onTile?.Leave(this);
            GameManager.I.UI.UINewPlayerInfo.TileInfo.Hide();

            for (int i = 0; i < count; i++)
            {
                SetFlipOnMoveByPosition(_onTile.Prev);
                Vector3 prevPos = _onTile.Prev.transform.position;
                prevPos.y += 1.3f;
                transform.DOJump(prevPos, 2, 1, time).OnComplete(CreateLandingParticle);
                Animator.Play("Jump");

                yield return new WaitForSeconds(time / 2);
                GameManager.I.Sound.PlayerMove();
                yield return new WaitForSeconds(time / 2);

                _onTile = _onTile.Prev;
                yield return CheckCollisionBoardObject();
                if (i != count - 1)
                {
                    _onTile.StepOn(this);
                }
            }

            yield return CheckArriveBoardObject();
            
            yield return _onTile.Arrive(this);
            
            SetFlipTowardEnemy();
        }

        private void CreateLandingParticle()
        {
            // 파티클 생성
            var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Particle_LandingDust);
            Instantiate(prefab, transform.position + new Vector3(0, -1f, 0), quaternion.identity);
        }
        
        private IEnumerator CheckCollisionBoardObject()
        {
            var summons = GameManager.I.Stage.BoardObjects;
            for (int i = summons.Count - 1; i >= 0; i--)
            {
                if (_onTile == summons[i].OnTile)
                {
                    yield return summons[i].OnCollisionPlayer();
                }
            }
        }
        
        private IEnumerator CheckArriveBoardObject()
        {
            var summons = GameManager.I.Stage.BoardObjects;
            for (int i = summons.Count - 1; i >= 0; i--)
            {
                if (_onTile == summons[i].OnTile)
                {
                    yield return summons[i].ArrivePlayer();
                }
            }
        }
        
        public IEnumerator CardAction(int num, BaseEntity target) {
            
            yield return _onTile.CardAction(num, target);
            yield return null;

            // [유물] 워프 부적
            /*if (GameManager.I.Player.PlayerInfo.CheckArtifactExist(Enums.ArtifactType.Warp)
                && num == 4)
            {
                GameManager.I.Stage.CardManager.WarpArtifact();
            }*/

        }
        
        [Button]
        public void TestAddBuff(BuffType buffType)
        {
           // AddBuff(new Slow());
            AddBuff(new Confusion());
        }

        public override void Attack(BaseEntity target, int damage)
        {
            Bubble?.SetBubble(BubbleText.attack);
            Animator?.Play("Attack");

            var targetPos = target.transform.position + new Vector3(0, -1, 0);
            var obj = Instantiate(ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Particle_Attack));
            obj.transform.position = transform.position;
            
            obj.transform.DOJump(targetPos, 3, 1, .8f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    target.Hit(CalculDamage(damage));
                    // var explosion = Instantiate(ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Particle_Explosion));
                    // explosion.transform.position = targetPos;
                     Destroy(obj);
                });
        }

        public void Defense(int value)
        {
            AddDefenseCount(value);
            Animator.Play("Shield");
        }

        [Button]
        public override int Heal(int value)
        {
            if (PlayerInfo.CheckBlessExist(BlessType.BlessWater2))
            {
                GameManager.I.Player.PlayerInfo.BlessEventDict[BlessType.BlessWater2]?.Invoke();
                int damage = (MaxHp -Hp) < value ? (MaxHp - _hp) : value;
                GameManager.I.Stage.Enemies[UnityEngine.Random.Range(0, GameManager.I.Stage.Enemies.Count)].Hit(damage);
            }

            return base.Heal(value);
        }
        public void Win()
        {   
            Animator.Play("Win");
            Bubble.SetBubble(BubbleText.win);
            for (int i = Buffs.Count - 1; i >= 0; i--)
            {
                Buffs[i].EndEvent();
            }
        }

        public void UpdateAction(PlayerActionType type)
        {
            if (type != CurActionType)
            {
                CurActionType = type;
                UpdateActionEvent?.Invoke(type);
            }
        }


        void SetFlipOnMoveByPosition(Tile moveTile)
        {
            bool filpX = true; // false일 때, 좌측을 바라봄

            var onTilePivot = (_onTile.transform.position.x + _onTile.transform.position.z) / 2;
            var nextTilePivot = (moveTile.transform.position.x + moveTile.transform.position.z) / 2;

            filpX = onTilePivot < nextTilePivot;
            Renderers.ForEach(r => Flip(r, filpX));

            var enemy = GameManager.I.Stage.Enemies.FirstOrDefault();
            if (enemy != null)
            {
                var enemyPivot = (enemy.transform.position.x + enemy.transform.position.z) / 2;
                bool enemyFlipX = enemyPivot < nextTilePivot;
                GameManager.I.Stage.Enemies.ForEach(e => Flip(e.Renderers.First(), enemyFlipX));
            }
        }

        void SetFlipTowardEnemy()
        {
            var enemy = GameManager.I.Stage.Enemies.FirstOrDefault();
            if (enemy != null)
            {
                var playerPivot = (transform.position.x + transform.position.z) / 2;
                var enemyPivot = (enemy.transform.position.x + enemy.transform.position.z) / 2;
                bool flipX = playerPivot < enemyPivot;
            
                Renderers.ForEach(r => Flip(r, flipX));
            }
        }

        void Flip(SpriteRenderer renderer,  bool filpX)
        {
            if (renderer.flipX != filpX)
            {
                renderer.transform.DORotate(new Vector3(0, 180, 0), 0.25f, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.InCirc)
                    .OnComplete(() =>
                    {
                        renderer.flipX = filpX;
                        renderer.transform.rotation = _defaultRotate;
                    });
            }
        }

        public void MotionThinking()
        {
            Animator.Play("Thinking");
        }

        public void MotionIdle()
        {
            Animator.Play("Idle");
        }
        
        public void MotionWorry()
        {
            Animator.Play("Worry");
        }

        public void SetGameOver()
        {
            Renderers[0].sortingOrder = 2;
            Renderers[1].sortingOrder = 3;
        }
    }
}

