/*using System;
using System.Collections.Generic;

namespace Don_Eyuil.Buff
{
    public class BattleUnitBuf_BleedCrystal : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "硬血结晶\r\n至多30层\r\n可配合硬血术效果\r\n";

        protected override string keywordId => "BattleUnitBuf_BleedCrystal";

        public BattleUnitBuf_BleedCrystal(BattleUnitModel model) : base(model)
        {
        }

        public override void Init(BattleUnitModel owner)
        {
            _owner = owner;
        }

        public static BattleUnitBuf_BleedCrystal GetBuf(BattleUnitModel model)
        {
            return model.bufListDetail.GetFieldValue<List<BattleUnitBuf>>("_bufList").Find(x => x is BattleUnitBuf_BleedCrystal) as BattleUnitBuf_BleedCrystal;
        }

        public static void GainBuf(BattleUnitModel model, int v)
        {
            if (model.bufListDetail.HasBuf<BattleUnitBuf_BleedCrystal>())
            {
                var buf = (model.bufListDetail.GetFieldValue<List<BattleUnitBuf>>("_bufList").Find(x => x is BattleUnitBuf_BleedCrystal) as BattleUnitBuf_BleedCrystal);
                buf.stack = Math.Min(buf.stack + v, 30);
            }
            else
            {
                var buf = new BattleUnitBuf_BleedCrystal(model);
                buf.stack = Math.Min(v, 30);
                model.bufListDetail.AddBuf(buf);
            }
        }

        public static bool UseBuf(BattleUnitModel model, int v)
        {
            if (BattleUnitBuf_BleedCrystal.GetBuf(model) == null || BattleUnitBuf_BleedCrystal.GetBuf(model).stack < v)
            {
                return false;
            }
            BattleUnitBuf_BleedCrystal.GetBuf(model).stack -= v;
            return true;
        }
    }

}
*/
