using LOR_BattleUnit_UI;

namespace Don_Eyuil.San_Sora.Player.Buff
{
    public class BattleUnitBuf_Sickle : BattleUnitBuf_SanHardBlood
    {
        public static string Desc = "若自身血羽与结晶硬血之和不低于30本速度骰子中使用书页的第一颗进攻型骰子将额外造\r\n成一次伤害";

        public BattleUnitBuf_Sickle(SpeedDiceUI dice) : base(dice)
        {
            //dice.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = TKS_BloodFiend_Initializer.ArtWorks["血镰骰子"];
        }

        bool fl = false;
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            if (fl) { return; }

            if (GetBufStack<BattleUnitBuf_Feather>(_owner) + GetBufStack<BattleUnitBuf_Crystal_HardBlood>(_owner) >= 30 && behavior.card == Card && behavior.Type == LOR_DiceSystem.BehaviourType.Atk)
            {
                fl = true;
                behavior.GiveDamage(behavior.card.target);
            }

            base.BeforeRollDice(behavior);
        }
    }
}
