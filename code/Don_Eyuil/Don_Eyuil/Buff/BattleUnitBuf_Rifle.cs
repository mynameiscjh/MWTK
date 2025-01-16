namespace Don_Eyuil.Buff
{
    public class BattleUnitBuf_Rifle : BattleUnitBuf
    {
        public static string Desc = "自身这一幕施加的\"流血\"翻倍";
        public override int GetMultiplierOnGiveKeywordBufByCard(BattleUnitBuf cardBuf, BattleUnitModel target)
        {
            if (cardBuf.bufType == KeywordBuf.Bleeding)
            {
                return 2;
            }

            return base.GetMultiplierOnGiveKeywordBufByCard(cardBuf, target);
        }

        public override void OnRoundEnd()
        {
            this.Destroy();
        }
    }
}
