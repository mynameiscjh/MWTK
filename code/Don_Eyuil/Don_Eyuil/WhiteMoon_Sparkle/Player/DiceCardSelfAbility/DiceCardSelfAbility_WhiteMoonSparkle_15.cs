using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;
using System.Linq;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_WhiteMoonSparkle_15 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]恢复1点光芒 若目标使用的书页指定了友方角色则额外恢复抽取1张书页(同时计入转移目标前后) \r\n" +
            "本书页根据当前所装备的副武器获得额外效果\r\n" +
            "泉之龙/秋之莲:[使用时]将1颗斩击骰子（3～4）置入书页末尾\r\n" +
            "千斤弓:[使用时]下一幕使自身获得1层迅捷\r\n" +
            "月之剑:[使用时]使本书页命中时使情感硬币最少的我方角色获得1点正面情感\r\n" +
            "埃尤尔之血:[使用时]使本书页命中时下一幕对目标施加1层[无法凝结的血]\r\n";
        public override void OnUseCard()
        {

            owner.cardSlotDetail.RecoverPlayPoint(1);

            if (card.target.cardSlotDetail.cardAry.Exists(x => x.target.faction == this.owner.faction))
            {
                owner.allyCardDetail.DrawCards(1);
            }

            if (BattleUnitBuf_Sparkle.Instance.SubWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Year)))
            {
                var temp = new BattleDiceBehavior()
                {
                    behaviourInCard = new LOR_DiceSystem.DiceBehaviour()
                    {
                        Min = 3,
                        Dice = 4,
                        Type = LOR_DiceSystem.BehaviourType.Atk,
                        Detail = LOR_DiceSystem.BehaviourDetail.Slash,
                        MotionDetail = LOR_DiceSystem.MotionDetail.J,
                        MotionDetailDefault = LOR_DiceSystem.MotionDetail.N,
                        EffectRes = "",
                        Script = "",
                        Desc = "",
                        ActionScript = "",
                    }
                };
                temp.SetIndex(card.card.GetBehaviourList().Count);
                card.AddDice(temp);
            }

            if (BattleUnitBuf_Sparkle.Instance.SubWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Bow)))
            {
                owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 1, owner);
            }
        }

        public override void OnSucceedAttack()
        {
            if (BattleUnitBuf_Sparkle.Instance.SubWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Sword)))
            {
                var temp = BattleObjectManager.instance.GetAliveList(owner.faction);
                temp.Sort((x, y) => x.emotionDetail.AllEmotionCoins.Count - y.emotionDetail.AllEmotionCoins.Count);
                if (temp.Count > 0)
                {
                    temp.First().emotionDetail.CreateEmotionCoin(EmotionCoinType.Positive);
                }
            }

            if (BattleUnitBuf_Sparkle.Instance.SubWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Blood)))
            {
                BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_UncondensableBlood>(card.target, 1);
            }
        }

    }
}
