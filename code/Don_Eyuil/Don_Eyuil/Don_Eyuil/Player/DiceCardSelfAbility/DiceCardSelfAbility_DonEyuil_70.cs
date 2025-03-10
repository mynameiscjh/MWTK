﻿using System;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_70 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[持有时]自身每承受5点[流血]伤害便时本书页所有进攻型骰子威力+1(至多+3)";
        public override string[] Keywords => new string[] { "Bleeding_Keyword", "DonEyuil", "DonEyuil_1_5" };
        public override void OnAddToHand(BattleUnitModel owner)
        {
            if (BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_BleedCOunt>(owner) != null)
            {
                return;
            }
            owner.bufListDetail.AddBuf(new BattleUnitBuf_BleedCOunt(owner));
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_BleedCOunt>(owner) == null)
            {
                return;
            }
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = Math.Min(3, BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_BleedCOunt>(owner).count / 5) });
        }
        public class BattleUnitBuf_BleedCOunt : BattleUnitBuf_Don_Eyuil
        {
            public BattleUnitBuf_BleedCOunt(BattleUnitModel model) : base(model)
            {
            }
            public int count = 0;
            public override void AfterTakeBleedingDamage(int Dmg)
            {
                count += Dmg;
            }
            public override void OnRoundEnd()
            {
                count = 0;
            }
        }
    }
}
