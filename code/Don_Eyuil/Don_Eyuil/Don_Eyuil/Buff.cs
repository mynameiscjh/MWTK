using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Don_Eyuil
{
    //硬血结晶
    public class BattleUnitBuf_HardBlood_Crystal: BattleUnitBuf_Don_Eyuil
    {
        //至多30层
        //可配合硬血术效果
        public override int GetMaxStack() => 30;
        public BattleUnitBuf_HardBlood_Crystal(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this,TKS_BloodFiend_Initializer.ArtWorks["硬血结晶"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
            this.stack = 0;
        }
    }
    //无法凝结的血
    public class BattleUnitBuf_UncondensableBlood : BattleUnitBuf_Don_Eyuil
    {
        //自身流血无法低于2+x
        
        public static void UncodensableBloodCheck(BattleUnitBuf BleedingBuf)
        {
            var owner = BleedingBuf.GetFieldValue<BattleUnitModel>("_owner");
            if (owner != null && BattleUnitBuf_UncondensableBlood.GetBufStack<BattleUnitBuf_UncondensableBlood>(owner) > 0)
            {
                BleedingBuf.stack = Math.Max(BleedingBuf.stack, 2 + BattleUnitBuf_UncondensableBlood.GetBufStack<BattleUnitBuf_UncondensableBlood>(owner));
            }
        }

        [HarmonyPatch(typeof(BattleUnitBuf_bleeding), "AfterDiceAction")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> BattleUnitBuf_bleeding_AfterDiceAction_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            codes.InsertRange(codes.Count - 2, new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call,AccessTools.Method(typeof(BattleUnitBuf_UncondensableBlood),"UncodensableBloodCheck"))
            });
            return codes.AsEnumerable<CodeInstruction>();
        }

        public BattleUnitBuf_UncondensableBlood(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["无法凝结的血"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
            this.stack = 0;
        }
    }
    //热血尖枪
    public class BattleUnitBuf_WarmBloodLance : BattleUnitBuf_Don_Eyuil
    {
        //自身这一幕施加的"流血"翻倍
        public override int GetMultiplierOnGiveKeywordBufByCard(BattleUnitBuf cardBuf, BattleUnitModel target)
        {
            return cardBuf.bufType == KeywordBuf.Bleeding ? 2 : 1;
        }
        public BattleUnitBuf_WarmBloodLance(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["热血尖枪"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
            this.stack = 0;
        }
    }

}
