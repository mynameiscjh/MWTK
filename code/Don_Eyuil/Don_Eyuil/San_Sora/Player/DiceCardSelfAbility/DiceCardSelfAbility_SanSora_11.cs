using Don_Eyuil.San_Sora.Player.Buff;

namespace Don_Eyuil.San_Sora.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_SanSora_11 : DiceCardSelfAbilityBase
    {
        public static string Desc = "若本书页应用在[血弓]骰子上则使本书页第二颗骰子最小值+2 [使用时]消耗至多9层[血羽]每消耗3层便使本书页所有骰子最大值+1";
        int num = 0;

        public override void OnUseCard()
        {
            num = 0;
            if (BattleUnitBuf_Don_Eyuil.UseBuf<BattleUnitBuf_Feather>(owner, 3))
            {
                num++;
            }
            if (BattleUnitBuf_Don_Eyuil.UseBuf<BattleUnitBuf_Feather>(owner, 3))
            {
                num++;
            }
            if (BattleUnitBuf_Don_Eyuil.UseBuf<BattleUnitBuf_Feather>(owner, 3))
            {
                num++;
            }
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            var buf = BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_SanSora>(owner);
            if (behavior == card.GetDiceBehaviorList()[1] && buf != null && buf.Bow != null && buf.Bow.Card != null && buf.Bow.Card == card)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { min = 2 });
            }

            behavior.ApplyDiceStatBonus(new DiceStatBonus() { max = num });

        }
    }
}
