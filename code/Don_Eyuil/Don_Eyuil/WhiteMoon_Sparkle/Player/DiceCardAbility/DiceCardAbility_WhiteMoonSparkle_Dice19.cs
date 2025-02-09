using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardAbility
{
    public class DiceCardAbility_WhiteMoonSparkle_Dice19 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]这一幕对目标施加1层[收尾标记]";

        public override void OnSucceedAttack()
        {
            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Flag>(card.target, 1);
        }
    }
}
