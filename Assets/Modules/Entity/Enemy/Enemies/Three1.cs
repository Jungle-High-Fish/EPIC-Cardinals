using System.Linq;
using Cardinals;
using Cardinals.Enums;

namespace Cardinals.Enemy
{
    public class Three1 : BaseEnemy
    {
        public Three1(string name, int maxHp) : base("삐삐", 15)
        {
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
                Sprite = null; // [TODO] 광폭화 이미지로 변경 필요 
            };
            
            DieEvent += () =>
            {
                var pair = GameManager.I.CurrentEnemies.FirstOrDefault(e => e.GetType() == typeof(Three2));
                
                if(pair != null) 
                    pair.BerserkMode = true;
            };
        }
        
        public override void Init() {
            base.Init("삐삐", 15);
            
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
                Sprite = null; // [TODO] 광폭화 이미지로 변경 필요 
            };
            
            DieEvent += () =>
            {
                var pair = GameManager.I.CurrentEnemies.FirstOrDefault(e => e.GetType() == typeof(Three2));
                
                if(pair != null) 
                    pair.BerserkMode = true;
            };
        }
    }
}