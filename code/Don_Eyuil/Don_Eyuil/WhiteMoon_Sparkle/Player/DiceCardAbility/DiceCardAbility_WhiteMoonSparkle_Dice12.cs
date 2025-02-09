namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardAbility
{
    public class DiceCardAbility_WhiteMoonSparkle_Dice12 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]下一幕使所有我方角色获得1层迅捷与忍耐";

        public override void OnSucceedAttack()
        {
            foreach (var item in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                item.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 1, owner);
                item.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 1, owner);
            }
        }
    }
}
