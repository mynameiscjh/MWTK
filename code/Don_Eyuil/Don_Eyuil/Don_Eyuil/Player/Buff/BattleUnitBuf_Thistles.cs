using System.Collections.Generic;

namespace Don_Eyuil.Don_Eyuil.Buff
{
    /*public class BattleUnitBuf_Thistles : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "投掷骰子时使自身在下一幕中获得1层[流血](每幕至多触发x次)\r\n自身速度降低x/2\r\n每幕结束时层数减半\r\n";
        int count = 0;

        public BattleUnitBuf_Thistles(BattleUnitModel model) : base(model)
        {
        }

        public static void GainBuf(BattleUnitModel model, int v)
        {
            if (model.bufListDetail.HasBuf<BattleUnitBuf_Thistles>())
            {
                (model.bufListDetail.GetFieldValue<List<BattleUnitBuf>>("_bufList").Find(x => x is BattleUnitBuf_Thistles) as BattleUnitBuf_Thistles).stack += v;
            }
            else
            {
                var buf = new BattleUnitBuf_Thistles(model);
                buf.stack = v;
                model.bufListDetail.AddBuf(buf);
            }
        }

        public static void GainReadyBuf(BattleUnitModel model, int v)
        {
            if (model.bufListDetail.HasBuf<BattleUnitBuf_Thistles>())
            {
                (model.bufListDetail.GetFieldValue<List<BattleUnitBuf>>("_readyBufList").Find(x => x is BattleUnitBuf_Thistles) as BattleUnitBuf_Thistles).stack += v;
            }
            else
            {
                var buf = new BattleUnitBuf_Thistles(model);
                buf.stack = v;
                model.bufListDetail.AddReadyBuf(buf);
            }
        }


        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            if (count <= this.stack)
            {
                _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 1, _owner);
                count++;
            }
        }
        public override int GetSpeedDiceAdder(int speedDiceResult)
        {
            return -this.stack / 2;
        }
        public override void OnRoundEnd()
        {
            this.stack /= 2;
            count = 0;
            if (this.stack <= 0)
            {
                this.Destroy();
            }
        }
    }*/
}
