using Don_Eyuil.Don_Eyuil.Buff;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_68 : DiceCardAbilityBase
    {
        public static string Desc = "[拼点胜利]下一幕对目标施加2层[流血]与[血晶荆棘]";
        public override void OnWinParrying()
        {
            this.card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 2, owner);
            BattleUnitBuf_Thistles.GainReadyBuf(this.card.target, 2);
        }
    }
}
