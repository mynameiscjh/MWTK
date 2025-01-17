namespace Don_Eyuil.Don_Eyuil.Buff
{
    public class BattleUnitBuf_DoubleSwords : BattleUnitBuf
    {
        public static string Desc = "自身受到单方面攻击时将以一颗(闪避4-8[拼点胜利]对目标施加1层[流血])迎击\r\n自身承受\"流血\"伤害时每承受3点便使下一颗进攻型骰子伤害+1\r\n";
        public int count = 0;
        public override float DmgFactor(int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None)
        {
            if (keyword == KeywordBuf.Bleeding)
            {
                count += dmg;
            }
            return base.DmgFactor(dmg, type, keyword);
        }

        public override void OnStartTargetedOneSide(BattlePlayingCardDataInUnitModel attackerCard)
        {
            BattleDiceCardModel battleDiceCardModel = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_经典反击书页, false));
            if (battleDiceCardModel != null)
            {
                this._owner.cardSlotDetail.keepCard.AddBehaviourForOnlyDefense(battleDiceCardModel, battleDiceCardModel.CreateDiceCardBehaviorList()[0]);
            }
        }

    }
}
