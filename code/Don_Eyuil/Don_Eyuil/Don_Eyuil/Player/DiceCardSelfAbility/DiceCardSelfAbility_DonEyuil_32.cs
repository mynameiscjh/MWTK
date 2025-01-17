namespace Don_Eyuil.Don_Eyuil.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_32 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页将同时命中所有敌方角色\r\n拼点失败时本书页依旧将击中目标但只施加[流血]\r\n";
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            foreach (var item in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
            {
                DiceCardSelfAbility_DonEyuil_01.GiveDamageForSubTarget(behavior, item);
            }
        }
        public override void OnLoseParrying()
        {
            foreach (var item in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
            {
                for (int i = 0; i < 3; i++)
                {
                    item.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 3, owner);
                }
            }
        }
    }
}
