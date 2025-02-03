namespace Don_Eyuil.San_Sora.Player.DiceCardAbility
{
    public class DiceCardAbility_SanSora_Dice14 : DiceCardAbilityBase
    {
        public static string Desc = "[拼点胜利]对目标施加3层[流血]";

        public override void OnWinParrying()
        {
            card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 3, owner);
        }
    }
}
