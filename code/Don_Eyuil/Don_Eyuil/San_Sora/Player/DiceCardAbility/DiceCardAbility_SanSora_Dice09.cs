using HarmonyLib;
using System;

namespace Don_Eyuil.San_Sora.Player.DiceCardAbility
{
    public class DiceCardAbility_SanSora_Dice09 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]这一幕使目标的[流血]无法低于与自身的速度差值";

        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if (target.bufListDetail.HasBuf<BattleUnitBuf_Lock>())
            {
                return;
            }
            target.bufListDetail.AddBuf(new BattleUnitBuf_Lock(target) { stack = card.speedDiceResultValue - target.GetSpeedDiceResult(card.targetSlotOrder).value });
        }

        [HarmonyPatch(typeof(BattleUnitBuf_bleeding), "AfterDiceAction")]
        [HarmonyPostfix]
        public static void BattleUnitBuf_bleeding_AfterDiceAction_Post(BattleUnitBuf_bleeding __instance, BattleUnitModel ____owner)
        {
            if (____owner.bufListDetail.HasBuf<BattleUnitBuf_Lock>())
            {
                __instance.stack = Math.Max(__instance.stack, BattleUnitBuf_Don_Eyuil.GetBufStack<BattleUnitBuf_Lock>(____owner));
            }
        }

        public class BattleUnitBuf_Lock : BattleUnitBuf_Don_Eyuil
        {
            public BattleUnitBuf_Lock(BattleUnitModel model) : base(model)
            {
            }

            public override void OnRoundEnd()
            {
                Destroy();
            }
        }
    }
}
