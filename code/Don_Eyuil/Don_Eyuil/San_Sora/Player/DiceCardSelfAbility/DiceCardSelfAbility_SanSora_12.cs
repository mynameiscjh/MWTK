using Don_Eyuil.San_Sora.Player.Buff;
using System;

namespace Don_Eyuil.San_Sora.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_SanSora_12 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本舞台每消耗10层血羽便使本书页费用-1(至多-2) 本书页击中目标时目标每有2层流血便使本书页造成的伤害增加10%(至多100%)";
        public override int GetCostAdder(BattleUnitModel unit, BattleDiceCardModel self)
        {
            int temp = 0;
            if (BattleUnitBuf_Feather.UsedStack >= 10)
            {
                temp++;
            }
            if (BattleUnitBuf_Feather.UsedStack >= 20)
            {
                temp++;
            }
            return -temp;
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = behavior.card.target.bufListDetail.HasBuf<BattleUnitBuf_bleeding>() ? Math.Min(behavior.card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding).stack * 5, 100) : 0 });
        }
    }
}
