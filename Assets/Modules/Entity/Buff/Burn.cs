using Cardinals.Enums;
using Cardinals.Game;

namespace Cardinals.Buff
{
    public class Burn : BaseBuff
    {
        public Burn(int count, int value = 1) : base(BuffType.Burn, count: count)
        {
            Value = value;
        }
        
        public override void Execute(BaseEntity entity)
        {
            // [축복] 그을린 상처: 보드를 한 바퀴 돌 때마다, 해당 전투에서 화상 데미지가 2씩 올라갑니다.
            if (entity as BaseEnemy &&
                GameManager.I.Player.PlayerInfo.CheckBlessExist(BlessType.BlessFire1))
            {
                entity.Hit(Value + (GameManager.I.Stage.CurEvent as BattleEvent).Round * 2);
                GameManager.I.Player.PlayerInfo.BlessEventDict[BlessType.BlessFire1]?.Invoke();
            }
            else
            {
                entity.Hit(Value);
            }
            
            base.Execute(entity);
        }
    }
}