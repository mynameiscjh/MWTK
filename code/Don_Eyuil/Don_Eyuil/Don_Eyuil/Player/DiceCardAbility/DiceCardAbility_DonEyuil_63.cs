namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_63 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]使目标本幕内流血层数不再衰减";
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            target.bufListDetail.AddBuf(new BattleUnitBuf_BleedLock());
        }

        public class BattleUnitBuf_BleedLock : BattleUnitBuf
        {
            public override KeywordBuf bufType => KeywordBuf.BloodStackBlock;
        }
    }
}
