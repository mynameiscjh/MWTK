using Don_Eyuil.Don_Eyuil.DiceCardSelfAbility;

namespace Don_Eyuil.Don_Eyuil.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_28 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]追加本书页击中目标次数×2的混乱伤害";
        public override void OnSucceedAttack()
        {
            var temp = card.cardAbility as DiceCardSelfAbility_DmgCount;
            if (temp != null)
            {
                card.target.TakeBreakDamage(temp.count * 2, attacker: owner);
            }
        }
    }
}
