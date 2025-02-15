using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;
using System.Collections.Generic;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_WhiteMoonSparkle_05 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页根据自身当前主武器改变\r\n[拼点开始]本书页所有骰子拼点失败时重复投掷1次(至多1次)但造成的伤害与混乱伤害-60%\r\n";

        public override void OnAddToHand(BattleUnitModel owner)
        {
            owner.allyCardDetail.GetAllDeck().FindAll(x => x.GetID() == MyId.Card_所护之物_千斤弓).ForEach(card =>
            {
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Year)))
                {
                    card = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所护之物_泉之龙_秋之莲));
                }
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Sword)))
                {
                    card = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所护之物_月之剑));
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
                    temp = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所护之物_泉之龙_秋之莲));
                }
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Sword)))
                {
                    temp = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所护之物_月之剑));
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

        public override void OnStartBattle()
        {
            list.Clear();
        }

        public List<BattleDiceBehavior> list = new List<BattleDiceBehavior>();

        public override void OnLoseParrying()
        {
            if (!list.Contains(card.currentBehavior))
            {
                card.currentBehavior.isBonusAttack = true;
                card.currentBehavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = -60 });
                list.Add(card.currentBehavior);
            }
        }
    }
}
