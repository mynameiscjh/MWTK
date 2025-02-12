using System.Linq;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_WhiteMoonSparkle_16 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[战斗开始]我方角色首次受到单方面攻击时给予其1颗突刺反击(4～8)\r\n[使用时]丢弃一张书页(优先同名)若丢弃的为同名书页 则将书页中的首颗进攻型骰子置入本书页末尾\r\n";

        public override void OnStartBattle()
        {
            foreach (var item in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                var card = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_反击通用书页));
                item.cardSlotDetail.keepCard.AddBehaviour(card, card.CreateDiceCardBehaviorList()[0]);
            }
        }

        public override void OnUseCard()
        {
            var temp = owner.allyCardDetail.GetHand();
            temp.Sort((x, y) =>
            {
                if (x.GetID() == card.card.GetID())
                {
                    return -1;
                }
                if (y.GetID() == card.card.GetID())
                {
                    return 1;
                }
                return 0;
            });
            if (temp.Count > 0)
            {
                owner.allyCardDetail.ExhaustACard(temp.First());
                if (temp.First().GetID() == card.card.GetID())
                {
                    card.AddDice(temp.First().CreateDiceCardBehaviorList().First());
                }
            }
        }
    }
}
