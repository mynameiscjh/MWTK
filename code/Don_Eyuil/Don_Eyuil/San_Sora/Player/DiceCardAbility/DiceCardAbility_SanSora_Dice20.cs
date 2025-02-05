namespace Don_Eyuil.San_Sora.Player.DiceCardAbility
{
    public class DiceCardAbility_SanSora_Dice20 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]下两幕对目标施加3层[流血]";

        public override void OnSucceedAttack(BattleUnitModel target)
        {
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 3, owner);
            target.bufListDetail.AddKeywordBufNextNextByCard(KeywordBuf.Bleeding, 3, owner);
        }
    }
}
