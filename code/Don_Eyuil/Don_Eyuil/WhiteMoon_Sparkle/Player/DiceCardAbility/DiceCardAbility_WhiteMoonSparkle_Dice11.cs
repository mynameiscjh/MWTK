using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardAbility
{
    public class DiceCardAbility_WhiteMoonSparkle_Dice11 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]使本书页剩余骰子额外命中(自身当前应用副武器数量)个目标";

        public override void OnSucceedAttack()
        {
            behavior.GiveDamage_SubTarget(card.target, BattleUnitBuf_Sparkle.Instance.SubWeapons.Count);
        }
    }
}
