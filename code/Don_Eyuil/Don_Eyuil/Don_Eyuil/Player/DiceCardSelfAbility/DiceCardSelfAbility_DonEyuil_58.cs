using LOR_DiceSystem;
using UnityEngine;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_58 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]自身速度每高于目标2点便时本书页施加的流血层数+1(至多+2)\r\n若以本书页击杀目标则对所有敌方角色施加9层[流血]\r\n";
        public override string[] Keywords => new string[] { "Bleeding_Keyword", "DonEyuil" };
        public override void OnUseCard()
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
                    int num2 = Mathf.Min(2, num / 2);
                    if (num2 > 0)
                    {
                        var buf = new BattleDiceCardBuf_Temp();
                        buf.SetStack(num2);
                        card.card.AddBuf(buf);
                    }
                }
            }
        }

        public override void OnEndBattle()
        {
            if (this.card.target != null && this.card.target.IsDead())
            {
                foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_opponent(base.owner.faction))
                {
                    battleUnitModel.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 9, base.owner);
                }
            }
        }

        public class BattleDiceCardBuf_Temp : BattleDiceCardBuf
        {
            public void SetStack(int v)
            {
                this._stack = v;
            }
            public override int OnAddKeywordBufByCard(BattleUnitBuf cardBuf, int stack)
            {
                return this._stack;
            }
            public override void OnRoundEnd()
            {
                this.Destroy();
            }
        }

    }
}
