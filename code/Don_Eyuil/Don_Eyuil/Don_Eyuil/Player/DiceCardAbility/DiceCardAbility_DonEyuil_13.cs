using Don_Eyuil.DiceCardSelfAbility;

namespace Don_Eyuil.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_13 : DiceCardAbilityBase
    {
        public static string Desc = "本书页每施加1层流血便使本骰子威力+1";

        public override void BeforeRollDice()
        {
            var buf = this.card.card.GetBufList().Find(x => x is DiceCardSelfAbility_DonEyuil_11.BattleDiceCardBuf_Temp) as DiceCardSelfAbility_DonEyuil_11.BattleDiceCardBuf_Temp;
            int power = buf != null ? buf.count : 0;
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = power });
        }

    }
}
