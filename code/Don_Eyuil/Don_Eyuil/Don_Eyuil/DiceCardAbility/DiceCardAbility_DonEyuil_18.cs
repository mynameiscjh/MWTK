namespace Don_Eyuil.Don_Eyuil.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_18 : DiceCardAbilityBase
    {
        public static string Desc = "[拼点胜利]这一幕对双方施加3层[流血]";

        public override void OnWinParrying()
        {
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 3, owner);
            behavior.card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 3, owner);
        }
    }
}
