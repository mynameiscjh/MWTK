using System.Collections.Generic;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.Buff
{
    public class BattleUnitBuf_Inherit : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "传承之物[中立buff](友方为堂埃尤尔时获得):\r\n自身速度+1 对指向堂埃尤尔的敌方角色造成的伤害与混乱伤害+1 若自身与堂埃尤尔幕指向了同一目标则使该目标本幕对堂埃尤尔造成的伤害与混乱伤害-1\r\n若堂埃尤尔同时被3张书页选中则这一幕对所有指向堂埃尤尔的敌方角色使用1张[一如既往]\r\n";

        public override int GetSpeedDiceAdder(int speedDiceResult)
        {
            return speedDiceResult + 1;
        }
        List<BattleUnitModel> attackDonEyuilList = new List<BattleUnitModel>();
        List<BattleUnitModel> donEyuilAttackList = new List<BattleUnitModel>();
        public override void OnStartBattle()
        {
            foreach (BattleUnitModel item in BattleObjectManager.instance.GetAliveList())
            {
                foreach (var card in item.cardSlotDetail.cardAry)
                {
                    if (card.target.Book.BookId == MyId.Book_堂_埃尤尔之页)
                    {
                        attackDonEyuilList.Add(item);
                    }
                }
            }

            foreach (var item in BattleObjectManager.instance.GetAliveList().Find(x => x.Book.BookId == MyId.Book_堂_埃尤尔之页).cardSlotDetail.cardAry)
            {
                donEyuilAttackList.Add(item.target);
            }

            if (attackDonEyuilList.Count >= 3)
            {
                var temp = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_一如既往_小耀));
                foreach (var item in attackDonEyuilList)
                {
                    var card = new BattlePlayingCardDataInUnitModel
                    {
                        owner = _owner,
                        card = temp,
                        target = item,
                        earlyTarget = item,
                        earlyTargetOrder = 0,
                        speedDiceResultValue = 0,
                        slotOrder = 0,
                        targetSlotOrder = 0,
                        cardAbility = temp.CreateDiceCardSelfAbilityScript()
                    };
                    card.cardAbility.card = card;
                    card.cardAbility.OnApplyCard();
                    card.ResetCardQueue();
                    BattleOneSidePlayManager.Instance.StartOneSidePlay(card);
                }
            }

            foreach (var card in _owner.cardSlotDetail.cardAry)
            {
                if (donEyuilAttackList.Contains(card.target))
                {
                    card.target.bufListDetail.AddBuf(new BattleUnitBuf_DmgDown());
                }
            }
        }

        public class BattleUnitBuf_DmgDown : BattleUnitBuf
        {
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                if (behavior.card.target.Book.BookId == MyId.Book_堂_埃尤尔之页)
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = -1, breakDmg = -1 });
                }
            }

            public override void OnRoundEnd()
            {
                Destroy();
            }
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (attackDonEyuilList.Contains(behavior.card.target))
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = 1, breakDmg = 1 });
            }
        }

        public override void OnRoundEnd()
        {
            attackDonEyuilList.Clear();
            donEyuilAttackList.Clear();
        }

        public BattleUnitBuf_Inherit(BattleUnitModel model) : base(model)
        {
            this.SetFieldValue("_bufIcon", TKS_BloodFiend_Initializer.ArtWorks["传承之物和传递之物"]);
            this.SetFieldValue("_iconInit", true);
        }
    }
}
