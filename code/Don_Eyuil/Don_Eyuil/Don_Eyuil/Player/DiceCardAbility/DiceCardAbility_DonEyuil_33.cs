namespace Don_Eyuil.Don_Eyuil.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_33 : DiceCardAbilityBase
    {
        public static string Desc = "[拼点胜利]摧毁目标书页所有骰子[命中时]施加2层[流血](重复触发3次)";

        public override void OnWinParrying()
        {
            card.target.currentDiceAction.DestroyDice(DiceMatch.AllDice);
        }
        public override void OnSucceedAttack()
        {
            foreach (var item in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
            {
                for (int i = 0; i < 3; i++)
                {
                    item.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 2, owner);
                }
            }
        }
    }
}
