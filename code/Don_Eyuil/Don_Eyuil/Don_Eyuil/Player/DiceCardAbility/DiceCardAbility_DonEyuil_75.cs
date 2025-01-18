using Don_Eyuil.Don_Eyuil.Player.Buff;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_75 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]使目标这一幕获得[流血]时层数+1若自身已经激活了[血弓]则额外+1";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            var buf = new BattleUnitBuf_MoreBleed() { stack = 1 };
            if (BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(owner) != null && BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(owner).Bow != null)
            {
                buf.stack = 2;
            }
            target.bufListDetail.AddBuf(buf);
        }
        public class BattleUnitBuf_MoreBleed : BattleUnitBuf
        {
            public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
            {
                if (cardBuf.bufType == KeywordBuf.Bleeding)
                {
                    return this.stack;
                }
                return base.OnGiveKeywordBufByCard(cardBuf, stack, target);
            }
        }
    }
}
