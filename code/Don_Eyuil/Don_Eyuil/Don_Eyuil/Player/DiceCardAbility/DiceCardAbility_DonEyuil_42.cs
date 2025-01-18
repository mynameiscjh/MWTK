namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_42 : DiceCardAbilityBase
    {
        public static string Desc = "[拼点胜利]对目标施加2层[流血]";
        public override void OnWinParrying()
        {
            card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Binding, 2, owner);
        }
    }
}
