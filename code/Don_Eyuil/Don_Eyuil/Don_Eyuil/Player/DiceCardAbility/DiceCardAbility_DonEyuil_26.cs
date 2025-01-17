namespace Don_Eyuil.Don_Eyuil.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_26 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]这一幕对目标施加3层[流血]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Bleeding, 3, owner);
        }
    }

}
