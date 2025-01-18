using System.Collections.Generic;

namespace Don_Eyuil.Don_Eyuil.Buff
{
    public class BattleUnitBuf_Scourge : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "堂埃尤尔派硬血术8式-血鞭\r\n自身以打击骰子施加\"流血\"时将额外对一名敌方角色施加等量流血\r\n自身每幕最后一张书页施加\"流血\"时将额外对目标施加等量\"血晶荆棘\"\r\n";
        public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
        {
            if (cardBuf.bufType == KeywordBuf.Bleeding && _owner.currentDiceAction.currentBehavior.Detail == LOR_DiceSystem.BehaviourDetail.Hit)
            {
                RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList_opponent(_owner.faction)).bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, stack);
            }

            return base.OnGiveKeywordBufByCard(cardBuf, stack, target);
        }

        public BattleUnitBuf_Scourge(BattleUnitModel model) : base(model)
        {
        }

        public bool CheckTrigger(BattlePlayingCardDataInUnitModel card)
        {
            BattlePlayingCardDataInUnitModel[] array = Singleton<StageController>.Instance.GetAllCards().ToArray();
            foreach (BattlePlayingCardDataInUnitModel battlePlayingCardDataInUnitModel in array)
            {
                if (battlePlayingCardDataInUnitModel.owner == base._owner && battlePlayingCardDataInUnitModel != card)
                {
                    return false;
                }
            }
            if (card != null)
            {
                Queue<BattleDiceBehavior> cardBehaviorQueue = card.cardBehaviorQueue;
                if (cardBehaviorQueue != null && cardBehaviorQueue.Count <= 0)
                {
                    return true;
                }
            }
            return false;
        }

        public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
        {
            if (CheckTrigger(card))
            {
                card.card.AddBuf(new BattleDiceCardBuf_Thistles());
            }
        }

        public class BattleDiceCardBuf_Thistles : BattleDiceCardBuf
        {
            public override int OnAddKeywordBufByCard(BattleUnitBuf cardBuf, int stack)
            {
                if (cardBuf.bufType == KeywordBuf.Bleeding)
                {
                    BattleUnitBuf_Thistles.GainBuf(cardBuf.GetFieldValue<BattleUnitModel>("_owner"), stack);
                }

                return base.OnAddKeywordBufByCard(cardBuf, stack);
            }
        }
    }
}
