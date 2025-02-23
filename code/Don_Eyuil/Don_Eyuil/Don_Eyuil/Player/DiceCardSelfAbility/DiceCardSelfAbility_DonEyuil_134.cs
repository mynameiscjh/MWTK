using Don_Eyuil.Don_Eyuil.Player.Buff;
using Don_Eyuil.San_Sora.Player.Buff;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_134 : DiceCardSelfAbilityBase
    {
        public static string Desc = "仅限装备了血剑时装备\r\n每一幕开始时若[硬血结晶]不低于15则自动抽取本书页\r\n[战斗开始]若[硬血结晶]不低于25层则消耗10层时自己获得[摇曳]";

        public override bool OnChooseCard(BattleUnitModel owner)
        {
            var buf = BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(owner);

            if (buf == null || buf.Sword == null)
            {
                return false;
            }

            return base.OnChooseCard(owner);
        }

        public override void OnRoundEnd(BattleUnitModel unit, BattleDiceCardModel self)
        {
            if (BattleUnitBuf_Don_Eyuil.GetBufStack<BattleUnitBuf_HardBlood_Crystal>(unit) >= 15)
            {
                unit.allyCardDetail.DrawCardsAllSpecific(self.GetID());
            }
        }

        public override void OnStartBattle()
        {
            if (BattleUnitBuf_Don_Eyuil.GetBufStack<BattleUnitBuf_HardBlood_Crystal>(owner) >= 25)
            {
                if (BattleUnitBuf_Don_Eyuil.UseBuf<BattleUnitBuf_HardBlood_Crystal>(owner, 10))
                {
                    BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Sway>(owner, 1);
                }
            }
        }

    }
}
