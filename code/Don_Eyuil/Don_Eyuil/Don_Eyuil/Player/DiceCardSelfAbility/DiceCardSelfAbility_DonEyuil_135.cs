using Don_Eyuil.Don_Eyuil.Player.Buff;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_135 : DiceCardSelfAbilityBase
    {
        public static string Desc = "仅限装备了血镰时装备\r\n[持有时]自身每施加5层[流血]便使本书页费用-1(使用后复原)";

        public override bool OnChooseCard(BattleUnitModel owner)
        {
            var buf = BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(owner);

            if (buf == null || buf.Scourge == null)
            {
                return false;
            }

            return base.OnChooseCard(owner);
        }

        public override void OnAddToHand(BattleUnitModel owner)
        {
            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_BleedCount>(owner, 1);
        }

        public override int GetCostAdder(BattleUnitModel unit, BattleDiceCardModel self)
        {
            var buf = BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_BleedCount>(unit);

            if (buf != null)
            {
                return -buf.count / 5;
            }

            return base.GetCostAdder(unit, self);
        }

        public override void OnUseCard()
        {
            BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_BleedCount>(owner)?.Destroy();
        }

        public class BattleUnitBuf_BleedCount : BattleUnitBuf_Don_Eyuil
        {
            public int count = 0;

            public override void AfterOtherUnitAddKeywordBuf(BattleUnitModel Adder, KeywordBuf BufType, BattleUnitModel Target, ref int Stack)
            {
                if (Adder == _owner && BufType == KeywordBuf.Bleeding)
                {
                    count += Stack;
                }
            }

            public BattleUnitBuf_BleedCount(BattleUnitModel model) : base(model)
            {
            }
        }
    }
}
