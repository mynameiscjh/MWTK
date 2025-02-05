using LOR_DiceSystem;
using UnityEngine;

namespace Don_Eyuil.Don_Eyuil.Player.Buff
{
    public class BattleUnitBuf_Lance : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "拼点时自身速度每高于目标2点便使自身所有骰子最大值+1(至多+4)\r\n自身以高于目标至少4点的速度击中目标时将对目标施加1层\"无法凝结的血\"(每幕至多触发3次)若击杀目标则在下一幕获得一层\"热血尖枪\"\r\n";

        protected override string keywordId => "BattleUnitBuf_Lance";

        public BattleUnitBuf_Lance(BattleUnitModel model) : base(model)
        {
        }

        public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
        {
            int speedDiceResultValue = card.speedDiceResultValue;
            BattleUnitModel target = card.target;
            int targetSlotOrder = card.targetSlotOrder;
            if (targetSlotOrder >= 0 && targetSlotOrder < target.speedDiceResult.Count)
            {
                SpeedDice speedDice = target.speedDiceResult[targetSlotOrder];
                if (speedDiceResultValue > speedDice.value)
                {
                    int num = speedDiceResultValue - speedDice.value;
                    int num2 = Mathf.Min(4, num / 2);
                    if (num2 > 0)
                    {
                        card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                        {
                            power = num2
                        });
                    }
                }
            }
        }

        public override void OnSuccessAttack(BattleDiceBehavior behavior)
        {
            int temp1 = behavior.card.speedDiceResultValue;
            int temp2 = behavior.card.target.speedDiceResult[behavior.card.targetSlotOrder].value;
            if (temp1 - temp2 >= 4)
            {
                //无法凝结的血 未实现
            }
        }

        public override void OnKill(BattleUnitModel target)
        {
            //热血尖枪 未实现
        }

    }
}
