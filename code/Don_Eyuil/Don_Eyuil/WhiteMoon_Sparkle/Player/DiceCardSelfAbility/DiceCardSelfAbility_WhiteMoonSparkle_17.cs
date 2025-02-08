using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;
using System;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_WhiteMoonSparkle_17 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[战斗开始]使所有我方角色本幕首次通过书页获得的正面状态层数+1 自身当前每应用1把副武器便使自身获得1层强壮(至多2层)";

        public override void OnStartBattle()
        {
            foreach (var item in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_temp>(item, 1);
            }

            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, Math.Min(BattleUnitBuf_Sparkle.Instance.SubWeapons.Count, 2), owner);
        }

        public class BattleUnitBuf_temp : BattleUnitBuf_Don_Eyuil
        {
            public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
            {
                if (cardBuf.positiveType == BufPositiveType.Positive && target.faction == _owner.faction)
                {
                    return 1;
                }

                return base.OnGiveKeywordBufByCard(cardBuf, stack, target);
            }

            public override void OnRoundEnd()
            {
                Destroy();
            }

            public BattleUnitBuf_temp(BattleUnitModel model) : base(model)
            {
            }
        }
    }
}
