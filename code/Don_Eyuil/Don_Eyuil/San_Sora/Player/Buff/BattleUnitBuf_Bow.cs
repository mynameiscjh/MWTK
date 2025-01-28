using LOR_BattleUnit_UI;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Don_Eyuil.San_Sora.Player.Buff
{
    public class BattleUnitBuf_Bow : BattleUnitBuf_SanHardBlood
    {
        public static string Desc = "本速度骰子中使用的书页将变为远程书页(无法对E.G.O书页与群体生效)且进攻型骰子最小值+2\r\n自身每幕第一张使用的书页施加的”流血”层数额外+1";

        public BattleUnitBuf_Bow(SpeedDiceUI dice) : base(dice)
        {
            dice.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = TKS_BloodFiend_Initializer.ArtWorks["血弓骰子"];
        }
        public List<BattlePlayingCardDataInUnitModel> changedCard = new List<BattlePlayingCardDataInUnitModel>();
        public override void OnStartBattle()
        {
            if (Card != null)
            {

                if (Card.card.XmlData.Spec.Ranged == LOR_DiceSystem.CardRange.Near)
                {
                    //_owner.view.charAppearance.SetAltMotion(ActionDetail.Penetrate, ActionDetail.Fire);
                    Card.card.XmlData.Spec.Ranged = LOR_DiceSystem.CardRange.Far;
                    changedCard.Add(Card);
                }
            }
        }
        public override void OnRoundEnd()
        {
            foreach (var card in changedCard)
            {
                card.card.XmlData.Spec.Ranged = LOR_DiceSystem.CardRange.Near;
            }
            changedCard.Clear();
        }
    }
}
