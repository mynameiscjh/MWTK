namespace Don_Eyuil.Don_Eyuil.Player.Buff
{
    public class BattleUnitBuf_Bow : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "自身每幕第一颗骰子最小值+3且命中目标时将在本幕对目标施加3层\"流血\"\r\n自身将对每幕第一个击中的目标施加\"深度创痕\"\r\n";
        bool fl1 = false;
        bool fl2 = false;

        public BattleUnitBuf_Bow(BattleUnitModel model) : base(model)
        {
        }

        protected override string keywordId => "BattleUnitBuf_Bow";

        public override void OnRoundStartAfter()
        {
            fl1 = false;
            fl2 = false;
            base.OnRoundStartAfter();
        }
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            if (!fl1)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { min = 3 });
                behavior.AddFlag((DiceFlag)139);
                fl1 = true;
            }
        }

        public override void OnSuccessAttack(BattleDiceBehavior behavior)
        {
            if (behavior.HasFlag((DiceFlag)139))
            {
                behavior.card.target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 3, _owner);
            }
            if (!fl2)
            {
                behavior.card.target.bufListDetail.AddBuf(new BattleUnitBuf_DeepWound(behavior.card.target));
                fl2 = true;
            }
        }

    }
}
