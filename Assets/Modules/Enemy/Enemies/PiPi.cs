using System.Linq;
using Cardinals;
using Cardinals.Enums;
using Cardinals.Game;

namespace Cardinals.Enemy
{
    public class PiPi : BaseEnemy
    {
        public override void Init(EnemyDataSO enemyData) {
            base.Init(enemyData);
            
            Patterns = new[]
            {
                new Pattern(EnemyActionType.Attack, 5),
                new Pattern(EnemyActionType.Defense, 5),
            };
            
            BerserkModeEvent += () =>
            {
                Patterns = new[]
                {
                    new Pattern(EnemyActionType.Attack, 10),
                    new Pattern(EnemyActionType.Defense, 5),
                };
                
                Turn = 0;
            };
            
            DieEvent += () =>
            {
                var pair = GameManager.I.CurrentEnemies.FirstOrDefault(e => e.GetType() == typeof(PoPo));
                
                if(pair != null) 
                    pair.BerserkMode = true;
            };
            
            
            Rewards = new Reward[]
            {
                new(RewardType.Gold, 50),
            };
        }
    }
}