using Don_Eyuil.San_Sora.Player.Buff;

namespace Don_Eyuil.San_Sora.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_SanSora_10 : DiceCardSelfAbilityBase
    {
        public static string Desc = "若自身[血羽]不低于15层则自动抽取本书页 [战斗开始]消耗10层[血羽]使我方所有[拉曼查乐园的血魔]获得[摇曳] 若本书页应用在[血甲]骰子上则本效果将继承至下一幕";

        public override void OnRoundEnd(BattleUnitModel unit, BattleDiceCardModel self)
        {
            if (BattleUnitBuf_Feather.GetBufStack<BattleUnitBuf_Feather>(unit) >= 15)
            {
                owner.allyCardDetail.DrawCardsAllSpecific(self.GetID());
            }
        }

        public override void OnStartBattle()
        {
            if (BattleUnitBuf_Don_Eyuil.UseBuf<BattleUnitBuf_Feather>(owner, 10))
            {
                foreach (var item in BattleObjectManager.instance.GetAliveList(owner.faction))
                {
                    if (MyId.Books_拉曼查乐园的血魔.Contains(item.Book.BookId))
                    {
                        BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Sway>(item, 1);
                    }
                }
                var buf = BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_SanSora>(owner);
                if (buf != null && buf.Armour != null && buf.Armour.Card != null && buf.Armour.Card == this.card)
                {
                    owner.bufListDetail.AddBuf(new BattleUnitBuf_Temp(owner));
                }
            }
        }

        public class BattleUnitBuf_Temp : BattleUnitBuf_Don_Eyuil
        {
            public BattleUnitBuf_Temp(BattleUnitModel model) : base(model)
            {
            }

            public override void OnRoundStart()
            {
                if (BattleUnitBuf_Don_Eyuil.UseBuf<BattleUnitBuf_Feather>(owner, 10))
                {
                    foreach (var item in BattleObjectManager.instance.GetAliveList(owner.faction))
                    {
                        if (MyId.Books_拉曼查乐园的血魔.Contains(item.Book.BookId))
                        {
                            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Sway>(item, 1);
                        }
                    }
                }
                this.Destroy();
            }
        }

    }
}
