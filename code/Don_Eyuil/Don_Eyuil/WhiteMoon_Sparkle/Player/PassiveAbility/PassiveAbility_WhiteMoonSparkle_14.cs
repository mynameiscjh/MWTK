using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.PassiveAbility
{
    public class PassiveAbility_WhiteMoonSparkle_14 : PassiveAbilityBase_Don_Eyuil
    {
        public static string Desc = "自身可使用固有E.G.O书页“传承之梦” “月下终曲”\r\n“传承之梦”根据自身当前主武器改变且仅能在当前主武器强化后使用并根据当前所应用的副武器获得额外效果\r\n自身个人E.G.O书页将享受全队情感充能但仅能通过正面情感充能\r\n我方角色正面情感硬币溢出时使混乱抗性最低的1名我方角色恢复2点混乱抗性\r\n自身负面情感硬币溢出时使自身恢复1点体力\r\n(不可转移)";

        public override void OnWaveStart()
        {
            MyTools.未实现提醒();
            owner.personalEgoDetail.AddCard(MyId.未实现id);
            owner.personalEgoDetail.AddCard(MyId.未实现id);
            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Temp>(owner, 1);
        }

        [HarmonyPatch(typeof(BattleUnitEmotionDetail), "CreateEmotionCoin")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> BattleUnitEmotionDetail_CreateEmotionCoin_Tran(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            codes.InsertRange(codes.FindIndex(x => x.Calls(AccessTools.Method(typeof(SpecialCardListModel), "AddEgoCoolTime"))) + 1, new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                CodeInstruction.LoadField(typeof(BattleUnitEmotionDetail), "_self"),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Ldarg_2),
                CodeInstruction.CallClosure<Action<BattleUnitModel, EmotionCoinType, int>>((unit, coinType, count) =>
                {
                    if (unit.passiveDetail.HasPassive<PassiveAbility_WhiteMoonSparkle_14>() && coinType == EmotionCoinType.Positive)
                    {
                        unit.personalEgoDetail.AddEgoCoolTime(count);
                    }
                })
            });

            return codes.AsEnumerable();
        }

        public class BattleUnitBuf_Temp : BattleUnitBuf_Don_Eyuil
        {

            public override void BeforeAddEmotionCoin(EmotionCoinType CoinType, ref int Count)
            {
                if (_owner.emotionDetail.AllEmotionCoins.Count >= _owner.emotionDetail.MaximumCoinNumber)
                {
                    if (CoinType == EmotionCoinType.Positive)
                    {
                        var temp = BattleObjectManager.instance.GetAliveList(_owner.faction);
                        temp.Sort((x, y) => x.breakDetail.breakLife - y.breakDetail.breakLife);
                        if (temp.Count <= 0) { return; }
                        temp.First().RecoverBreakLife(2);
                    }
                    else
                    {
                        _owner.RecoverHP(1);
                    }
                }
            }

            public BattleUnitBuf_Temp(BattleUnitModel model) : base(model)
            {
            }
        }

    }
}
