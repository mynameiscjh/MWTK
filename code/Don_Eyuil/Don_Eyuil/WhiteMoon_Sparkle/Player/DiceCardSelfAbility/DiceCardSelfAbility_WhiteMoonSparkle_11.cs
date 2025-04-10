﻿using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_WhiteMoonSparkle_11 : DiceCardSelfAbilityBase
    {

        public static string Desc = "本书页根据当前所应用的副武器获得额外效果\r\n本书页第一颗骰子[命中时]追加等同于基础值次数的2点伤害";



        public override void OnApplyCard()
        {
            if (!BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Bow)))
            {
                var temp = new BattleDiceCardModel();
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Year)))
                {
                    temp = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_传承之梦_泉之龙_秋之莲));
                }
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Sword)))
                {
                    temp = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_传承之梦_月之剑));
                }
                card.card.GetBufList().ForEach(x => temp.AddBuf(x));
                temp.SetCurrentCostMax();
                temp.SetCurrentCost(card.card.GetCost());
                card.card = temp;
                card.cardAbility = temp.CreateDiceCardSelfAbilityScript();
                card.cardAbility.card = card;
                card.cardAbility.OnApplyCard();
                card.ResetCardQueue();
                card.card.AddCoolTime(card.card.MaxCooltimeValue);
            }
            else
            {
                card.card.SetCurrentCost(card.card.XmlData.Spec.Cost);
                card.card.SetCurrentCostMax();
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
                typeof(BattleUnitBuf_Don_Eyuil.OnTakeBleedingDamagePatch).GetInternalDelegate().DynamicInvoke(card.target, (int)((card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding)?.stack ?? 0) * 1.5f), KeywordBuf.Bleeding);
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

        public override void OnSucceedAttack()
        {
            if (card.currentBehavior.Index == 0)
            {
                for (int i = 0; i < card.currentBehavior.DiceVanillaValue; i++)
                {
                    card.target.TakeDamage(2);
                }
            }
        }
    }
}
