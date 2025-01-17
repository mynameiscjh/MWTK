using Don_Eyuil.Don_Eyuil.DiceCardSelfAbility;
using System.Collections.Generic;

namespace Don_Eyuil.Don_Eyuil.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_19 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]重复触发本书页拼点胜利次自身[流血]";

        public override void OnSucceedAttack()
        {
            if (this.card.cardAbility is DiceCardSelfAbility_DonEyuil_77)
            {
                var ability = this.card.cardAbility as DiceCardSelfAbility_DonEyuil_77;
                var buf = owner.bufListDetail.GetFieldValue<List<BattleUnitBuf>>("_bufList").Find(x => x is BattleUnitBuf_bleeding) as BattleUnitBuf_bleeding;
                if (buf == null)
                {
                    return;
                }
                for (int i = 0; i < ability.winCount; i++)
                {
                    buf.AfterDiceAction(behavior);
                }
            }
        }
    }
}
