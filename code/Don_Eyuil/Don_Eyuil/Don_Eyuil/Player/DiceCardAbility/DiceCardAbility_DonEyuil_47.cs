namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_47 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]这一幕与下一幕施加4层[流血]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Bleeding, 4, owner);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 4, owner);
        }
    }
}
