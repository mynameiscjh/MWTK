namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_02 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]这一幕对双方施加3层[流血]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 3, owner);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 3, owner);
        }
    }
}
