using Cardinals;
using Cardinals.Buff;
using Cardinals.Enums;
using Cardinals.Game;

namespace Cardinals.Enemy
{
    public class Two : BaseEnemy
    {
        public Two(string name, int maxHp) : base("퉤퉤기", 10)
        {
            FixPattern = new Pattern(EnemyActionType.Buff, action: Slow);
            Patterns = new[]
            {
                new Pattern(EnemyActionType.Defense, 4),
                new Pattern(EnemyActionType.Attack, 4),
            };
        }

        private void Slow()
        {
            GameManager.I.Player.AddBuff(new Slow());
        }

        public override void Init()
        {
            base.Init("퉤퉤기", 10);

            FixPattern = new Pattern(EnemyActionType.Buff, action: Slow);
            Patterns = new[]
            {
                new Pattern(EnemyActionType.Defense, 4),
                new Pattern(EnemyActionType.Attack, 4),
            };
            
            Rewards = new Reward[]
            {
                new(RewardType.Gold, 60),
                new(RewardType.Potion, 1)
            };
        }
    }
}