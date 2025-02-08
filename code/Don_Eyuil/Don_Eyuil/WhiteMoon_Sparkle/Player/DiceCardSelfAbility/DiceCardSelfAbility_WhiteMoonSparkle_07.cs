using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_WhiteMoonSparkle_07 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页根据自身当前主武器改变\r\n本书页将在使用后移出战斗 并在2幕后回到手中\r\n[使用时]本书页使用期间使目标受到的非命中伤害+50% \r\n";

        public override void OnAddToHand(BattleUnitModel owner)
        {
            if (!BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Year)))
            {
                owner.allyCardDetail.AddCardToHand(BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.未实现id)));
                owner.allyCardDetail.ExhaustCard(MyId.未实现id);
            }
        }

        public override void OnUseCard()
        {
            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitCard_ReturnCard>(owner, 1).card = card.card;
            owner.allyCardDetail.ExhaustACardAnywhere(card.card);
        }

        public class BattleUnitCard_非命中伤害加百分之50 : BattleUnitBuf_Don_Eyuil
        {

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
