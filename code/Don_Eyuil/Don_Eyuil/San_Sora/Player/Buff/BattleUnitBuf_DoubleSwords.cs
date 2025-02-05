using LOR_BattleUnit_UI;
using LOR_DiceSystem;
using System.Collections.Generic;

namespace Don_Eyuil.San_Sora.Player.Buff
{
    public class BattleUnitBuf_DoubleSwords : BattleUnitBuf_SanHardBlood
    {
        public static string Desc = "本骰子中使用的书页恢复体力时将同时恢复混乱抗性\r\n战斗开始时自身拥有”结晶硬血”则在这一幕中获得一颗斩击反击骰子(4-8[命中时]施加1层[流血])\r\n";

        public BattleUnitBuf_DoubleSwords(SpeedDiceUI dice) : base(dice)
        {
            //dice.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = TKS_BloodFiend_Initializer.ArtWorks["双剑骰子"];
        }

        public override void OnSuccessAttack(BattleDiceBehavior behavior)
        {
            if (behavior.card == Card)
            {
                behavior.AddFlag((DiceFlag)1);
            }
        }

        public override void OnStartBattle()
        {
            if (GetBufStack<BattleUnitBuf_Crystal_HardBlood>(_owner) >= 6)
            {
                DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(MyId.Card_经典反击书页, false);
                List<BattleDiceBehavior> list = new List<BattleDiceBehavior>();
                BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior();
                battleDiceBehavior.behaviourInCard = cardItem.DiceBehaviourList[2].Copy();
                battleDiceBehavior.SetIndex(1);
                list.Add(battleDiceBehavior);
                this._owner.cardSlotDetail.keepCard.AddBehaviours(cardItem, list);
            }

        }

        public override void BeforeRecoverHp(int v)
        {
            if (_owner.currentDiceAction.currentBehavior.HasFlag((DiceFlag)1))
            {
                _owner.RecoverBreakLife(v);
            }
        }
    }
}
