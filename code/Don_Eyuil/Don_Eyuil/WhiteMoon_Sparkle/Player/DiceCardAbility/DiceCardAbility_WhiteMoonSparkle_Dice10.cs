using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardAbility
{
    public class DiceCardAbility_WhiteMoonSparkle_Dice10 : DiceCardAbilityBase
    {
        public static string Desc = "[拼点失败]使本书页下一颗骰子最大值+10 [投掷时]若本书页无下一颗骰子则使所有我方角色恢复5点混乱抗性";

        public override void OnLoseParrying()
        {
            var temp = card.cardBehaviorQueue.ToList();
            if (temp.Count >= temp.FindIndex(x => x == behavior))
            {
                card.ApplyDiceStatBonus(DiceMatch.NextDice, new DiceStatBonus() { max = 10 });
            }
            else
            {
                foreach (var item in BattleObjectManager.instance.GetAliveList(owner.faction))
                {
                    item.RecoverBreakLife(5);
                }
            }
        }
    }
}
