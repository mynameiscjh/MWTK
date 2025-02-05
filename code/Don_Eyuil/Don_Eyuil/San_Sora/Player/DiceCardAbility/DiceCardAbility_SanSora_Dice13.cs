using Don_Eyuil.San_Sora.Player.Buff;

namespace Don_Eyuil.San_Sora.Player.DiceCardAbility
{
    public class DiceCardAbility_SanSora_Dice13 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]消耗3层[血羽]对目标下两幕对目标施加4层[流血]";

        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if (BattleUnitBuf_Don_Eyuil.UseBuf<BattleUnitBuf_Feather>(owner, 3))
            {
                target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 4, owner);
                target.bufListDetail.AddKeywordBufNextNextByCard(KeywordBuf.Bleeding, 4, owner);
            }
        }
    }
}
