using HarmonyLib;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.PassiveAbility
{
    public class PassiveAbility_WhiteMoonSparkle_16 : PassiveAbilityBase_Don_Eyuil
    {
        public static string Desc = "受到单方面攻击时将等同于目标书页骰子数等量的招架骰子(1~2[拼点失败]不承受反震伤害)迎击\r\n下令战斗时获得1颗远程反击骰子(斩击4~8使目标当前骰子无法再次投掷)\r\n(本被动不可转移)";

        public override void OnStartBattle()
        {
            var temp = ItemXmlDataList.instance.GetCardItem(MyId.未实现id);
            var card = BattleDiceCardModel.CreatePlayingCard(temp);
            owner.cardSlotDetail.keepCard.AddBehaviour(card, card.CreateDiceCardBehaviorList()[0]);
        }

        [HarmonyPatch(typeof(BattleOneSidePlayManager), "StartOneSidePlay")]
        [HarmonyPrefix]
        public static bool BattleOneSidePlayManager_StartOneSidePlay_Pre(BattlePlayingCardDataInUnitModel card)
        {
            if (card.target.passiveDetail.HasPassive<PassiveAbility_WhiteMoonSparkle_16>())
            {
                MyTools.未实现提醒();
                var temp = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.未实现id));
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
                parryCard.cardAbility.card = parryCard;
                parryCard.cardAbility.OnApplyCard();
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
