using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_WhiteMoonSparkle_10 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页根据当前所应用的副武器获得额外效果\r\n本书页使用期间使双方“进攻型”骰子无论是否取得拼点胜利皆可对目标造成命中伤害 \r\n使目标造成的伤害与混乱伤害减少自身骰子点数3/2（向下取整）" +
            "额外效果:\r\n" +
            "泉之龙/秋之莲:[使用时]本书页造成的伤害与混乱伤害+40% \r\n" +
            "千斤弓:[使用时]本书页使用期间使目标受到的非命中伤害+50% \r\n" +
            "月之剑:[使用时]本书页骰子最大最小值提升25%\r\n" +
            "埃尤尔之血:[使用时]本书页首次命中目标时对目标造成一次目标当前流血层数150%的流血伤害\r\n";

        public override void OnAddToHand(BattleUnitModel owner)
        {
            if (!BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Year)))
            {
                owner.personalEgoDetail.RemoveCard(MyId.Card_传承之梦_泉之龙_秋之莲);
            }
            if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Bow)))
            {
                owner.personalEgoDetail.AddCard(MyId.Card_传承之梦_千斤弓);
            }
            if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Sword)))
            {
                owner.personalEgoDetail.AddCard(MyId.Card_传承之梦_月之剑);
            }
        }

        public override void OnUseCard()
        {
            if (BattleUnitBuf_Sparkle.Instance.SubWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Year)))
            {
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { dmgRate = 40, breakRate = 40 });
            }
            if (BattleUnitBuf_Sparkle.Instance.SubWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Bow)))
            {
                BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_受到的非命中伤害加百分之50>(card.target, 1);
            }
            if (BattleUnitBuf_Sparkle.Instance.SubWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Sword)))
            {
                card.ForeachQueue(DiceMatch.AllDice, x => x.ApplyDiceStatBonus(new DiceStatBonus() { max = x.GetDiceMax() / 4, min = x.GetDiceMin() / 4 }));
            }
        }
        bool fl = false;
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (!fl && BattleUnitBuf_Sparkle.Instance.SubWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Blood)))
            {
                fl = true;
                card.target.TakeDamage((int)((card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding)?.stack ?? 0) * 1.5f));
                BattleUnitBuf_Don_Eyuil.OnTakeBleedingDamagePatch.Trigger_BleedingDmg_After(card.target, (int)((card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding)?.stack ?? 0) * 1.5f), KeywordBuf.Bleeding);
            }
        }

        public class BattleUnitBuf_受到的非命中伤害加百分之50 : BattleUnitBuf_Don_Eyuil
        {
            public override float DmgFactor(int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None)
            {
                if (type != DamageType.Attack)
                {
                    return 1.5f;
                }
                return base.DmgFactor(dmg, type, keyword);
            }

            public override void OnEndParrying()
            {
                Destroy();
            }

            public BattleUnitBuf_受到的非命中伤害加百分之50(BattleUnitModel model) : base(model)
            {
            }
        }

        public override void OnLoseParrying()
        {
            card.currentBehavior.GiveDamage(card.target);
        }

        public override void OnStartParrying()
        {
            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_使造成的伤害与混乱伤害减少目标骰子点数2分之3>(card.target, 1);
        }

        public class BattleUnitBuf_使造成的伤害与混乱伤害减少目标骰子点数2分之3 : BattleUnitBuf_Don_Eyuil
        {
            public BattleUnitBuf_使造成的伤害与混乱伤害减少目标骰子点数2分之3(BattleUnitModel model) : base(model)
            {
            }

            public override void BeforeGiveDamage(BattleDiceBehavior behavior)
            {
                if (behavior.TargetDice == null)
                {
                    return;
                }
                if (behavior?.TargetDice?.card?.cardAbility?.GetType() == typeof(DiceCardSelfAbility_WhiteMoonSparkle_10))
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = -behavior.TargetDice.DiceResultValue / 2 * 3, breakDmg = -behavior.TargetDice.DiceResultValue / 2 * 3 });
                }
            }

            public override void OnEndParrying()
            {
                Destroy();
            }
        }
    }
}
