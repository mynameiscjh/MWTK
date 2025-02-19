namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_Dice_28 : DiceCardAbilityBase
    {
        public static string Desc = "[投掷前]本书页使用期间自身[流血]不再衰减";
        public override void BeforeRollDice()
        {
            owner.bufListDetail.AddBuf(new BattleUnitBuf_BleedLock());
        }

        public class BattleUnitBuf_BleedLock : BattleUnitBuf
        {
            public override KeywordBuf bufType => KeywordBuf.BloodStackBlock;

            public override void OnEndBattle(BattlePlayingCardDataInUnitModel curCard)
            {
                Destroy();
            }
        }
    }
}
