using LOR_BattleUnit_UI;
using System;
using System.Collections.Generic;

namespace Don_Eyuil.San_Sora.Player.Buff
{
    public class BattleUnitBuf_SanHardBlood : BattleUnitBuf_Don_Eyuil
    {
        public SpeedDiceUI dice;

        public BattlePlayingCardDataInUnitModel Card => _owner.cardSlotDetail.cardAry[dice.view.speedDiceSetterUI.GetFieldValue<List<SpeedDiceUI>>("_speedDices").FindIndex(x => x == dice)];
        public int Index => dice.view.speedDiceSetterUI.GetFieldValue<List<SpeedDiceUI>>("_speedDices").FindIndex(x => x == dice);
        public override int paramInBufDesc => dice.view.speedDiceSetterUI.GetFieldValue<List<SpeedDiceUI>>("_speedDices").FindIndex(x => x == dice);

        public BattleUnitBuf_SanHardBlood(SpeedDiceUI dice) : base(dice.view.model)
        {
            this.dice = dice;
        }

        public static T GainBuf<T>(SpeedDiceUI dice) where T : BattleUnitBuf_SanHardBlood
        {
            T BuffInstance = Activator.CreateInstance(typeof(T), new object[] { dice }) as T;
            dice.view.model.bufListDetail.AddBuf(BuffInstance);
            return BuffInstance;
        }

        public static SpeedDiceUI GetBufDice<T>(BattleUnitModel model, BufReadyType ReadyType = BufReadyType.ThisRound) where T : BattleUnitBuf_SanHardBlood
        {
            return BattleUnitBuf_Don_Eyuil.GetBuf<T>(model, ReadyType).dice;
        }
    }

}
