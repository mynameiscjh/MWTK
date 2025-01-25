namespace Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DmgCount : DiceCardSelfAbilityBase
    {
        public static string Desc = "";
        public int count = 0;
        public override void OnSucceedAttack()
        {
            count++;
        }
    }
}
