using System.Collections.Generic;
using System.Linq;
using Cardinals;
using Cardinals.Board;
using Cardinals.Buff;
using Cardinals.Enemy;
using Cardinals.Enums;
using Cardinals.Game;
using UnityEngine;

namespace Cardinals.Enemy
{
    public class Pazizizizic : BaseEnemy
    {
        public override int Hp
        {
            get => base.Hp;
            protected set
            {
                base.Hp = value;
                if (!BerserkMode && Hp < MaxHp / 2)
                {
                    BerserkMode = true;
                }
            }
        }

        public override void Init(EnemyDataSO enemyData)
        {
            base.Init(enemyData);
            
            Patterns = new[]
            {
                new Pattern(EnemyActionType.TileCurse, action: ThunderBolt),
                new Pattern(EnemyActionType.Attack, 10),
                new Pattern(EnemyActionType.Defense, 10),
            };

            BerserkModeEvent += () =>
            {
                Patterns = new Pattern[]
                {
                    new(EnemyActionType.TileCurse, action: BerserkThunderBolt),
                    new(EnemyActionType.Attack, 15),
                    new(EnemyActionType.Attack, 10),
                };
                FixPattern = new(EnemyActionType.UserDebuff, action: ElectricShock);
                Turn = 0;
            };

            Rewards = new Reward[]
            {
                new(RewardType.Gold, 100),
                new(RewardType.Artifact, 3),
            };
        }


        void ThunderBolt()
        {
            var list = GameManager.I.Stage.Board.GetCursedTilesList()?.ToList();
            if (list != null)
            {
                var index = Random.Range(0, list.Count);
                var tile = list[index];

                tile.SetCurse(TileCurseType.ThunderBolt, 3);
            }
        }

        void BerserkThunderBolt()
        {
            List<Tile> tiles = new();
            
            // 각 라인에서 타일 지정
            for (int i = 0; i < 4; i++)
            {
                var list = GameManager.I.Stage.Board.GetBoardEdgeTileSequence(i, false)
                    .Where(t => t.TileState == TileState.Normal)
                    .Where(t => GameManager.I.Player.OnTile != t).ToList();

                if (list.Count > 0)
                {
                    var idx = Random.Range(0, list.Count);
                    tiles.Add(list[idx]);
                }
            }
            
            // 3개 이하로 줄이기
            for (int i = tiles.Count; i > 3; i--)
            {
                int idx = Random.Range(0, tiles.Count());
                tiles.RemoveAt(idx);
            }
            
            // 저주 적용
            foreach (var tile in tiles)
            {
                tile.SetCurse(TileCurseType.ThunderBolt, 3);
            }
        }
        
        void ElectricShock()
        {
            GameManager.I.Player.AddBuff(new ElectricShock());
        }
        
    }
}