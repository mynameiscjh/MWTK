namespace Don_Eyuil.Don_Eyuil.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_16 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]对所有敌方角色施加共计15层[流血]";

        public override void OnSucceedAttack()
        {
            int temp = BattleObjectManager.instance.GetAliveList_opponent(owner.faction).Count;
            foreach (var item in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
            {
                item.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 15 / temp, owner);
            }
        }
    }
}
