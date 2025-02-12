using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;
using System;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_WhiteMoonSparkle_07 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页根据自身当前主武器改变\r\n本书页将在使用后移出战斗 并在2幕后回到手中\r\n[使用时]本书页造成的伤害与混乱伤害+40% 且本书页每命中3次敌人便在下一幕获得1层强壮与伤害强化(至多3层)";

        public override void OnAddToHand(BattleUnitModel owner)
        {
            owner.allyCardDetail.GetAllDeck().FindAll(x => x.GetID() == MyId.Card_所见之梦_泉之龙_秋之莲).ForEach(card =>
            {
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Bow)))
                {
                    card = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所见之梦_千斤弓));
                }
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Sword)))
                {
                    card = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所见之梦_月之剑));
                }
            });
        }

        public override void OnApplyCard()
        {
            if (!BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Year)))
            {
                var temp = new BattleDiceCardModel();
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Bow)))
                {
                    temp = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所见之梦_千斤弓));
                }
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Sword)))
                {
                    temp = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所见之梦_月之剑));
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
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { dmgRate = 40, breakRate = 40 });

            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitCard_ReturnCard>(owner, 1).card = card.card;
            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_命中3次敌人便在下一幕获得1层强壮与伤害强化_至多3层>(owner, 1).card = card;

            owner.allyCardDetail.ExhaustACardAnywhere(card.card);
        }

        public class BattleUnitBuf_命中3次敌人便在下一幕获得1层强壮与伤害强化_至多3层 : BattleUnitBuf_Don_Eyuil
        {
            public BattleUnitBuf_命中3次敌人便在下一幕获得1层强壮与伤害强化_至多3层(BattleUnitModel model) : base(model)
            {
            }

            int count = 0;
            public BattlePlayingCardDataInUnitModel card;
            public override void OnSuccessAttack(BattleDiceBehavior behavior)
            {
                if (behavior.card == card)
                {
                    count++;
                }
            }

            public override void OnRoundStart()
            {
                _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, Math.Min(count / 3, 3));
                _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.DmgUp, Math.Min(count / 3, 3));
            }
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
