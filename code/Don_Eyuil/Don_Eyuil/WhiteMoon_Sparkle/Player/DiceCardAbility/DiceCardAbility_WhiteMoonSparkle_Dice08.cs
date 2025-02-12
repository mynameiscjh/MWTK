using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardAbility
{
    public class DiceCardAbility_WhiteMoonSparkle_Dice08 : DiceCardAbilityBase
    {
        public static string Desc = "[拼点胜利]本骰子造成的伤害与混乱伤害+2";

        public override void OnWinParrying()
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = 2, breakDmg = 2 });
        }
    }
}
