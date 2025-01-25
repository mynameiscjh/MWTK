namespace Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_72 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]抽取1张书页并在下一幕使自身获得2层[迅捷]\r\n若自身速度不低于8则额外抽取1张书页\r\n";
        public override string[] Keywords => new string[] { "DonEyuil", "DonEyuil_2" };
        public override void OnUseCard()
        {
            owner.allyCardDetail.DrawCards(1);
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 2, owner);
            if (card.speedDiceResultValue >= 8)
            {
                owner.allyCardDetail.DrawCards(1);
            }
        }
    }
}
