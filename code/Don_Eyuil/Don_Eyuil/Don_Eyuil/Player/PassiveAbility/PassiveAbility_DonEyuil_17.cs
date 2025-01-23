using Don_Eyuil.Buff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Don_Eyuil.PassiveAbility
{
    public class PassiveAbility_DonEyuil_17 : PassiveAbilityBase
    {
        public override string debugDesc => "在一幕中自身每获得3点正面情感便使自身在下一幕中获得1层\"强壮\"与\"迅捷\"(至多2层)\r\n获得正面情感时若自身情感槽已被填满则使情感最低的一名友方角色获得1点正面情感\r\n若开启舞台时只有自身一名我方角色…\r\n(获得效果 光荣的决斗)\r\n(不可转移)\r\n";
        public int posNum = 0;
        public override void OnRoundStart()
        {
            posNum = owner.emotionDetail.PositiveCoins.Count;

            if (BattleObjectManager.instance.GetAliveList(owner.faction).Count == 1 && !owner.IsDead())
            {
                BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Duel>(owner, 0);
            }

        }
        public override void OnRoundEnd()
        {
            int temp = owner.emotionDetail.PositiveCoins.Count;

            owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, Math.Min((temp - posNum) / 3, 2));
            owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Quickness, Math.Min((temp - posNum) / 3, 2));
        }

        [HarmonyPatch(typeof(BattleUnitEmotionDetail), "CreateEmotionCoin")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> BattleUnitEmotionDetail_CreateEmotionCoin_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            var index = codes.FindIndex(x => x.opcode == OpCodes.Ldloc_3);
            codes.InsertRange(index, new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Ldarg_2),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PassiveAbility_DonEyuil_17), "Func"))
            });
            return codes.AsEnumerable();
        }

        public static void Func(BattleUnitEmotionDetail emotionDetail, EmotionCoinType coinType, int count)
        {
            if (emotionDetail.AllEmotionCoins.Count >= emotionDetail.MaximumCoinNumber && coinType == EmotionCoinType.Positive)
            {
                var temp = new List<BattleUnitModel>(BattleObjectManager.instance.GetAliveList(emotionDetail.GetFieldValue<BattleUnitModel>("_self").faction));
                temp.Remove(emotionDetail.GetFieldValue<BattleUnitModel>("_self"));
                temp.Sort((x, y) => x.emotionDetail.EmotionLevel - y.emotionDetail.EmotionLevel);
                temp.First().emotionDetail.CreateEmotionCoin(EmotionCoinType.Positive, 1);
            }
        }

    }

}
