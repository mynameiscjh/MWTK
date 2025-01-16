using Don_Eyuil.Buff;

namespace Don_Eyuil.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_56 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页仅限[硬血结晶]达到15层后使用\r\n[使用时]消耗所有[硬血结晶]每消耗1层便使本书页施加的[流血]层数+1\r\n";

        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return BattleUnitBuf_BleedCrystal.GetBuf(owner) != null && BattleUnitBuf_BleedCrystal.GetBuf(owner).stack >= 15;
        }
        public int useBuf = 0;
        public override void OnUseCard()
        {
            useBuf = 0;
            if (BattleUnitBuf_BleedCrystal.GetBuf(owner) != null)
            {
                useBuf = BattleUnitBuf_BleedCrystal.GetBuf(owner).stack;
                BattleUnitBuf_BleedCrystal.UseBuf(owner, useBuf);
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
