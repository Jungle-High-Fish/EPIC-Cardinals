using System.Linq;
using Cardinals.Enums;
using Cardinals.Game;

namespace Cardinals.Enemy
{
    public class Three2 : BaseEnemy
    {
        public override void Init(EnemyDataSO enemyData)
        {
            base.Init(enemyData);

            Patterns = new[]
            {
                new Pattern(EnemyActionType.Defense, 5),
                new Pattern(EnemyActionType.Attack, 5),
            };

            BerserkModeEvent += () =>
            {
                Patterns = new[]
                {
                    new Pattern(EnemyActionType.Defense, 10),
                    new Pattern(EnemyActionType.Attack, 5),
                };
                
                Turn = 0;
            };

            DieEvent += () =>
            {
                var pair = GameManager.I.CurrentEnemies.FirstOrDefault(e => e.GetType() == typeof(Three1));
                if(pair != null) 
                    pair.BerserkMode = true;
            };
        }
    }
}