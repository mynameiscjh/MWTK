using Don_Eyuil.San_Sora.Player.Buff;

namespace Don_Eyuil.San_Sora.Player.DiceCardAbility
{
    public class DiceCardAbility_SanSora_Dice19 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]若本书页应用在[血刃]骰子上则这一幕对目标施加3层[流血]";

        public override void OnSucceedAttack(BattleUnitModel target)
        {
            var buf = BattleUnitBuf_SanSora.GetBuf<BattleUnitBuf_SanSora>(owner);
            if (buf == null || buf.Blade == null || buf.Blade.Card == null)
            {
                return;
            }
            if (buf.Blade.Card == card)
            {
                target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 3, owner);
            }
        }
    }
}
