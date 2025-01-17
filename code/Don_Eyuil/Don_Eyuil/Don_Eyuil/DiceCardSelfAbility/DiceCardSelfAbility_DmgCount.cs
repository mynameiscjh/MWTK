namespace Don_Eyuil.Don_Eyuil.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DmgCount : DiceCardSelfAbilityBase
    {
        public static string Desc = "这是用来给第三骰记录击中次数的书页效果";
        public int count = 0;
        public override void OnSucceedAttack()
        {
            count++;
        }
    }
}
