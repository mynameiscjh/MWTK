namespace Don_Eyuil.San_Sora.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_SanSora_09 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]若自身速度不低于6则使本书页所有骰子威力+2";

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (behavior.card.speedDiceResultValue >= 6)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 2 });
            }
        }
    }
}
