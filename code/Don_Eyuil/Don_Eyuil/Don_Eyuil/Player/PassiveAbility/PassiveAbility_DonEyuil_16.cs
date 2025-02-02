﻿using HarmonyLib;
using System;

namespace Don_Eyuil.Don_Eyuil.Player.PassiveAbility
{
    public class PassiveAbility_DonEyuil_16 : PassiveAbilityBase
    {
        public override string debugDesc => "自身所有第一颗进攻型骰子命中带有\"流血\"的目标时将恢复等同于自身骰子基础值的体力\r\n在一幕中自身每恢复3点体力便在下一幕获得1层\"伤害提升\"(至多3层)\r\n";

        public bool fl = false;

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            fl = true;
        }

        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (behavior.card.target.bufListDetail.HasBuf<BattleUnitBuf_bleeding>() && fl)
            {
                owner.RecoverHP(2);
                fl = false;
            }
        }
        public static int recoverHpCount = 0;

        [HarmonyPatch(typeof(BattleUnitModel), "RecoverHP")]
        [HarmonyPrefix]
        public static void BattleUnitModel_RecoverHP_Pre(int v)
        {
            recoverHpCount += v;
        }

        public override void OnRoundEnd()
        {
            owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.DmgUp, Math.Min(recoverHpCount / 3, 3));
            recoverHpCount = 0;
        }
    }
}
