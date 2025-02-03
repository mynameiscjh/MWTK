namespace Don_Eyuil.San_Sora.Player.DiceCardAbility
{
    public class DiceCardAbility_SanSora_Dice08 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]这一幕与下两幕对目标施加4层[流血]";

        public override void OnSucceedAttack(BattleUnitModel target)
        {
            target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Bleeding, 4, owner);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 4, owner);
            target.bufListDetail.AddKeywordBufNextNextByCard(KeywordBuf.Bleeding, 4, owner);
        }
    }
}
