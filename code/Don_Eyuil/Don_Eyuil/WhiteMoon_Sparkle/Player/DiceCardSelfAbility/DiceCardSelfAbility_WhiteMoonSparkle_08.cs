using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_WhiteMoonSparkle_08 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页根据自身当前主武器改变\r\n本书页将在使用后移出战斗 并在2幕后回到手中\r\n[使用时]本书页使用期间使目标受到的非命中伤害+50% \r\n";

        public override void OnAddToHand(BattleUnitModel owner)
        {
            owner.allyCardDetail.GetAllDeck().FindAll(x => x.GetID() == MyId.Card_所见之梦_千斤弓).ForEach(card =>
            {
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Year)))
                {
                    card = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所见之梦_泉之龙_秋之莲));
                }
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Sword)))
                {
                    card = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所见之梦_月之剑));
                }
            });
        }

        public override void OnApplyCard()
        {
            if (!BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Bow)))
            {
                var temp = new BattleDiceCardModel();
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Year)))
                {
                    temp = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所见之梦_泉之龙_秋之莲));
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
            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitCard_ReturnCard>(owner, 1).card = card.card;
            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitCard_非命中伤害加百分之50>(card.target, 1);
            owner.allyCardDetail.ExhaustACardAnywhere(card.card);
        }

        public class BattleUnitCard_非命中伤害加百分之50 : BattleUnitBuf_Don_Eyuil
        {
            public BattleUnitCard_非命中伤害加百分之50(BattleUnitModel model) : base(model)
            {
            }

            public override float DmgFactor(int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None)
            {
                if (type != DamageType.Attack)
                {
                    return 1.5f;
                }

                return base.DmgFactor(dmg, type, keyword);
            }

            public override void OnRoundEnd()
            {
                Destroy();
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
