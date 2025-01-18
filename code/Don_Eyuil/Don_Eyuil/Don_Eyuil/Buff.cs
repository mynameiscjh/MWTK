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
        public override void OnRoundEnd()
        {
            this.Destroy();
        }

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

        public override void OnRoundEnd()
        {
            this.Destroy();
        }
    }
    //深度创痕
    public class BattleUnitBuf_DeepWound : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "这一幕受到的\"流血\"伤害增加50%";
        public BattleUnitBuf_DeepWound(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["深度创痕"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
            this.stack = 0;
        }
        public override void OnRoundEnd()
        {
            this.Destroy();
        }
        public override float DmgFactor(int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None)
        {
            if (keyword == KeywordBuf.Bleeding)
            {
                return 1.5f;
            }
            return base.DmgFactor(dmg, type, keyword);
        }
    }
    //血晶荆棘
    public class BattleUnitBuf_BloodCrystalThorn : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "投掷骰子时使自身在下一幕中获得1层[流血](每幕至多触发x次) 自身速度降低x/2 每幕结束时层数减半";
        public BattleUnitBuf_BloodCrystalThorn(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["血晶荆棘"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
            this.stack = 0;
        }
        public int TriggeredOnRollDiceCount = 0;
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            TriggeredOnRollDiceCount++;
            if(TriggeredOnRollDiceCount <= this.stack)
            {
                _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 1);
            }
        }
        public override int GetSpeedDiceAdder(int speedDiceResult)
        {
            return -this.stack / 2;
        }
        public override void OnRoundEnd()
        {
            this.stack /= 2;
            if (this.stack <= 0)
            {
                this.Destroy();
            }
            TriggeredOnRollDiceCount = 0;
        }
    }
    //汹涌的血潮(不衰减）
    public class BattleUnitBuf_BloodTide : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "所有敌方角色被施加\"流血\"时层数+x\r\n自身对处于流血状态的敌方角色造成的伤害与混乱伤害x×10%";
        public BattleUnitBuf_BloodTide(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["汹涌的血潮"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
            this.stack = 1;
        }
        public override void BeforeOtherUnitAddKeywordBuf(KeywordBuf BufType, BattleUnitModel Target, ref int Stack)
        {
            if(bufType == KeywordBuf.Bleeding && Target.faction != _owner.faction )
            {
                Stack += this.stack;
            }
        }
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            if(behavior != null && behavior.card!=null && behavior.card.target != null)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmgRate = 10 * stack,
                    breakRate = 10 * stack,
                });
            }
        }
    }


}
