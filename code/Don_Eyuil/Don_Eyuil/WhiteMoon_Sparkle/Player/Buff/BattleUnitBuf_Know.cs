using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.Buff
{
    public class BattleUnitBuf_Know : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "全面洞悉[中立buff]:\r\n这一幕自身投出的点数无法超过骰子最大值\r\n";

        [HarmonyPatch(typeof(BattleDiceBehavior), "UpdateDiceFinalValue")]
        [HarmonyPostfix]
        public static void BattleDiceBehavior_UpdateDiceFinalValue_Post(BattlePlayingCardDataInUnitModel ___card, ref int ____diceFinalResultValue, BattleDiceBehavior __instance)
        {
            if (___card.owner.bufListDetail.HasBuf<BattleUnitBuf_Know>())
            {
                ____diceFinalResultValue = Math.Min(____diceFinalResultValue, __instance.GetDiceMax());
            }
        }

        public override void OnRoundEnd()
        {
            Destroy();
        }

        public BattleUnitBuf_Know(BattleUnitModel model) : base(model)
        {
        }
    }
}
