namespace Don_Eyuil.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_11 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页命中时将使本书页施加的[流血]层数+1";

        public override void OnUseCard()
        {
            var buf = new BattleDiceCardBuf_Temp();
            buf.SetStack(1);
            this.card.card.AddBuf(buf);
        }

        public class BattleDiceCardBuf_Temp : BattleDiceCardBuf
        {
            public int count = 0;

            public void SetStack(int v)
            {
                this._stack = v;
            }
            public override int OnAddKeywordBufByCard(BattleUnitBuf cardBuf, int stack)
            {
                if (cardBuf.bufType == KeywordBuf.Bleeding)
                {
                    count += stack + this._stack;
                    return this._stack;
                }

                return base.OnAddKeywordBufByCard(cardBuf, stack);
            }
            public override void OnRoundEnd()
            {
                this.Destroy();
            }
        }
    }
}
