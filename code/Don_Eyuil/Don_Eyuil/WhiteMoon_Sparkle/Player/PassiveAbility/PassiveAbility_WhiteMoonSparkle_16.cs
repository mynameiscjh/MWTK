using HarmonyLib;
using LOR_DiceSystem;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.PassiveAbility
{
    public class PassiveAbility_WhiteMoonSparkle_16 : PassiveAbilityBase_Don_Eyuil
    {
        public static string Desc = "受到单方面攻击时将等同于目标书页骰子数等量的招架骰子(1~2[拼点失败]不承受反震伤害)迎击\r\n下令战斗时获得1颗远程反击骰子(斩击4~8使目标当前骰子无法再次投掷)\r\n(本被动不可转移)";

        public override void OnStartBattle()
        {
            var temp = ItemXmlDataList.instance.GetCardItem(MyId.Card_反击通用书页);
            var card = BattleDiceCardModel.CreatePlayingCard(temp);
            owner.cardSlotDetail.keepCard.AddBehaviour(card, card.CreateDiceCardBehaviorList()[1]);
        }

        public class DiceCardAbility_不承受反震伤害 : DiceCardAbilityBase
        {
            [HarmonyPatch(typeof(BattleDiceBehavior), "GiveDeflectDamage")]
            [HarmonyPrefix]
            public static bool BattleDiceBehavior_GiveDeflectDamage_Pre(BattleDiceBehavior targetDice)
            {
                if (targetDice.abilityList.Exists(x => x.GetType() == typeof(DiceCardAbility_不承受反震伤害)))
                {
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(BattleOneSidePlayManager), "StartOneSidePlay")]
        [HarmonyPrefix]
        public static bool BattleOneSidePlayManager_StartOneSidePlay_Pre(BattlePlayingCardDataInUnitModel card)
        {
            if (card.target.passiveDetail.HasPassive<PassiveAbility_WhiteMoonSparkle_16>())
            {

                var xmlData = new DiceCardXmlInfo()
                {
                    workshopID = TKS_BloodFiend_Initializer.packageId,
                    workshopName = "虚构的书页",
                    Artwork = "DonEyuil_1.png",
                    Rarity = Rarity.Common,
                    Spec = new DiceCardSpec { Ranged = CardRange.Near, Cost = 0, affection = CardAffection.One, emotionLimit = 0 },
                    Chapter = 7,
                    _id = 139186,
                };
                for (int i = 0; i < card.cardBehaviorQueue.Count; i++)
                {
                    xmlData.DiceBehaviourList.Add(new DiceBehaviour
                    {
                        Min = 1,
                        Dice = 2,
                        Type = BehaviourType.Def,
                        Detail = BehaviourDetail.Guard,
                        MotionDetail = MotionDetail.Z,
                        MotionDetailDefault = MotionDetail.N,
                        Script = "不承受反震伤害"
                    });
                }

                var temp = BattleDiceCardModel.CreatePlayingCard(xmlData);
                var parryCard = new BattlePlayingCardDataInUnitModel
                {
                    owner = card.target,
                    card = temp,
                    target = card.owner,
                    earlyTarget = card.owner,
                    earlyTargetOrder = 0,
                    speedDiceResultValue = 0,
                    slotOrder = 0,
                    targetSlotOrder = 0,
                    cardAbility = temp.CreateDiceCardSelfAbilityScript()
                };
                //parryCard.cardAbility.card = parryCard;
                //parryCard.cardAbility.OnApplyCard();
                parryCard.ResetCardQueue();
                if (card.owner.faction == Faction.Enemy)
                {
                    BattleParryingManager.Instance.StartParrying(card, parryCard);
                }
                else
                {
                    BattleParryingManager.Instance.StartParrying(parryCard, card);
                }
                return false;
            }

            return true;
        }
    }
}
