using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;
using LOR_DiceSystem;
using System.Collections.Generic;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_WhiteMoonSparkle_04 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页根据自身当前主武器改变（泉之龙/秋之莲）\r\n[拼点开始]摧毁本书页所有骰子 并置入与目标书页骰子数等量的点数为被摧毁骰子最大最小值平均值的进攻型骰子 骰子类型为目标弱点抗性（且首颗骰子拥有所有被摧毁骰子的骰子效果）";

        public override void OnAddToHand(BattleUnitModel owner)
        {
            if (!BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Year)))
            {
                owner.allyCardDetail.ExhaustCard(MyId.Card_所护之物_泉之龙_秋之莲);
            }
            if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Sword)))
            {
                owner.allyCardDetail.AddCardToHand(BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所护之物_月之剑)));
            }
            if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Bow)))
            {
                owner.allyCardDetail.AddCardToHand(BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所护之物_千斤弓)));
            }
        }

        public override void OnStartParrying()
        {
            int dice = 0;
            List<DiceCardAbilityBase> abilities = new List<DiceCardAbilityBase>();

            foreach (var item in card.GetDiceBehaviorList())
            {
                dice += (item.GetDiceMax() + item.GetDiceMin()) / 2;
                abilities.AddRange(item.abilityList);
            }

            dice /= card.GetDiceBehaviorList().Count;

            card.DestroyDice(DiceMatch.AllDice);

            for (int i = 0; i < card.target.cardSlotDetail.cardAry[card.targetSlotOrder].cardBehaviorQueue.Count; i++)
            {
                var temp = new BattleDiceBehavior()
                {
                    behaviourInCard = new DiceBehaviour
                    {
                        Min = dice,
                        Dice = dice,
                        Type = BehaviourType.Atk,
                        Detail = GetDetail(card.target),
                        MotionDetail = MotionDetail.J,
                        MotionDetailDefault = MotionDetail.N,
                        EffectRes = "",
                        Script = "",
                        Desc = "",
                        ActionScript = "",
                    }
                };

                if (i == 0)
                {
                    temp.abilityList = abilities;
                }

                temp.card = card;
                temp.SetIndex(i);
                card.AddDice(temp);
            }
        }

        public static BehaviourDetail GetDetail(BattleUnitModel model)
        {
            var s = model.GetResistHP(BehaviourDetail.Slash);
            var p = model.GetResistHP(BehaviourDetail.Penetrate);
            var h = model.GetResistHP(BehaviourDetail.Hit);
            if (s <= p && s <= h)
            {
                return BehaviourDetail.Slash;
            }
            if (p <= s && p <= h)
            {
                return BehaviourDetail.Penetrate;
            }
            if (h <= s && h <= p)
            {
                return BehaviourDetail.Hit;
            }
            return BehaviourDetail.Slash;
        }

    }
}
