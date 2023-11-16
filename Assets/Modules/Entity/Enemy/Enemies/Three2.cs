using System.Linq;
using Cardinals.Enums;

namespace Cardinals.Enemy
{
    public class Three2 : BaseEnemy
    {
        public Three2(string name, int maxHp) : base(name, maxHp)
        {
            Patterns = new[]
            {
                new Pattern(EnemyActionType.Defense, 5),
                new Pattern(EnemyActionType.Attack, 5),
            };

            BerserkModeEvent += () =>
            {
                Patterns = new[]
                {
                    new Pattern(EnemyActionType.Attack, 10),
                    new Pattern(EnemyActionType.Defense, 5),
                };
                
                Turn = 0;
                Sprite = null; // [TODO] 광폭화 이미지로 변경 필요 
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