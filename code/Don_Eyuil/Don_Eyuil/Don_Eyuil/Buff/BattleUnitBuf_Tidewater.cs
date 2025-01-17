using System.Collections.Generic;

namespace Don_Eyuil.Buff
{
    public class BattleUnitBuf_Tidewater : BattleUnitBuf
    {
        public static string Desc = "汹涌的血潮：所有敌方角色被施加\"流血\"时层数+x\r\n自身对处于流血状态的敌方角色造成的伤害与混乱伤害x×10%\r\n";

        public static void GainBuf(BattleUnitModel model, int v)
        {
            if (model.bufListDetail.HasBuf<BattleUnitBuf_Tidewater>())
            {
                (model.bufListDetail.GetFieldValue<List<BattleUnitBuf>>("_bufList").Find(x => x is BattleUnitBuf_Tidewater) as BattleUnitBuf_Tidewater).stack += v;
            }
            else
            {
                var buf = new BattleUnitBuf_Tidewater();
                buf.stack = v;
                model.bufListDetail.AddBuf(buf);
            }
        }

        public static BattleUnitBuf_Tidewater GetBuf(BattleUnitModel model)
        {
            return model.bufListDetail.GetFieldValue<List<BattleUnitBuf>>("_bufList").Find(x => x is BattleUnitBuf_Tidewater) as BattleUnitBuf_Tidewater;
        }

        public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
        {
            if (cardBuf.bufType == KeywordBuf.Bleeding)
            {
                return this.stack;
            }

            return base.OnGiveKeywordBufByCard(cardBuf, stack, target);
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (behavior.card.target.bufListDetail.HasBuf<BattleUnitBuf_bleeding>())
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = this.stack * 10, breakRate = this.stack * 10 });
            }
        }
    }
}
