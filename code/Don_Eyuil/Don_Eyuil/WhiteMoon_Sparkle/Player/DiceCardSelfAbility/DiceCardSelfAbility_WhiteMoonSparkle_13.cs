using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;
using System.Collections.Generic;
using System.Linq;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_WhiteMoonSparkle_13 : DiceCardSelfAbilityBase
    {
        public static string Desc = "仅限自身应用至少2种副武器时使用\r\n[使用时]自身当前每应用1种副武器便使本书页所有骰子最大最小值+1且造成的伤害+20%\r\n根据当前所应用的副武器获得额外效果\r\n" +
            "额外效果:\r\n" +
            "泉之龙/秋之莲:首颗骰子命中效果改为[命中时]使所有我方角色获得1点正面情感\r\n" +
            "千斤弓:首颗骰子命中效果改为[命中时]这一幕与下一幕对目标施加1层[收尾标记]\r\n" +
            "月之剑:首颗骰子命中效果改为[命中时]这一幕与下一幕对目标施加[全面洞悉]\r\n" +
            "埃尤尔之血:本书页所有骰子若都至少命中1名目标则使自身获得1层[汹涌的血潮]\r\n";

        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return BattleUnitBuf_Sparkle.Instance.SubWeapons.Count >= 2;
        }

        public override void OnUseCard()
        {
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { max = BattleUnitBuf_Sparkle.Instance.SubWeapons.Count, min = BattleUnitBuf_Sparkle.Instance.SubWeapons.Count, dmgRate = BattleUnitBuf_Sparkle.Instance.SubWeapons.Count * 20 });

            card.cardBehaviorQueue.First().abilityList.Clear();
            if (BattleUnitBuf_Sparkle.Instance.SubWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Year)))
            {
                card.cardBehaviorQueue.First().AddAbility(new DiceCardAbility_使所有我方角色获得1点正面情感());
            }
            if (BattleUnitBuf_Sparkle.Instance.SubWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Bow)))
            {
                card.cardBehaviorQueue.First().AddAbility(new DiceCardAbility_这一幕与下一幕对目标施加1层收尾标记());
            }
            if (BattleUnitBuf_Sparkle.Instance.SubWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Sword)))
            {
                card.cardBehaviorQueue.First().AddAbility(new DiceCardAbility_这一幕与下一幕对目标施加1层全面洞悉());
            }

            count = card.cardBehaviorQueue.Count();
        }
        int count = 0;
        List<BattleDiceBehavior> list = new List<BattleDiceBehavior>();
        public override void OnSucceedAttack()
        {
            list.Add(card.currentBehavior);
            if (BattleUnitBuf_Sparkle.Instance.SubWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Blood)))
            {
                if (list.Count == count)
                {
                    BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_BloodTide>(owner, 1);
                }
            }
        }

        public class DiceCardAbility_使所有我方角色获得1点正面情感 : DiceCardAbilityBase
        {
            public override void OnSucceedAttack()
            {
                foreach (var item in BattleObjectManager.instance.GetAliveList(owner.faction))
                {
                    item.emotionDetail.CreateEmotionCoin(EmotionCoinType.Positive);
                }
            }
        }
        public class DiceCardAbility_这一幕与下一幕对目标施加1层收尾标记 : DiceCardAbilityBase
        {
            public override void OnSucceedAttack(BattleUnitModel target)
            {
                BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Flag>(target, 1);
                BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Flag>(target, 1, BufReadyType.NextRound);
            }
        }
        public class DiceCardAbility_这一幕与下一幕对目标施加1层全面洞悉 : DiceCardAbilityBase
        {
            public override void OnSucceedAttack(BattleUnitModel target)
            {
                BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Know>(target, 1);
                BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Know>(target, 1, BufReadyType.NextRound);
            }
        }
    }
}
