using Cardinals;
using Cardinals.Buff;
using Cardinals.Enums;

namespace Cardinals.Enemy
{
    public class Two : BaseEnemy
    {
        public Two(string name, int maxHp) : base(name, maxHp)
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
            GameManager.I.TempPlayer.AddBuff(new Slow());
        }
    }
}