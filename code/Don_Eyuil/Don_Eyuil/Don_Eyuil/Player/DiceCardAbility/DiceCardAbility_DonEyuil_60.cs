using Don_Eyuil.Don_Eyuil.Player.Buff;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_60 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]对目标施加1层[流血](重复触发自身激活的硬血术书页次)";
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            for (int i = 0; i < BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(owner).ActivatedNum; i++)
            {
                target.bufListDetail.AddKeywordBufNextNextByCard(KeywordBuf.Bleeding, 1);
            }
        }

    }
}
