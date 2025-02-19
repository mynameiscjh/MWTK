namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_Dice_30 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]使手中一张费用不为0的书页费用-1";

        public override void OnSucceedAttack()
        {
            foreach (var item in owner.allyCardDetail.GetHand())
            {
                if (item.GetCost() > 0 && !item.GetFieldValue<DiceCardSelfAbilityBase>("_script").IsFixedCost())
                {
                    item.AddCost(-1);
                    return;
                }
            }
        }
    }
}
