using System;
using System.Collections.Generic;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.PassiveAbility
{
    public class PassiveAbility_WhiteMoonSparkle_17 : PassiveAbilityBase_Don_Eyuil
    {
        public static string Desc = "战斗开始自身每有一张书页指向同一目标便使自身本幕对其造成的伤害+1(至多+3)";

        public Dictionary<BattleUnitModel, List<BattlePlayingCardDataInUnitModel>> dic = new Dictionary<BattleUnitModel, List<BattlePlayingCardDataInUnitModel>>();

        public override void OnStartBattle()
        {
            foreach (var item in owner.cardSlotDetail.cardAry)
            {
                if (dic.TryGetValue(item.target, out var temp))
                {
                    if (temp == null)
                    {
                        temp = new List<BattlePlayingCardDataInUnitModel>();
                    }
                }
                else
                {
                    dic.Add(item.target, new List<BattlePlayingCardDataInUnitModel>());
                }
                dic[item.target].Add(item);
            }

            foreach (var item in dic.Values)
            {
                foreach (var card in item)
                {
                    card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { dmg = Math.Min(3, item.Count) });
                }
            }
        }
    }
}
