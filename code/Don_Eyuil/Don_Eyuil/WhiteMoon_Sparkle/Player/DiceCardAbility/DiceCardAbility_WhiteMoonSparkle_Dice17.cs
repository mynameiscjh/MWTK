namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardAbility
{
    public class DiceCardAbility_WhiteMoonSparkle_Dice17 : DiceCardAbilityBase
    {
        public static string Desc = "[拼点胜利]摧毁目标书页剩余骰子[命中时]追加5点伤害";

        public override void OnWinParrying()
        {
            behavior.TargetDice.card.DestroyDice(DiceMatch.AllDice);
        }
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            target.TakeDamage(5);
        }
    }
}
