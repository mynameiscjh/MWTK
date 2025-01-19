using Don_Eyuil.Don_Eyuil.Player.Buff;
using System;
using System.Collections.Generic;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_76 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[战斗开始]对自身施加3层[流血]并使两名我方角色获得1层[振奋]自身每激活了一种[硬血术]书页便额外施加1层[强壮](至多2层)";
        public override void OnStartBattle()
        {
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Blurry, 3, owner);
            var temp_list = new List<BattleUnitModel>(BattleObjectManager.instance.GetAliveList(owner.faction));
            for (int i = 0; i < 2; i++)
            {
                if (temp_list.Count <= 0)
                {
                    return;
                }
                if (BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(owner) == null)
                {
                    return;
                }
                var temp = RandomUtil.SelectOne(temp_list);
                temp_list.Remove(temp);
                temp.bufListDetail.AddKeywordBufByCard(KeywordBuf.BreakProtection, 1, owner);
                temp.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, Math.Min(2, BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(owner).ActivatedNum), owner);

            }
        }
    }
}
