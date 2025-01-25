using Don_Eyuil.PassiveAbility;

namespace Don_Eyuil.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_60 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]对目标施加1层[流血](重复触发自身激活的硬血术书页次)";
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            target.bufListDetail.AddKeywordBufNextNextByCard(KeywordBuf.Bleeding, PassiveAbility_DonEyuil_15.count);
        }

    }
}
