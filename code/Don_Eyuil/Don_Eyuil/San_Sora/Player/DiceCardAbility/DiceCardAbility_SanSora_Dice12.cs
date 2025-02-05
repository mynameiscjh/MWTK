using Don_Eyuil.San_Sora.Player.Buff;

namespace Don_Eyuil.San_Sora.Player.DiceCardAbility
{
    public class DiceCardAbility_SanSora_Dice12 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]若本书页应用在[血枪]骰子上则在下一幕获得2层[迅捷]";

        public override void OnSucceedAttack()
        {
            var buf = BattleUnitBuf_SanSora.GetBuf<BattleUnitBuf_SanSora>(owner);
            if (buf == null || buf.Lance == null || buf.Lance.Card == null)
            {
                return;
            }
            if (card == buf.Lance.Card)
            {
                owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 2, owner);
            }
        }
    }
}
