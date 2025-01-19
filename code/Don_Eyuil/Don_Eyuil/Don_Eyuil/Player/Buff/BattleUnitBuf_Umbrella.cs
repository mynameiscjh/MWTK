using Don_Eyuil.Buff;
using System.Collections.Generic;
using DiceCardXmlInfo = LOR_DiceSystem.DiceCardXmlInfo;

namespace Don_Eyuil.Don_Eyuil.Buff
{
    public class BattleUnitBuf_Umbrella : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "自身命中带有流血的目标时造成的混乱伤害增加25%同时使自身获得1层\"结晶硬血\"\r\n下令战斗时若自身至少拥有6层\"结晶硬血\"则使自身获得一颗反击(突刺4-8)骰子\r\n";

        protected override string keywordId => "BattleUnitBuf_Umbrella";

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (!behavior.card.target.bufListDetail.HasBuf<BattleUnitBuf_bleeding>())
            {
                return;
            }
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { breakRate = 25 });
            BattleUnitBuf_HardBlood_Crystal.GainBuf<BattleUnitBuf_HardBlood_Crystal>(_owner, 1);
        }

        public override void OnStartBattle()
        {

            if (GetBufStack<BattleUnitBuf_HardBlood_Crystal>(_owner) < 6)
            {
                return;
            }
            base.OnStartBattle();
            DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(MyId.Card_经典反击书页, false);
            List<BattleDiceBehavior> list = new List<BattleDiceBehavior>();
            int num = 0;
            BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior();
            battleDiceBehavior.behaviourInCard = cardItem.DiceBehaviourList[1].Copy();
            battleDiceBehavior.SetIndex(num++);
            list.Add(battleDiceBehavior);
            this._owner.cardSlotDetail.keepCard.AddBehaviours(cardItem, list);
        }

        public BattleUnitBuf_Umbrella(BattleUnitModel model) : base(model)
        {
        }
    }
}
