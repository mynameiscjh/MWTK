using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardAbility
{
    public class DiceCardAbility_WhiteMoonSparkle_Dice09 : DiceCardAbilityBase
    {
        public static string Desc = "[拼点失败]使本书页最后1颗进攻型骰子威力+1";

        public override void OnLoseParrying()
        {
            var temp = new List<BattleDiceBehavior>(card.cardBehaviorQueue);
            temp.RemoveAll(x => x.Type != LOR_DiceSystem.BehaviourType.Atk);
            temp.Last().ApplyDiceStatBonus(new DiceStatBonus() { power = 1 });
        }
    }
}
