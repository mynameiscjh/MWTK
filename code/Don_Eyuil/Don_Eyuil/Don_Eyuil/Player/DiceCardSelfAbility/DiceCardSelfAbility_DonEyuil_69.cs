using Don_Eyuil.Don_Eyuil.Player.Buff;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_69 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]若自身应用了[血鞭]则使本书页施加[流血]时同时施加[血晶荆棘]";
        public override string[] Keywords => new string[] { "Bleeding_Keyword", "BattleUnitBuf_Thistles", "DonEyuil", "DonEyuil_9" };
        public override void OnUseCard()
        {
            if (BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(owner) != null && BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(owner).Scourge != null)
            {
                owner.bufListDetail.AddBuf(new BattleUnitBuf_AddThistles(owner));
            }
        }
        public class BattleUnitBuf_AddThistles : BattleUnitBuf_Don_Eyuil
        {
            public BattleUnitBuf_AddThistles(BattleUnitModel model) : base(model)
            {
            }
            public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
            {
                BattleUnitBuf_BloodCrystalThorn.GainBuf<BattleUnitBuf_BloodCrystalThorn>(target, stack);
                return base.OnGiveKeywordBufByCard(cardBuf, stack, target);
            }
        }
    }
}
