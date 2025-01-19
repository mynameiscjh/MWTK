using System;
using System.Collections.Generic;

namespace Don_Eyuil.Buff
{
    public class BattleUnitBuf_Sickle : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "堂埃尤尔派硬血术3式-血镰\r\n目标每有2层流血便使自身斩击骰子造成的伤害增加10%(至多50%)\r\n所有我方角色每施加50层\"流血\"便使自身获得1层\"汹涌的血潮\"\r\n";
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = behavior.Detail == LOR_DiceSystem.BehaviourDetail.Slash ? Math.Min(50, behavior.card.owner.bufListDetail.HasBuf<BattleUnitBuf_bleeding>() ? behavior.card.owner.bufListDetail.GetFieldValue<List<BattleUnitBuf>>("_bufList").Find(x => x is BattleUnitBuf_bleeding).stack / 2 * 10 : 0) : 0 });
        }
        bool fl = false;

        public BattleUnitBuf_Sickle(BattleUnitModel model) : base(model)
        {
        }

        public override void OnRoundStartAfter()
        {
            if (fl)
            {
                return;
            }
            foreach (var item in BattleObjectManager.instance.GetAliveList(_owner.faction))
            {
                BattleUnitBuf_BleedCount.T39 = _owner;
                item.bufListDetail.AddBuf(new BattleUnitBuf_BleedCount() { stack = 0 });
            }
            fl = true;
        }

        public class BattleUnitBuf_BleedCount : BattleUnitBuf
        {
            public static BattleUnitModel T39 = null;
            public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
            {
                if (cardBuf.bufType == KeywordBuf.Bleeding)
                {
                    this.stack += stack;
                }

                if (stack >= 50 && T39 != null)
                {
                    BattleUnitBuf_BloodTide.GainBuf<BattleUnitBuf_BloodTide>(T39, 1);
                    this.stack -= 50;
                }

                return base.OnGiveKeywordBufByCard(cardBuf, stack, target);
            }
        }

    }
}
