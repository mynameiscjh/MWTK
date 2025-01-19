using HarmonyLib;
using System.Collections.Generic;

namespace Don_Eyuil.Buff
{
   /* public class BattleUnitBuf_Flow : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "自身[流血]无法低于2+x";

        public BattleUnitBuf_Flow(BattleUnitModel model) : base(model)
        {
        }

        public static BattleUnitBuf_Flow GetBuf(BattleUnitModel model)
        {
            return model.bufListDetail.GetFieldValue<List<BattleUnitBuf>>("_bufList").Find(x => x is BattleUnitBuf_Flow) as BattleUnitBuf_Flow;
        }

        public static void GainBuf(BattleUnitModel model, int v)
        {
            if (model.bufListDetail.HasBuf<BattleUnitBuf_Flow>())
            {
                (model.bufListDetail.GetFieldValue<List<BattleUnitBuf>>("_bufList").Find(x => x is BattleUnitBuf_Flow) as BattleUnitBuf_Flow).stack += v;
            }
            else
            {
                var buf = new BattleUnitBuf_Flow(model);
                buf.stack = v;
                model.bufListDetail.AddBuf(buf);
            }
        }

        public static void GainReadyBuf(BattleUnitModel model, int v)
        {
            var buf = new BattleUnitBuf_Flow(model);
            buf.stack = v;
            model.bufListDetail.AddReadyBuf(buf);
        }

        public static void GainReadyReadyBuf(BattleUnitModel model, int v)
        {
            var buf = new BattleUnitBuf_Flow(model);
            buf.stack = v;
            model.bufListDetail.AddReadyReadyBuf(buf);
        }

        [HarmonyPatch(typeof(BattleUnitBuf_bleeding), "AfterDiceAction")]
        [HarmonyPostfix]
        public static void BattleUnitBuf_bleeding_AfterDiceAction_Post(BattleUnitModel ____owner, BattleUnitBuf_bleeding __instance)
        {
            var temp = ____owner.bufListDetail.GetFieldValue<List<BattleUnitBuf>>("_bufList").Find(x => x is BattleUnitBuf_Flow) as BattleUnitBuf_Flow;
            if (temp != null)
            {
                if (__instance.stack < temp.stack + 2)
                {
                    __instance.stack = temp.stack + 2;
                }
            }
        }
    }*/
}
