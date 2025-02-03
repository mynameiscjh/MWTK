namespace Don_Eyuil.San_Sora.Player.DiceCardAbility
{
    public class DiceCardAbility_SanSora_Dice16 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]这一幕使目标被施加的[流血]层数+1";

        public override void OnSucceedAttack(BattleUnitModel target)
        {
            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_MoreBleed>(target, 1);
        }


        public class BattleUnitBuf_MoreBleed : BattleUnitBuf_Don_Eyuil
        {
            public override void BeforeAddKeywordBuf(KeywordBuf BufType, ref int Stack)
            {
                if (BufType == KeywordBuf.Bleeding)
                {
                    Stack += 1;
                }
            }

            public override void OnRoundEnd()
            {
                Destroy();
            }

            public BattleUnitBuf_MoreBleed(BattleUnitModel model) : base(model)
            {
            }
        }
    }
}
