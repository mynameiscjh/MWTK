namespace Don_Eyuil.Don_Eyuil.Buff
{
    public class BattleUnitBuf_Scourge : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "堂埃尤尔派硬血术8式-血鞭\r\n自身以打击骰子施加\"流血\"时将额外对一名敌方角色施加等量流血\r\n自身每幕最后一张书页施加\"流血\"时将额外对目标施加等量\"血晶荆棘\"\r\n";
        public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
        {
            if (cardBuf.bufType == KeywordBuf.Bleeding && _owner.currentDiceAction.currentBehavior.Detail == LOR_DiceSystem.BehaviourDetail.Hit)
            {
                target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, stack);
            }

            return base.OnGiveKeywordBufByCard(cardBuf, stack, target);
        }
        public int cardCount = 0;

        public BattleUnitBuf_Scourge(BattleUnitModel model) : base(model)
        {
        }

        public override void OnStartBattle()
        {
            cardCount = _owner.cardSlotDetail.cardAry.Count;
        }

        public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
        {
            if (cardCount == 1)
            {
                card.card.AddBuf(new BattleDiceCardBuf_Thistles());
            }
            cardCount--;
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
