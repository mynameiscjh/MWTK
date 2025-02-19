﻿namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_59 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]对目标下两幕施加3层[无法凝结的血]";
        public override string[] Keywords => new string[] { "BattleUnitBuf_Flow" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            BattleUnitBuf_UncondensableBlood.GainBuf<BattleUnitBuf_UncondensableBlood>(target, 3, BufReadyType.NextRound);
            BattleUnitBuf_UncondensableBlood.GainBuf<BattleUnitBuf_UncondensableBlood>(target, 3, BufReadyType.NextNextRound);
        }
    }
}
