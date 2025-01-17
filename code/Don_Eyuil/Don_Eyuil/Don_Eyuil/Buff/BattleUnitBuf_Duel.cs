namespace Don_Eyuil.Buff
{
    public class BattleUnitBuf_Duel : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "拼点时双方所有骰子威力+3\r\n拼点胜利的一方造成的伤害增加25%\r\n自身受到的单方面伤害与混乱伤害减少50%\r\n";

        public BattleUnitBuf_Duel(BattleUnitModel model) : base(model)
        {
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (behavior.TargetDice != null)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 3 });
                behavior.TargetDice.ApplyDiceStatBonus(new DiceStatBonus() { power = 3 });
            }
        }

        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmgRate = 25
            });
        }

        public override void OnLoseParrying(BattleDiceBehavior behavior)
        {
            behavior.TargetDice.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmgRate = 25
            });
        }

        public override void OnStartTargetedOneSide(BattlePlayingCardDataInUnitModel attackerCard)
        {
            attackerCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { dmgRate = -50, breakRate = -50 });
        }
    }

}
