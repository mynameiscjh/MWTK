using Don_Eyuil.Don_Eyuil.Player.Buff;
using LOR_DiceSystem;
using System.Collections.Generic;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_136 : DiceCardSelfAbilityBase
    {
        public static string Desc = "仅限装备了血弓时装备\r\n[使用时]若自身上一张书页为[血鞭]则在本书页末尾置入一颗打击骰子(4-8)";

        public override bool OnChooseCard(BattleUnitModel owner)
        {
            var buf = BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(owner);

            if (buf == null || buf.Bow == null)
            {
                return false;
            }

            return base.OnChooseCard(owner);
        }

        public override void OnUseCard()
        {
            var list = owner.cardHistory.GetCurrentRoundCardList(StageController.Instance.RoundTurn);

            if (list != null && list.FindIndex(x => x == card) != -1 && list.Count >= list.FindIndex(x => x == card) && list[+1].card.GetID() == MyId.Card_堂埃尤尔派硬血术8式_血鞭_2)
            {
                var temp = new BattleDiceBehavior()
                {
                    behaviourInCard = new DiceBehaviour()
                    {
                        Min = 4,
                        Dice = 8,
                        Type = BehaviourType.Atk,
                        Detail = BehaviourDetail.Hit,
                        MotionDetail = MotionDetail.H,
                        MotionDetailDefault = MotionDetail.N,
                    },
                    card = card,
                    abilityList = new List<DiceCardAbilityBase>(),
                };

                card.AddDice(temp);
            }
        }
    }
}
