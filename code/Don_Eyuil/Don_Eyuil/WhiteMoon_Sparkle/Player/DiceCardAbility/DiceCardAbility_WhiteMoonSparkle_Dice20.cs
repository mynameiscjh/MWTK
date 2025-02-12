using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardAbility
{
    public class DiceCardAbility_WhiteMoonSparkle_Dice20 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]这一幕对目标施加[全面洞悉]";

        public override void OnSucceedAttack()
        {
            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Know>(card.target, 1);
        }
    }
}
