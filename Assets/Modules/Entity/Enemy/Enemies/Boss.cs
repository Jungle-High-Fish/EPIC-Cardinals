using Cardinals;
using Cardinals.Enemy;
using Cardinals.Enums;
using Cardinals.Game;

namespace Cardinals.Enemy
{
    public class Boss : BaseEnemy
    {
        public override int Hp
        {
            get => base.Hp;
            set
            {
                base.Hp = value;
                if (!BerserkMode && Hp < MaxHp / 2)
                {
                    BerserkMode = true;
                }
            }
        }
        
        public Boss(string name, int maxHp) : base("파지지지직이", 40)
        {
            Patterns = new[]
            {
                new Pattern(EnemyActionType.Magic, action: Electric),
                new Pattern(EnemyActionType.Attack, 5),
                new Pattern(EnemyActionType.Defense, 5),
            };

            BerserkModeEvent += () =>
            {
                FixPattern = new(EnemyActionType.Buff, action: ElectricShock);
                Patterns = new Pattern[]
                {
                    new(EnemyActionType.Magic, action: BerserkElectric),
                    new(EnemyActionType.Attack, 7),
                    new(EnemyActionType.Attack, 5),
                };
                Turn = 0;
                Sprite = null; // [TODO] 광폭화 이미지로 변경 필요 
            };
        }

        public override void Init()
        {
            base.Init("파지지지직이", 40);
            
            Patterns = new[]
            {
                new Pattern(EnemyActionType.Magic, action: Electric),
                new Pattern(EnemyActionType.Attack, 5),
                new Pattern(EnemyActionType.Defense, 5),
            };

            BerserkModeEvent += () =>
            {
                FixPattern = new(EnemyActionType.Buff, action: ElectricShock);
                Patterns = new Pattern[]
                {
                    new(EnemyActionType.Magic, action: BerserkElectric),
                    new(EnemyActionType.Attack, 7),
                    new(EnemyActionType.Attack, 5),
                };
                Turn = 0;
                Sprite = null; // [TODO] 광폭화 이미지로 변경 필요 
            };

            Rewards = new Reward[]
            {
                new(RewardType.Gold, 100),
                new(RewardType.Artifact, 3),
                new(RewardType.Card, 1),
            };
        }
            

        void Electric()
        {
            // 전류 x1
        }

        void BerserkElectric()
        {
            // 전류 x3
        }

        void ElectricShock()
        {
            // 감전
        }
    }
}