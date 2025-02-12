using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;
using System.Collections.Generic;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_WhiteMoonSparkle_05 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页根据自身当前主武器改变\r\n[拼点开始]本书页所有骰子拼点失败时重复投掷1次（至多1次）但造成的伤害与混乱伤害-60%\r\n";

        public override void OnAddToHand(BattleUnitModel owner)
        {
            if (!BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Bow)))
            {
                owner.allyCardDetail.ExhaustCard(MyId.Card_所护之物_千斤弓);
            }
            if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Sword)))
            {
                owner.allyCardDetail.AddCardToHand(BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所护之物_月之剑)));
            }
            if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Year)))
            {
                owner.allyCardDetail.AddCardToHand(BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所护之物_泉之龙_秋之莲)));
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
