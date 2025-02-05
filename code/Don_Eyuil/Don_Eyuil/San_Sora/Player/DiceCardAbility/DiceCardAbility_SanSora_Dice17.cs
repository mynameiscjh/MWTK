namespace Don_Eyuil.San_Sora.Player.DiceCardAbility
{
    public class DiceCardAbility_SanSora_Dice17 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]若本骰子基础值不低于6则使本骰子重复投掷一次(至多2次)";
        int count = 0;
        public override void OnSucceedAttack()
        {
            if (count >= 2)
            {
                return;
            }

            if (behavior.DiceVanillaValue >= 6)
            {
                base.ActivateBonusAttackDice();
                count++;
            }
        }
    }
}
