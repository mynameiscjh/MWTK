using Don_Eyuil.Don_Eyuil.Buff;
using Don_Eyuil.Don_Eyuil.Player.Buff;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_74 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]若自身已经激活了[血鞭]则本书页命中目标时将额外施加2层[血晶荆棘]";
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(owner) != null && BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(owner).Scourge != null)
            {
                BattleUnitBuf_BloodCrystalThorn.GainBuf<BattleUnitBuf_BloodCrystalThorn>(behavior.card.target, 2);
            }
        }
    }
}
