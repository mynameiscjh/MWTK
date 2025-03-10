﻿using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_WhiteMoonSparkle_09 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页根据自身当前主武器改变\r\n本书页将在使用后移出战斗 并在2幕后回到手中\r\n[使用时]本书页骰子最大最小值提升25% 且拼点胜利时造成的伤害与混乱伤害-90%但拼点胜利时使骰子重复投掷1次\r\n";

        public override void OnAddToHand(BattleUnitModel owner)
        {
            owner.allyCardDetail.GetAllDeck().FindAll(x => x.GetID() == MyId.Card_所见之梦_月之剑).ForEach(card =>
            {
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Bow)))
                {
                    card = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所见之梦_千斤弓));
                }
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Year)))
                {
                    card = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所见之梦_泉之龙_秋之莲));
                }
            });
        }

        public override void OnApplyCard()
        {
            if (!BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Sword)))
            {
                var temp = new BattleDiceCardModel();
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Year)))
                {
                    temp = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所见之梦_泉之龙_秋之莲));
                }
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Bow)))
                {
                    temp = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所见之梦_千斤弓));
                }
                card.card.GetBufList().ForEach(x => temp.AddBuf(x));
                temp.SetCurrentCost(card.card.GetCost());
                card.card = temp;
                card.cardAbility = temp.CreateDiceCardSelfAbilityScript();
                card.cardAbility.card = card;
                card.cardAbility.OnApplyCard();
                card.ResetCardQueue();
            }
            else
            {
                card.card.SetCurrentCost(card.card.XmlData.Spec.Cost);
            }
        }

        public override void OnUseCard()
        {
            card.ForeachQueue(DiceMatch.AllDice, x => x.ApplyDiceStatBonus(new DiceStatBonus() { max = x.GetDiceMax() / 4, min = x.GetDiceMin() / 4 }));

            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitCard_ReturnCard>(owner, 1).card = card.card;

            owner.allyCardDetail.ExhaustACardAnywhere(card.card);
        }

        public override void OnWinParryingAtk()
        {
            card.currentBehavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = -90, breakRate = -90 });
            card.currentBehavior.isBonusAttack = true;
        }

        public override void OnWinParryingDef()
        {
            card.currentBehavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = -90, breakRate = -90 });
            card.currentBehavior.isBonusAttack = true;
        }

        public class BattleUnitCard_ReturnCard : BattleUnitBuf_Don_Eyuil
        {
            int count = 0;
            public BattleDiceCardModel card;

            public BattleUnitCard_ReturnCard(BattleUnitModel model) : base(model)
            {
            }

            public override void OnRoundStart()
            {
                count++;
                if (count == 2)
                {
                    _owner.allyCardDetail.AddCardToHand(card);
                    Destroy();
                }
            }
        }
    }
}
