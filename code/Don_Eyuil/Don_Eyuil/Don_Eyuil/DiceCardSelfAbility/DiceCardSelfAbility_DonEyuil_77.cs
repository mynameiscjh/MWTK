namespace Don_Eyuil.Don_Eyuil.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_77 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]这一幕对自身施加3层流血";
        public int winCount = 0;
        public override void OnUseCard()
        {
            winCount = 0;
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 3, owner);
        }

        public override void OnWinParryingAtk()
        {
            winCount++;
        }
        public override void OnWinParryingDef()
        {
            winCount++;
        }
    }
}
