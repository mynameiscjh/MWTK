using System.Collections.Generic;
using static Don_Eyuil.WhiteMoon_Sparkle.Player.Buff.BattleUnitBuf_Inherit;

namespace Don_Eyuil.Don_Eyuil.Player.Buff
{
    public class BattleUnitBuf_Transmit : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "传递之物[中立buff]（友方为白月骑士时堂埃尤尔获得）:\r\n自身获得的正面情感+1对指向耀的敌方角色施加的\"流血\"层数+1\r\n若耀同时被3张书页选中则这一幕对所有指向耀的敌方角色使用1张[一如既往]\r\n自身情感等级达到4时使\"耀\"获得\"埃尤尔之血\"\r\n";

        public override void BeforeAddEmotionCoin(EmotionCoinType CoinType, ref int Count)
        {
            if (CoinType == EmotionCoinType.Positive)
            {
                Count++;
            }
            MyTools.未实现提醒();
            var temp = BattleObjectManager.instance.GetAliveList().Find(x => x.Book.BookId == MyId.Book_小耀之页);
            if (_owner.emotionDetail.EmotionLevel == 4 && temp.personalEgoDetail.GetCardAll().Find(x => x.GetID() == MyId.未实现id) == null)
            {
                temp.personalEgoDetail.AddCard(MyId.未实现id);
            }
        }

        List<BattleUnitModel> attackSparkleList = new List<BattleUnitModel>();
        List<BattleUnitModel> sparkleAttackList = new List<BattleUnitModel>();
        public override void OnStartBattle()
        {
            foreach (BattleUnitModel item in BattleObjectManager.instance.GetAliveList())
            {
                foreach (var card in item.cardSlotDetail.cardAry)
                {
                    if (card.target.Book.BookId == MyId.Book_小耀之页)
                    {
                        attackSparkleList.Add(item);
                    }
                }
            }

            foreach (var item in BattleObjectManager.instance.GetAliveList().Find(x => x.Book.BookId == MyId.Book_小耀之页).cardSlotDetail.cardAry)
            {
                sparkleAttackList.Add(item.target);
            }

            if (attackSparkleList.Count >= 3)
            {
                MyTools.未实现提醒();
                var temp = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.未实现id));
                foreach (var item in attackSparkleList)
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
        }

        public override void BeforeOtherUnitAddKeywordBuf(KeywordBuf BufType, BattleUnitModel Target, ref int Stack)
        {
            if (BufType == KeywordBuf.Bleeding && attackSparkleList.Contains(Target))
            {
                Stack++;
            }
        }

        public BattleUnitBuf_Transmit(BattleUnitModel model) : base(model)
        {
        }
    }
}
