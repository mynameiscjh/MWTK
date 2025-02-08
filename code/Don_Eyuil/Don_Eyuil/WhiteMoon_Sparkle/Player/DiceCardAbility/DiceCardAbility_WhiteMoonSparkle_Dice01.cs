using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardAbility
{
    public class DiceCardAbility_WhiteMoonSparkle_Dice01 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]下一幕对目标施加1层[收尾标记]";

        public override void OnSucceedAttack(BattleUnitModel target)
        {
            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Flag>(target, 1);
        }
    }
}
