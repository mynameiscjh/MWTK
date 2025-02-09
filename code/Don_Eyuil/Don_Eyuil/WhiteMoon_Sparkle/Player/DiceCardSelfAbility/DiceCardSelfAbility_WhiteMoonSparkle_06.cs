using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;
using LOR_DiceSystem;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_WhiteMoonSparkle_06 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页根据自身当前主武器改变\r\n本书页将视为拥有所有月相标记\r\n[拼点开始]若目标首颗骰子平均值大于15则在书页末尾置入一颗打击骰子（13～17若骰子基础值不低于20则造成伤害+50%）反之则置入书页开头\r\n";

        public override void OnAddToHand(BattleUnitModel owner)
        {
            if (!BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Sword)))
            {
                owner.allyCardDetail.ExhaustCard(MyId.Card_所护之物_月之剑);
            }
            if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Year)))
            {
                owner.allyCardDetail.AddCardToHand(BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所护之物_泉之龙_秋之莲)));
            }
            if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Bow)))
            {
                owner.allyCardDetail.AddCardToHand(BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_所护之物_千斤弓)));
            }
        }

        public override void OnStartParrying()
        {
            var temp = card.target.currentDiceAction.cardBehaviorQueue.Peek();
            var dice = new BattleDiceBehavior
            {
                behaviourInCard = new DiceBehaviour
                {
                    Min = 13,
                    Dice = 17,
                    Type = BehaviourType.Atk,
                    Detail = BehaviourDetail.Hit,
                    MotionDetail = MotionDetail.J,
                    MotionDetailDefault = MotionDetail.N,
                    EffectRes = "",
                    Script = "20DmgUp50",
                    Desc = "若骰子基础值不低于20则造成伤害+50%",
                    ActionScript = "",
                },
                card = card
            };
            dice.AddAbility(new DiceCardAbility_20DmgUp50());
            if ((temp.GetDiceMax() + temp.GetDiceMin()) / 2 >= 15)
            {
                dice.SetIndex(1);
                card.AddDice(dice);
            }
            else
            {
                dice.SetIndex(0);
                card.AddDiceFront(dice);
            }
        }

        public class DiceCardAbility_20DmgUp50 : DiceCardAbilityBase
        {
            public override void OnRollDice()
            {
                if (behavior.DiceResultValue >= 20)
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = 50 });
                }
            }
        }

    }
}
