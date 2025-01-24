namespace Don_Eyuil.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_56 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页仅限[硬血结晶]达到15层后使用\r\n[使用时]消耗所有[硬血结晶]每消耗1层便使本书页施加的[流血]层数+1\r\n";
        public override string[] Keywords => new string[] { "BattleUnitBuf_BleedCrystal", "Bleeding_Keyword", "DonEyuil" };
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return BattleUnitBuf_HardBlood_Crystal.GetBufStack<BattleUnitBuf_HardBlood_Crystal>(owner) >= 15;
        }
        public int useBuf = 0;
        public override void OnUseCard()
        {
            useBuf = 0;
            if (BattleUnitBuf_HardBlood_Crystal.GetBufStack<BattleUnitBuf_HardBlood_Crystal>(owner) > 0)
            {
                useBuf = BattleUnitBuf_HardBlood_Crystal.GetBufStack<BattleUnitBuf_HardBlood_Crystal>(owner);
                BattleUnitBuf_HardBlood_Crystal.RemoveBuf<BattleUnitBuf_HardBlood_Crystal>(owner);
                BattleDiceCardBuf_Temp buf = new BattleDiceCardBuf_Temp();
                buf.SetStack(useBuf);
                this.card.card.AddBuf(buf);
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
